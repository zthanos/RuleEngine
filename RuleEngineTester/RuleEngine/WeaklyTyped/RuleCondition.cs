using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RuleEngineTester.RuleEngine.Conditions;
using RuleEngineTester.RuleEngine.ErrorHandling;
using System.Linq.Expressions;

namespace RuleEngineTester.RuleEngine.WeaklyTyped;

public class RuleCondition
{
    private readonly ILogger _logger;
    private  string _description;
    public string ExpressionToExecute { get {  return _description; } }   
    public RuleCondition(ILogger logger)
    {
        _logger = logger;
        _description = string.Empty;
    }
    public List<RuleConditionDefinition> _conditions = new();


    public List<RuleConditionDefinition> Conditions => _conditions;
    public void Add(RuleConditionDefinition ruleConditionDefinition)
    {
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
        _description = c!.ToString();
        return Expression.Lambda<Func<JObject, bool>>(c, targetParameter).Compile();
    }
    public static ConditionBuilder CreateBuilder(ILogger logger)
    {
        return new ConditionBuilder(logger);
    }

}

public class ConditionEvaluator
{
    private readonly ILogger _logger;
    public ConditionEvaluator(ILogger logger)
    {
        _logger = logger;
    }

    public static Expression CreateConditionEvaluator(JObject target, RuleConditionDefinition condition)
    {
        var parameter = Expression.Parameter(typeof(JObject), "item");
        var propertyAccess = Expression.Call(parameter, "Property", null, Expression.Constant(condition.PropertyName));
        var a = target.Properties().First(w => w.Name == condition.PropertyName);
        var propertyData = (target as JObject).Properties().First(w=> w.Name == condition.PropertyName).Value;
        if (propertyData is not JValue)
        {
            throw new RuleEngineException("Unable to retrieve type");
        }
        var value = ((JValue)propertyData).Value;
        var conditionValue = Expression.Convert(Expression.Constant(condition.Value), value!.GetType());
       
        return condition.ConditionType switch
        {
            ConditionType.Null => Expression.Equal(Expression.Constant(value), Expression.Constant(null)),
            ConditionType.NotNull => Expression.NotEqual(Expression.Constant(value), Expression.Constant(null)),
            ConditionType.Empty => Expression.Equal(Expression.Constant(value), Expression.Constant(string.Empty)),
            ConditionType.NotEmpty => Expression.NotEqual(Expression.Constant(value), Expression.Constant(string.Empty)),
            ConditionType.GreaterThanOrEquals => Expression.GreaterThanOrEqual(Expression.Constant(value), conditionValue),
            ConditionType.GreaterThan => Expression.GreaterThan(Expression.Constant(value), conditionValue),
            ConditionType.LessThan => Expression.LessThan(Expression.Constant(value), conditionValue),
            ConditionType.LessThanOrEquals => Expression.LessThanOrEqual(Expression.Constant(value), conditionValue),
            ConditionType.Equals => Expression.Equal(Expression.Constant(value), conditionValue),
            ConditionType.NotEquals => Expression.NotEqual(Expression.Constant(value), conditionValue),

            // Add more cases for other condition types
            _ => throw new NotSupportedException($"Condition type '{condition.ConditionType}' is not supported.")
        };
    }
}
