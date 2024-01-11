using System.Linq.Expressions;

namespace RuleEngineTester.RuleEngine.Evaluators;

public interface IConditionEvaluator<T>
{
    bool Evaluate(T typedTarget);
    Expression<Func<T, bool>> BuildExpression(ParameterExpression parameter);
}
