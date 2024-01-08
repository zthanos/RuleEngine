namespace RuleEngineTester.RuleEngine.Evaluators;

public interface IConditionEvaluator<T>
{
    bool Evaluate(T typedTarget);
}
