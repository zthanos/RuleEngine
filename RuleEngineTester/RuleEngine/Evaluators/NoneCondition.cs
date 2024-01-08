namespace RuleEngineTester.RuleEngine.Evaluators;

// Add more condition classes as needed...

public class NoneCondition<T> : ConditionEvaluatorBase<T>
{
    public override bool Evaluate(T typedTarget)
    {
        return true;
    }
}
