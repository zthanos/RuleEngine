using RuleEngineTester.RuleEngine.Evaluators;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace RuleEngineTester.RuleEngine
{
    public class LsRule<T> : IRule
    {
        public IList<Func<T, bool>> ConditionFunctions { get; internal set; }
        public IList<Action> Actions { get; internal set; }

        public void AddCondition(Condition condition) => ConditionFunctions.Add(LsRule<T>.CreateConditionFunc(condition));
        public void AddConditions(IList<Condition> conditions)
        {
            foreach (var condition in conditions)
            {
                AddCondition(condition);
            }
        }

        public void AddAction(Action action) => Actions.Add(action);
        public void AddActions(IList<Action> actions)
        {
            foreach (var action in actions)
            {
                AddAction(action);
            }
        }

        public LsRule()
        {
            ConditionFunctions = new ObservableCollection<Func<T, bool>>();
            Actions = new ObservableCollection<Action>();
        }

        private static Func<T, bool> CreateConditionFunc(Condition condition)
        {
            var parameter = Expression.Parameter(typeof(T));
            var evaluatorFactory = new ConditionEvaluatorFactory<T>();
            var conditionEvaluator = evaluatorFactory.CreateConditionEvaluator(condition);

   
                return conditionEvaluator.BuildExpression(parameter).Compile();
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
            foreach (var action in Actions)
            {
                LsRule<T>.SetPropertyValue(typedTarget, action.PropertyName, action.ValidValue!);
            }
        }

        private void HandleFailedConditions(object obj, List<string> failedConditions)
        {
            // Handle what to do with the failed conditions (e.g., log, store, etc.)
            // You can customize this based on your needs
            // Example: Log the failed conditions for the given rule and customer
            Console.WriteLine($"Failed conditions for rule: {this}, customer: {obj}");
            foreach (var failedCondition in failedConditions)
            {
                Console.WriteLine($"  - {failedCondition}");
            }
        }

        public void ApplyRules(object target)
        {
            if (target is T typedTarget)
            {
                List<string> failedConditions = [];

                foreach (var condition in ConditionFunctions)
                {
                    if (!condition(typedTarget)) // Cast to T before applying the condition
                    {
                        failedConditions.Add(GetConditionDescription(condition));
                    }
                }

                if (failedConditions.Count == 0)
                {
                    ExecuteActions(typedTarget);
                }
                else
                {
                    // Handle failed conditions (e.g., log, store, etc.)
                    HandleFailedConditions(typedTarget, failedConditions);
                }
            }
            else
            {
                // Handle the case where the target is not of the expected type
                Console.WriteLine("Target is not of the expected type");
            }
        }

        private static string GetConditionDescription(Func<T, bool> condition)
        {
            // Provide a more human-readable description of the condition for logging or reporting
            // Example: "Age > 18"

            if (condition.Target != null)
            {
                var type = condition.Target.GetType();
                var methodName = condition.Method.Name;

                // Extracting parameter information
                var parameters = condition.Method.GetParameters();
                var parameterDescriptions = parameters.Select(p => $"{p.ParameterType} {p.Name}");

                return $"{type.Name}.{methodName}({string.Join(", ", parameterDescriptions)})";
            }

            // Fallback to the default ToString() if condition.Target is null
            return condition.Method.ToString()!;
        }
    }
}
