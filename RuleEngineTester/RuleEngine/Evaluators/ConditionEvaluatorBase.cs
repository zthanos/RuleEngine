using System.Linq.Expressions;

namespace RuleEngineTester.RuleEngine.Evaluators;

public abstract class ConditionEvaluatorBase<T> : IConditionEvaluator<T>
{
    protected object GetPropertyValue(T target, string propertyName)
    {
        var propertyInfo = target!.GetType().GetProperty(propertyName);
        return propertyInfo?.GetValue(target)!;
    }

    protected object ConcertValueToType(object expectedValue, Type targetType)
    {
        var converter = System.ComponentModel.TypeDescriptor.GetConverter(targetType);
        if (converter.IsValid(expectedValue))
        {
            return converter.ConvertFrom(expectedValue)!;
        }
        else
        {
            // Handle conversion failure
            throw new InvalidOperationException($"{expectedValue} ConvertValueToType failed {targetType.ToString()}");
        }
    }

    public abstract bool Evaluate(T typedTarget);

    public abstract Expression<Func<T, bool>> BuildExpression(ParameterExpression parameter);

}
