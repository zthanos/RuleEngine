namespace RuleEngineTester.RuleEngine.Evaluators;

public class NotEmptyCondition<T> : ConditionEvaluatorBase<T>
{
    private readonly string propertyName;

    public NotEmptyCondition(string propertyName)
    {
        this.propertyName = propertyName;
    }

    public override bool Evaluate(T typedTarget)
    {
        return !string.IsNullOrEmpty((string)GetPropertyValue(typedTarget, propertyName)!);
    }
}
