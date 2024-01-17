using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json;
using RuleEngineTester.RuleEngine.Conditions;
using System.Linq.Expressions;
using RuleEngineTester.RuleEngine.ErrorHandling;
using RuleEngineTester.RuleEngine.Evaluators;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;
using System.Data.Common;

namespace RuleEngineTester.RuleEngine.WeaklyTyped;

public class RuleCondition
{
    private readonly ILogger _logger;
    private List<RuleConditionDefinition> _conditions = new();


    public List<RuleConditionDefinition> Conditions => _conditions;
    public void Init(RuleConditionDefinition ruleConditionDefinition)
    {
        _conditions.Add(ruleConditionDefinition);
    }
    public void And(RuleConditionDefinition ruleConditionDefinition)
    {
        _conditions.Add(ruleConditionDefinition);
    }

    public void Or(RuleConditionDefinition ruleConditionDefinition)
    {
        _conditions.Add(ruleConditionDefinition);
    }



    public static ConditionBuilder CreateBuilder(ILogger logger)
    {
        return new ConditionBuilder(logger);
    }

    public class ConditionBuilder
    {
        private RuleCondition _condition;

        private readonly ILogger _logger;

        public ConditionBuilder(ILogger logger)
        {
            _logger = logger;
            _condition = new();
        }

        public ConditionBuilder InitCondition(string property, ConditionType condition, Type type, object? value)
        {
            RuleConditionDefinition ruleConditionDefinition = new(
                property,
                condition,
                type,
                value,
                OperatorType.And);
            _condition.Init(ruleConditionDefinition);
            return this;
        }
        public ConditionBuilder AndCondition(string property, ConditionType condition, Type type, object? value)
        {
            RuleConditionDefinition ruleConditionDefinition = new(
               property,
               condition,
               type,
               value,
               OperatorType.And);
            _condition.And(ruleConditionDefinition);

            return this;
        }
        public ConditionBuilder OrCondition(string property, ConditionType condition, Type type, object? value)
        {
            RuleConditionDefinition ruleConditionDefinition = new(
               property,
               condition,
               type,
               value,
               OperatorType.Or);
            _condition.Or(ruleConditionDefinition);

            return this;
        }
        public RuleCondition Build()
        {
            return _condition;
        }
    }

}

public static class ConditionEvaluator
{
    private static object? GetPorpertyValue(string propertyName, object target)
    {
        var property = target.GetType().GetProperty(propertyName);
        if (property == null)
            throw new RuleEngineException($"Property {propertyName} not found in {target}");
        return property.GetValue(target, null);
    }

    private static JObject ConvertToObject(JSchema schema, string jsonData)
    {
        JObject jObj = JObject.Parse(jsonData);
        JsonTextReader reader = new JsonTextReader(new StringReader(jsonData));
        JSchemaValidatingReader validatingReader = new JSchemaValidatingReader(reader);
        validatingReader.Schema = schema;
        validatingReader.ValidationEventHandler += (sender, args) =>
        {
            throw new RuleEngineException($"An error occurred while validating the JSON.\n{args.Message}");
        };
        // Deserialize using the validating reader
        JsonSerializer serializer = new JsonSerializer();
        return serializer.Deserialize<JObject>(validatingReader);
    }
    public static Func<object, bool> EvaluateConditions(JSchema schema, string jsonData, IList<RuleConditionDefinition> conditions)
    {
        var target = ConvertToObject(schema, jsonData);
        Expression c = null;
        foreach (var condition in conditions)
        {
            if (c == null)
                c = ConditionEvaluator.CreateConditionEvaluator(target, condition);
            else if (condition.ConditionOperator == OperatorType.And)
            {
               c = Expression.AndAlso(c, ConditionEvaluator.CreateConditionEvaluator(target, condition));
            }
            else
            {
                c = Expression.OrElse(c, ConditionEvaluator.CreateConditionEvaluator(target, condition));
            }
        }
        return Expression.Lambda<Func<object, bool>>(c).Compile();
    }

