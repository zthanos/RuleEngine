using System.Linq.Expressions;

namespace RuleEngineTester.RuleEngine.Evaluators
{
    public class ConditionEvaluatorFactory<T>
    {
        public IConditionEvaluator<T> CreateConditionEvaluator(Condition condition)
        {
            switch (condition.ConditionType)
            {
                case ConditionType.Null:
                    return new NullCondition<T>(condition.Name);
                case ConditionType.NotNull:
                    return new NotNullCondition<T>(condition.Name);
                case ConditionType.NotEmpty:
                    return new NotEmptyCondition<T>(condition.Name);
                //case ConditionType.None:
                //    return new NoneCondition<T>();
                case ConditionType.GreaterThan:
                    return new GreaterThanCondition<T>(condition.Name, condition.Value!);
                case ConditionType.GreaterThanOrEquals:
                    return new GreaterThanOrEqualsCondition<T>(condition.Name, condition.Value!);
                case ConditionType.LessThan:
                    return new LessThanCondition<T>(condition.Name, condition.Value!);
                case ConditionType.LessThanOrEquals:
                    return new LessThanOrEqualsCondition<T>(condition.Name, condition.Value!);
                case ConditionType.Equals:
                    return new EqualsCondition<T>(condition.Name, condition.Value!);
                case ConditionType.Composite:
                    return new CompositeConditionEvaluator<T>(condition.SubConditions);
                // Add more cases for other condition types
                default:
                    throw new NotSupportedException($"Condition type '{condition.ConditionType}' is not supported.");
            }
        }
    }

    public class CompositeConditionEvaluator<T> : IConditionEvaluator<T>
    {
        private readonly List<Condition> subConditions;

        public CompositeConditionEvaluator(List<Condition> subConditions)
        {
            this.subConditions = subConditions;
        }

        public bool Evaluate(T target)
        {
            // Default to true for an empty list of conditions
            if (subConditions == null || subConditions.Count == 0)
            {
                return true;
            }

            // Use AND logic for now; you might extend it to handle OR and other logical operators
            foreach (var condition in subConditions)
            {
                var conditionEvaluator = new ConditionEvaluatorFactory<T>().CreateConditionEvaluator(condition);
                if (!conditionEvaluator.Evaluate(target))
                {
                    return false; // Short-circuit on the first false condition
                }
            }

            return true;
        }

        public Expression<Func<T, bool>> BuildExpression(ParameterExpression parameter)
        {
            // You need to implement the logic to build the composite expression here
            throw new NotImplementedException("BuildExpression not implemented in CompositeConditionEvaluator.");
        }
    }

}
