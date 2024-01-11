using System;
using System.Linq.Expressions;

namespace RuleEngineTester.RuleEngine.Evaluators
{
    public class NotEmptyCondition<T> : ConditionEvaluatorBase<T>
    {
        private readonly string propertyName;

        public NotEmptyCondition(string propertyName)
        {
            this.propertyName = propertyName;
        }

        public override bool Evaluate(T typedTarget)
        {
            var value = GetPropertyValue(typedTarget, propertyName);
            return value is string stringValue && !string.IsNullOrEmpty(stringValue);
        }

        public override Expression<Func<T, bool>> BuildExpression(ParameterExpression parameter)
        {
            // Build and return the expression for not empty condition
            return (Expression<Func<T, bool>>)Expression.Lambda<Func<T, bool>>(
                Expression.NotEqual(
                    Expression.Property(parameter, propertyName),
                    Expression.Constant(string.Empty)
                ),
                parameter
            );
        }
    }
}
