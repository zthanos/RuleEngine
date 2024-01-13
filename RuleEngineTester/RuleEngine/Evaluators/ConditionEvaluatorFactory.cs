using System.Linq.Expressions;

namespace RuleEngineTester.RuleEngine.Evaluators
{
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
            // Build expressions for sub-conditions and combine them based on the logical operator
            Expression combinedExpression = null;

            foreach (var condition in subConditions)
            {
                var conditionEvaluator = new ConditionEvaluatorFactory<T>().CreateConditionEvaluator(condition);
                var conditionExpression = conditionEvaluator.BuildExpression(parameter);

                if (combinedExpression == null)
                {
                    // First condition
                    combinedExpression = conditionExpression.Body;
                }
                else
                {
                    // Combine with AND logic for now; you might extend it to handle OR
                    combinedExpression = Expression.AndAlso(combinedExpression, conditionExpression.Body);
                }
            }

            if (combinedExpression == null)
            {
                // No sub-conditions
                return null;
            }

            return Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
        }
    }

}
