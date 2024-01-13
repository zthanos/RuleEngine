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
        object parsedValue = null;

        if (converter.IsValid(expectedValue))
        {
            parsedValue = converter.ConvertFrom(expectedValue);
        }
        else
        {
            // Handle conversion failure
        }

        return parsedValue;
    }

    public abstract bool Evaluate(T typedTarget);

    public abstract Expression<Func<T, bool>> BuildExpression(ParameterExpression parameter);

}
