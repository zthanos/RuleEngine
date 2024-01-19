using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using RuleEngineTester.RuleEngine.ErrorHandling;
using System.Threading.Tasks.Dataflow;

namespace RuleEngineTester.RuleEngine.WeaklyTyped;

public class Rule : IRule
{
    private readonly ILogger _logger;
    private JSchema? _typeToApplyRule;
    private string? _applyToType;
    private List<RuleCondition> _conditons = new();
    private List<RuleAction> _actions = new();

    public Rule(ILogger logger)
    {
        _logger = logger;
    }
    public void SetType(string type, JSchema jsonSchema)
    {
        _applyToType = type;
        _typeToApplyRule = jsonSchema;
    }

    public void AddCondition(RuleCondition condition)
    {
        _conditons.Add(condition);

    }

    public void AddAction(RuleAction action) => _actions.Add(action);
    public void ExecuteAction(JObject target)
    {

        foreach (var action in _actions)
        {
            // Access the property in the JObject
            JToken propertyValue = target.Property(action.PropertyName)?.Value!;

            // Modify the property value based on the action
            action.Execute(target);

            // Update the property in the JObject
            //if (target.Property(action.PropertyName) is JProperty property)
            //{
            //    property.Value = newValue;
            //}
        }
        _logger.LogInformation($"Action applied to: {target}");
    }

    //public void SetPropertyValue(JObject target, string propertyName, object value)
    //{

    //}


    public RuleExecutionResult ApplyRule(string jsonData)
    {
        JObject jObj = JObject.Parse(jsonData);
        //_logger.LogDebug($"Type : {_applyToType} \n Data: \n {jsonData}");

        JsonTextReader reader = new JsonTextReader(new StringReader(jsonData));
        JSchemaValidatingReader validatingReader = new JSchemaValidatingReader(reader);
        validatingReader.Schema = _typeToApplyRule;
        validatingReader.ValidationEventHandler += (sender, args) =>
        {
            throw new RuleEngineException($"An error occurred while validating the JSON.\n{args.Message}");
        };
        // Deserialize using the validating reader
        JsonSerializer serializer = new JsonSerializer();
        var data = serializer.Deserialize<JObject>(validatingReader);
        _logger.LogInformation($"Apply rule to: {data}");

        if (data is null)
        {
            throw new RuleEngineException("Invalid Object.");
        }
        bool ruledPassed = false;
        Dictionary<string, bool> conditionResult = new();
        foreach (RuleCondition condition in _conditons)
        {
            var res = condition.Evaluate(data);
            conditionResult.Add(condition.ExpressionToExecute, res);
            if (res)
            {
                ruledPassed = true;
                // Condition is satisfied
                _logger.LogInformation($"Condition '{condition.ExpressionToExecute}' is satisfied.");
            }
            else
            {
                // Condition is not satisfied
                _logger.LogInformation($"Condition '{condition.ExpressionToExecute}' is not satisfied.");
            }
        }
        ruledPassed = conditionResult.Values.All(w => w == true);
        if (ruledPassed)
        {
            ExecuteAction(data);
        }


        return new RuleExecutionResult(data, ruledPassed, conditionResult);
    }

    public JSchema GetApplyToType() => _typeToApplyRule!;
    public string GetApplyToTypeName() => _applyToType!;



    public static RuleBuilder CreateBuilder(ILogger logger) => new RuleBuilder(logger);
}

public record RuleExecutionResult(JObject Target, bool Succeed, Dictionary<string, bool> ConditionResults);