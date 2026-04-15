using LsRuleEngine.Builders;
using LsRuleEngine.ErrorHandling;
using LsRuleEngine.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace LsRuleEngine;

public class LsRule(string ruleName, ILogger logger) : IRule
{
    private readonly ILogger _logger = logger;
    private JSchema? _typeToApplyRule;
    private string? _applyToType;
    private readonly string _ruleName = ruleName;
    private readonly List<RuleCondition> _conditions = [];
    private readonly List<RuleAction> _actions = [];
    public string Name => _ruleName;
    public IEnumerable<RuleCondition> Conditions => _conditions;
    public IEnumerable<RuleAction> Actions => _actions;
    public string TypeToApplyRule => _applyToType ?? string.Empty;


    //public void AddName(string name) => Name = name;

    public void SetType(string type, JSchema jsonSchema)
    {
        _applyToType = type;
        _typeToApplyRule = jsonSchema;
    }

    public void AddCondition(RuleCondition condition)
    {
        _conditions.Add(condition);

    }

    public void AddAction(RuleAction action) => _actions.Add(action);
    public void ExecuteAction(JObject target)
    {

        foreach (var action in _actions)
        {
            action.Execute(target);

        }
        _logger.LogInformation("Action applied to: {target}", target);
    }

    //public void SetPropertyValue(JObject target, string propertyName, object value)
    //{

    //}


    public RuleExecutionResult ApplyRule(string jsonData)
    {
        JObject jObj = JObject.Parse(jsonData);
        //_logger.LogDebug($"Type : {_applyToType} \n Data: \n {jsonData}");

        JsonTextReader reader = new(new StringReader(jsonData));
        JSchemaValidatingReader validatingReader = new(reader)
        {
            Schema = _typeToApplyRule
        };
        validatingReader.ValidationEventHandler += (sender, args) =>
        {
            _logger.LogDebug("An error occurred while validating the JSON.\n{args}", args.Message);
            //  throw new RuleEngineException($"An error occurred while validating the JSON.\n{args.Message}");
        };
        // Deserialize using the validating reader
        JsonSerializer serializer = new();
        var data = serializer.Deserialize<JObject>(validatingReader);
        _logger.LogInformation("Apply rule to: {data}", data);

        if (data is null)
        {
            throw new RuleEngineException("Invalid Object.");
        }
        bool ruledPassed = false;
        var conditionResults = new List<ConditionResult>();
        foreach (RuleCondition condition in _conditions)
        {
            var res = condition.Evaluate(data);
            conditionResults.Add(new ConditionResult(condition.ExpressionToExecute, res));
            if (res)
            {
                ruledPassed = true;
                // Condition is satisfied
                _logger.LogInformation("Condition '{condition}' is satisfied.", condition.ExpressionToExecute);
            }
            else
            {
                // Condition is not satisfied
                _logger.LogInformation("Condition '{condition}' is not satisfied.", condition.ExpressionToExecute);
            }
        }
        ruledPassed = conditionResults.All(w => w.result == true);
        if (ruledPassed)
        {
            ExecuteAction(data);
        }


        return new RuleExecutionResult(data, ruledPassed, conditionResults);
    }

    public JSchema GetApplyToType() => _typeToApplyRule!;
    public string GetApplyToTypeName() => _applyToType!;



    public static RuleBuilder CreateBuilder(string ruleName, ILogger logger) => new(ruleName, logger);
}

public record RuleExecutionResult(JObject Target, bool Succeed, IEnumerable<ConditionResult> ConditionResults);
public record ConditionResult(string condition, bool result);