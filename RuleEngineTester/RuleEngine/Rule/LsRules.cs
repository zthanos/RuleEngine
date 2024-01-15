using RuleEngineTester.RuleEngine.Actions;
using RuleEngineTester.RuleEngine.Conditions;
using RuleEngineTester.RuleEngine.ErrorHandling;
using RuleEngineTester.RuleEngine.Evaluators;
using RuleEngineTester.RuleEngine.Rule.Interfaces;
using System.ComponentModel;
using System.Linq.Expressions;

namespace RuleEngineTester.RuleEngine.Rule
{
    /// <summary>
    /// Represents a rule engine implementation for a specific type <typeparamref name="T"/>.
    /// The rule engine allows defining conditions and actions, which are then applied to objects of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to which the rules are applied.</typeparam>
    public class LsRule<T> : IRule<T>, IRule
    {
        private Func<T, bool>? combinedCondition;

        private readonly IList<RuleAction> _Actions = [];
        private string appliesTo = "";
        private string ruleName = "";


        /// <summary>
        /// Adds a condition to the rule.
        /// </summary>
        /// <param name="condition">The condition to be added.</param>
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

        /// <summary>
        /// Adds a list of conditions to the rule.
        /// </summary>
        /// <param name="conditions">The list of conditions to be added.</param>
        public void AddConditions(IList<Condition> conditions) => conditions.ToList().ForEach(f => AddCondition(f));

        /// <summary>
        /// Adds an action to the rule.
        /// </summary>
        /// <param name="action">The action to be added.</param>
        public void AddAction(RuleAction action) => _Actions.Add(action);

        /// <summary>
        /// Adds a list of actions to the rule.
        /// </summary>
        /// <param name="actions">The list of actions to be added.</param>
        public void AddActions(IList<RuleAction> actions)
        {
            foreach (var action in actions)
            {
                AddAction(action);
            }
        }

        /// <summary>
        /// Executes the defined actions on the specified object if the conditions are met.
        /// </summary>
        /// <param name="typedTarget">The object to which the rule is applied.</param>
        private void ExecuteActions(T typedTarget)
        {
            foreach (var action in _Actions)
            {
                LsRule<T>.SetPropertyValue(typedTarget, action.PropertyName, action.ValidValue!);
            }
        }

        /// <summary>
        /// Sets the value of a property on the target object.
        /// </summary>
        /// <param name="typedTarget">The target object.</param>
        /// <param name="propertyName">The name of the property to set.</param>
        /// <param name="value">The value to set.</param>
        private static void SetPropertyValue(T typedTarget, string propertyName, object value)
        {
            var property = typedTarget!.GetType().GetProperty(propertyName);

            if (value is string stringValue)
            {
                // Convert the string to the type of the property using TypeConverter
                var converter = TypeDescriptor.GetConverter(property!.PropertyType);
                try
                {
                    var convertedValue = converter.ConvertFromString(stringValue);
                    property.SetValue(typedTarget, convertedValue);
                }
                catch (Exception ex)
                {
                    // Handle the case where conversion fails
                    throw new RuleEngineException($"Error: Unable to convert '{stringValue}' to {property.PropertyType.Name} for property '{propertyName}'.\n{ex.Message}");
                }
            }
            else
            {
                // Handle other cases or log a warning
                throw new RuleEngineException($"Error: Unsupported type '{value.GetType().FullName}' for property '{propertyName}'.");

            }
        }

        /// <summary>
        /// Applies the rules to the specified target object.
        /// </summary>
        /// <param name="target">The target object to which the rules are applied.</param>
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

        public LsRule() { }

        /// <summary>
        /// Creates a new instance of the <see cref="LsRule{T}"/> using the builder pattern.
        /// </summary>
        /// <returns>A new instance of the <see cref="LsRule{T}"/> builder.</returns>
        public static LsRuleBuilder CreateBuilder() => new LsRuleBuilder();

        /// <summary>
        /// Sets the name of the rule.
        /// </summary>
        /// <param name="name">The name of the rule.</param>
        public void SetRuleName(string name)
        {
            ruleName = name;
        }

        /// <summary>
        /// Sets the type to which the rule applies.
        /// </summary>
        /// <param name="appliesTo">The type to which the rule applies.</param>

        public void SetAppliesTo(string appliesTo)
        {
            this.appliesTo = appliesTo;
        }

        /// <summary>
        /// The builder class for constructing instances of <see cref="LsRule{T}"/>.
        /// </summary>
        public class LsRuleBuilder
        {
            private IRule? _rule;

            /// <summary>
            /// Adds a list of conditions to the rule being built.
            /// </summary>
            /// <param name="conditions">The list of conditions to be added.</param>
            public LsRuleBuilder AddConditions(IList<Condition> conditions)
            {
                foreach (var condition in conditions)
                {
                    _rule?.AddCondition(condition);
                }
                return this;
            }

            /// <summary>
            /// Adds a list of actions to the rule being built.
            /// </summary>
            /// <param name="actions">The list of actions to be added.</param>
            public LsRuleBuilder AddActions(IList<RuleAction> actions)
            {
                foreach (var action in actions)
                {
                    _rule?.AddAction(action);
                }
                return this;
            }

            /// <summary>
            /// Sets the name of the rule being built.
            /// </summary>
            /// <param name="ruleName">The name of the rule.</param>
            public LsRuleBuilder WithRuleName(string ruleName)
            {
                _rule?.SetRuleName(ruleName);
                return this;
            }

            /// <summary>
            /// Sets the type to which the rule applies.
            /// </summary>
            /// <param name="appliesTo">The type to which the rule applies.</param>
            public LsRuleBuilder AppliesTo(string appliesTo)
            {
                _rule?.SetAppliesTo(appliesTo);
                return this;
            }

            /// <summary>
            /// Specifies the type for which the rule is being built.
            /// </summary>
            /// <param name="typeName">The name of the type for which the rule is being built.</param>
            public LsRuleBuilder ForType(string typeName)
            {

                Type type = Type.GetType(typeName)!;
                if (type is null)
                {
                    throw new RuleEngineException($"The type '{typeName}' cannot be resolved!");
                }

                // Ensure the type implements IRuleApplicable at compile-time
                if (typeof(IRuleApplicable).IsAssignableFrom(type))
                {
                    var lsRuleType = typeof(LsRule<>).MakeGenericType(type);
                    _rule = (IRule)Activator.CreateInstance(lsRuleType)!;
                }

                return this;
            }

            public IRule Build() => _rule!;
        }
    }
}