    public static Expression CreateConditionEvaluator(object? target, RuleConditionDefinition condition)
    {
        var parameter = Expression.Parameter(typeof(JObject), "item");
        var propertyAccess = Expression.Call(parameter, "Property", null, Expression.Constant(condition.PropertyName));

        return condition.ConditionType switch
        {
            ConditionType.Null => Expression.Equal(propertyAccess, Expression.Constant(null)),
            ConditionType.NotNull => Expression.NotEqual(propertyAccess, Expression.Constant(null)),
            ConditionType.Empty => Expression.Equal(propertyAccess, Expression.Constant(string.Empty)),
            ConditionType.NotEmpty => Expression.NotEqual(propertyAccess, Expression.Constant(string.Empty)),
            ConditionType.GreaterThanOrEquals => Expression.GreaterThanOrEqual(propertyAccess, Expression.Constant(condition.Value)),
            ConditionType.GreaterThan => Expression.GreaterThan(propertyAccess, Expression.Constant(condition.Value)),
            ConditionType.LessThan => Expression.LessThan(propertyAccess, Expression.Constant(condition.Value)),
            ConditionType.LessThanOrEquals => Expression.LessThanOrEqual(propertyAccess, Expression.Constant(condition.Value)),

            // Add more cases for other condition types
            _ => throw new NotSupportedException($"Condition type '{condition.ConditionType}' is not supported.")
        };
    }
}


//public interface IConditionEvaluator
//{
//    bool Evaluate(object typedTarget);
//    Expression<Func<object, bool>> BuildExpression(object target, ParameterExpression parameter);

////};
//public static class ConditionEvaluator //: IConditionEvaluator
//{

//    //protected object ConcertValueToType(object expectedValue, Type targetType)
//    //{
//    //    var converter = System.ComponentModel.TypeDescriptor.GetConverter(targetType);
//    //    if (converter.IsValid(expectedValue))
//    //    {
//    //        return converter.ConvertFrom(expectedValue)!;
//    //    }
//    //    else
//    //    {
//    //        // Handle conversion failure
//    //        throw new InvalidOperationException($"{expectedValue} ConvertValueToType failed {targetType.ToString()}");
//    //    }
//    //}

//    //public abstract bool Evaluate(object typedTarget);
//    //public abstract Expression<Func<object, bool>> BuildExpression(object target, ParameterExpression parameter);
 
//    public static Expression CreateConditionEvaluator(object? target, RuleConditionDefinition condition)
//    {
//        var parameter = Expression.Parameter(typeof(JObject), "item");
//        var propertyAccess = Expression.Call(parameter, "Property", null, Expression.Constant(condition.PropertyName));

//        return condition.ConditionType switch
//        {
//            ConditionType.Null => Expression.Equal(propertyAccess, Expression.Constant(null)),
//            ConditionType.NotNull => Expression.NotEqual(propertyAccess, Expression.Constant(null)),
//            ConditionType.Empty => Expression.Equal(propertyAccess, Expression.Constant(string.Empty)),
//            ConditionType.NotEmpty => Expression.NotEqual(propertyAccess, Expression.Constant(string.Empty)),
//            ConditionType.GreaterThanOrEquals => Expression.GreaterThanOrEqual(propertyAccess, Expression.Constant(condition.Value)),
//            ConditionType.GreaterThan => Expression.GreaterThan(propertyAccess, Expression.Constant(condition.Value)),
//            ConditionType.LessThan => Expression.LessThan(propertyAccess, Expression.Constant(condition.Value)),
//            ConditionType.LessThanOrEquals => Expression.LessThanOrEqual(propertyAccess, Expression.Constant(condition.Value)),

//            // Add more cases for other condition types
//            _ => throw new NotSupportedException($"Condition type '{condition.ConditionType}' is not supported.")
//        };
//    }
//}