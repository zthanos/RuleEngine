using LsRuleEngine.Builders;
using LsRuleEngine.Enums;
using LsRuleEngine.Evaluators;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace LsRuleEngine;

public class RuleCondition(ILogger logger)
{
    private readonly ILogger _logger = logger;
    private  string _description = string.Empty;
    public string ExpressionToExecute { get {  return _description; } }   
    public string Description => _description;

    public List<RuleConditionDefinition> _conditions = [];


    public List<RuleConditionDefinition> Conditions => _conditions;
    public void Add(RuleConditionDefinition ruleConditionDefinition)
    {
        _description = $"{ruleConditionDefinition.ConditionOperator} {ruleConditionDefinition.PropertyName} {ruleConditionDefinition.ConditionType} {ruleConditionDefinition.Value}";
        _conditions.Add(ruleConditionDefinition);
    }

    public bool Evaluate(JObject target)
    {

        return EvaluateConditions(target, this).Invoke(target);
    }
    private Func<JObject, bool> EvaluateConditions(JObject target, RuleCondition condition)
    {
        // var target = ConvertToObject(schema, jsonData);
        var targetParameter = Expression.Parameter(typeof(JObject), "target");

        Expression? c = null;
        foreach (var definition in condition.Conditions)
        {
            if (c == null)
                c = ConditionEvaluator.CreateConditionEvaluator(target, definition);
            else if (definition.ConditionOperator == OperatorType.And)
            {
                
                c = Expression.AndAlso(c, ConditionEvaluator.CreateConditionEvaluator(target, definition));
            }
            else
            {
                c = Expression.OrElse(c, ConditionEvaluator.CreateConditionEvaluator(target, definition));
            }
        }
        //_description = c!.ToString();
        _logger.LogDebug("condition: {condition}", c!.ToString());
        return Expression.Lambda<Func<JObject, bool>>(c, targetParameter).Compile();
    }
    public static ConditionBuilder CreateBuilder(ILogger logger)
    {
        return new ConditionBuilder(logger);
    }

}
