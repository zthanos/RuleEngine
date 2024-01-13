using System;
using System.Linq.Expressions;

namespace RuleEngineTester.RuleEngine.Evaluators
{
    public class GreaterThanCondition<T> : ConditionEvaluatorBase<T>
    {
        private readonly string propertyName;
        private readonly object expectedValue;

        public GreaterThanCondition(string propertyName, object expectedValue)
        {
            this.propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            this.expectedValue = expectedValue;
        }

        public override bool Evaluate(T typedTarget)
        {
            var propertyValue = GetPropertyValue(typedTarget, propertyName);

            // Assuming propertyValue and expectedValue are of numeric types
            if (propertyValue is IComparable comparablePropertyValue && expectedValue is IComparable comparableExpectedValue)
            {
                return comparablePropertyValue.CompareTo(comparableExpectedValue) > 0;
            }

            // Default to false if types are not suitable for comparison
            return false;
        }



        public override Expression<Func<T, bool>> BuildExpression(ParameterExpression parameter)
        {
            //// Build expression for the GreaterThanOrEquals condition
            //Expression propertyExpression = Expression.Property(parameter, propertyName);
            //Expression expectedValueExpression = Expression.Constant(expectedValue);

            //// Convert expectedValue to the type of the property if they are not already the same type
            //if (propertyExpression.Type != expectedValueExpression.Type)
            //{
            //    expectedValueExpression = Expression.Convert(expectedValueExpression, propertyExpression.Type);
            //}

            //var greaterThanOrEqualsExpression = Expression.GreaterThanOrEqual(propertyExpression, expectedValueExpression);

            //return Expression.Lambda<Func<T, bool>>(greaterThanOrEqualsExpression, parameter);
            var propertyExpression = Expression.Property(parameter, propertyName);
            var expectedValueExpression = Expression.Constant(expectedValue);
            var targetType = propertyExpression.Type; // The target type is the type of the property

            // Use TypeConverter to convert the string to the target type
            var converter = System.ComponentModel.TypeDescriptor.GetConverter(targetType);
            object parsedValue = null;

            if (converter.IsValid(expectedValue))
            {
                parsedValue = converter.ConvertFrom(expectedValue);
            }
            else
            {
                // Handle conversion failure
            }

            var parsedValueExpression = Expression.Constant(parsedValue, targetType);
            var greaterThanOrEqualsExpression = Expression.GreaterThanOrEqual(propertyExpression, parsedValueExpression);
            return Expression.Lambda<Func<T, bool>>(greaterThanOrEqualsExpression, parameter);

        }
    }
}

