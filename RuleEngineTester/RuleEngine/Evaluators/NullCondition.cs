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
}
