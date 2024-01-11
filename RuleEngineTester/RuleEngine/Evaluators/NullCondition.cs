using System.Linq.Expressions;

namespace RuleEngineTester.RuleEngine.Evaluators;

public class NullCondition<T> : ConditionEvaluatorBase<T>
{
    private readonly string propertyName;

    public NullCondition(string propertyName)
    {
        this.propertyName = propertyName;
    }

    public override bool Evaluate(T typedTarget)
    {
        return GetPropertyValue(typedTarget, propertyName) == null;
    }

    public override Expression<Func<T, bool>> BuildExpression(ParameterExpression parameter)
    {
        // Build and return the expression for null condition
        return Expression.Lambda<Func<T, bool>>(
            Expression.Equal(
                Expression.Property(parameter, propertyName),
                Expression.Constant(null)
            ),
            parameter
        );
    }
}
