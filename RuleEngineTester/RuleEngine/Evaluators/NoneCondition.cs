using System.Linq.Expressions;

namespace RuleEngineTester.RuleEngine.Evaluators;

// Add more condition classes as needed...

public class NoneCondition<T> : ConditionEvaluatorBase<T>
{
    public override bool Evaluate(T typedTarget)
    {
        return true;
    }

    public override Expression<Func<T, bool>> BuildExpression(ParameterExpression parameter)
    {
        // Build and return the expression for null condition
        return Expression.Lambda<Func<T, bool>>(
            Expression.Equal(
                Expression.Property(parameter, ""),
                Expression.Constant(null)
            ),
            parameter
        );
    }
}
