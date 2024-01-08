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
}
