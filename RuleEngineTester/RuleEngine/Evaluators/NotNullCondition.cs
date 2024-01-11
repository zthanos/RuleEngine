using System.Linq.Expressions;

namespace RuleEngineTester.RuleEngine.Evaluators;

public class NotNullCondition<T> : ConditionEvaluatorBase<T>
{
    private readonly string propertyName;

    public NotNullCondition(string propertyName)
    {
        this.propertyName = propertyName;
    }

    public override bool Evaluate(T typedTarget)
    {
        return GetPropertyValue(typedTarget, propertyName) != null;
    }

    public override Expression<Func<T, bool>> BuildExpression(ParameterExpression parameter)
    {
        return Expression.Lambda<Func<T, bool>>(
            Expression.NotEqual(
                Expression.Property(parameter, propertyName),
                Expression.Constant(null, typeof(object))  // Explicitly set the constant type to object
            ),
            parameter
        );
    }



}
