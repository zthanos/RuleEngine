using RuleEngineTester.RuleEngine.Actions;
using RuleEngineTester.RuleEngine.Conditions;
using RuleEngineTester.RuleEngine.Evaluators;
using RuleEngineTester.RuleEngine.Rule.Interfaces;
using System.Linq.Expressions;

namespace RuleEngineTester.RuleEngine.Rule
{
    public class LsRule<T> : IRule<T>, IRule
    {
        private Func<T, bool>? combinedCondition;

        private readonly IList<RuleAction> _Actions = [];

        public void AddCondition(Condition condition)
        {
            var parameter = Expression.Parameter(typeof(T));
            var evaluatorFactory = new ConditionEvaluatorFactory<T>();
            var conditionEvaluator = evaluatorFactory.CreateConditionEvaluator(condition);

            var conditionFunc = conditionEvaluator.BuildExpression(parameter).Compile();

            if (combinedCondition == null)
            {
                // If it's the first condition, set the combined condition
                combinedCondition = conditionFunc;
            }
            else
            {
                // Combine the existing condition with the new one using logical AND
                var combinedExpression = Expression.AndAlso(
                    Expression.Invoke(Expression.Constant(combinedCondition), parameter),
                    Expression.Invoke(Expression.Constant(conditionFunc), parameter)
                );

                // Compile the combined expression into a Func<T, bool>
                combinedCondition = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter).Compile();
            }
        }

        public void AddConditions(IList<Condition> conditions) => conditions.ToList().ForEach(f => AddCondition(f));

        public void AddAction(RuleAction action) => _Actions.Add(action);
        public void AddActions(IList<RuleAction> actions)
        {
            foreach (var action in actions)
            {
                AddAction(action);
            }
        }

        private static void SetPropertyValue(T typedTarget, string propertyName, object value)
        {
            var property = typedTarget!.GetType().GetProperty(propertyName);
            if (property != null && property.PropertyType == typeof(bool))
            {
                property.SetValue(typedTarget, value);
            }
            // Optionally, you might want to handle other property types or log a warning for unexpected types.
        }

        private void ExecuteActions(T typedTarget)
        {
            foreach (var action in _Actions)
            {
                LsRule<T>.SetPropertyValue(typedTarget, action.PropertyName, action.ValidValue!);
            }
        }

        public void ApplyRules(T target)
        {
            if (combinedCondition != null && combinedCondition(target))
            {
                ExecuteActions(target);
            }
            else
            {
                // Handle the case where the conditions are not met
                Console.WriteLine("Conditions not met");
            }
        }
    }
}
