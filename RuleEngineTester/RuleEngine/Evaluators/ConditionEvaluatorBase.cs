namespace RuleEngineTester.RuleEngine.Evaluators;

public abstract class ConditionEvaluatorBase<T> : IConditionEvaluator<T>
{
    protected object GetPropertyValue(T target, string propertyName)
    {
        var propertyInfo = target.GetType().GetProperty(propertyName);
        return propertyInfo?.GetValue(target);
    }

    public abstract bool Evaluate(T typedTarget);
}
