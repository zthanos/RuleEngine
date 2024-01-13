namespace RuleEngineTester.RuleEngine.Evaluators;

public class ConditionEvaluatorFactory<T>
{
    public IConditionEvaluator<T> CreateConditionEvaluator(Condition condition)
    {
        return condition.ConditionType switch
        {
            ConditionType.Null => new NullCondition<T>(condition.Name),
            ConditionType.NotNull => new NotNullCondition<T>(condition.Name),
            ConditionType.NotEmpty => new NotEmptyCondition<T>(condition.Name),
            ConditionType.GreaterThan => new GreaterThanCondition<T>(condition.Name, condition.Value!),
            ConditionType.GreaterThanOrEquals => new GreaterThanOrEqualsCondition<T>(condition.Name, condition.Value!),
            ConditionType.LessThan => new LessThanCondition<T>(condition.Name, condition.Value!),
            ConditionType.LessThanOrEquals => new LessThanOrEqualsCondition<T>(condition.Name, condition.Value!),
            ConditionType.Equals => new EqualsCondition<T>(condition.Name, condition.Value!),
            ConditionType.NotEquals => new NotEqualsCondition<T>(condition.Name, condition.Value!),
            ConditionType.Composite => new CompositeConditionEvaluator<T>(condition.SubConditions),
            // Add more cases for other condition types
            _ => throw new NotSupportedException($"Condition type '{condition.ConditionType}' is not supported."),
        };
    }
}
