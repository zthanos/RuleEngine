using LsRuleEngine.Enums;
using LsRuleEngine.ErrorHandling;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace LsRuleEngine.Evaluators;

public class ConditionEvaluator
{
    public static Expression CreateConditionEvaluator(JObject target, RuleConditionDefinition condition)
    {
        var parameter = Expression.Parameter(typeof(JObject), "item");
        var propertyAccess = Expression.Call(parameter, "Property", null, Expression.Constant(condition.PropertyName));
        var a = target.Properties().First(w => w.Name == condition.PropertyName);
        var propertyData = (target as JObject).Properties().First(w => w.Name == condition.PropertyName).Value;
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
