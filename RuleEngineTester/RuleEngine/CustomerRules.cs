//using System.Text;

//namespace RuleEngineTester.RuleEngine;

//public class CustomerRules<TCustomer> : IRulesProcessor<TCustomer> where TCustomer : class
//{
//    private readonly List<IRule<TCustomer>> _rules;

//    public CustomerRules(List<IRule<TCustomer>> rules)
//    {
//        _rules = rules ?? throw new ArgumentNullException(nameof(rules));
//    }

//    public CustomerRules()
//    {
//        _rules = new();
//    }


//    public void ApplyRules(TCustomer customer)
//    {
//        foreach (var rule in _rules)
//        {
//            List<string> failedConditions = new List<string>();

//            foreach (var condition in rule.Conditions)
//            {
//                if (!condition(customer))
//                {
//                    failedConditions.Add(GetConditionDescription(condition));
//                }
//            }

//            if (failedConditions.Count == 0)
//            {
//                foreach (var action in rule.Actions)
//                {
//                    action(customer);
//                }
//            }
//            else
//            {
//                // Handle failed conditions (e.g., log, store, etc.)
//                HandleFailedConditions(rule, customer, failedConditions);
//            }
//        }
//    }

//    public void AddRule(List<Condition> conditions)
//    {
//        var ruleConditions = ConvertConditionsToFunc(conditions);
//        var rule = new LsRule<TCustomer>(ruleConditions, new List<Action<TCustomer>>());
//        _rules.Add(rule);
//    }

//    private List<Func<TCustomer, bool>> ConvertConditionsToFunc(List<Condition> conditions)
//    {
//        return conditions.Select(condition => CreateConditionFunc(condition)).ToList();
//    }

//    private Func<TCustomer, bool> CreateConditionFunc(Condition condition)
//    {
//        switch (condition.ConditionType)
//        {
//            case ConditionType.Null:
//                return customer => GetPropertyValue(customer, condition.Name) == null;
//            case ConditionType.NotNull:
//                return customer => GetPropertyValue(customer, condition.Name) != null;
//            case ConditionType.NotEmpty:
//                return customer => (string)GetPropertyValue(customer, condition.Name)! != string.Empty;
//            case ConditionType.None:
//                return customer => true;
//            // Add more cases for other condition types
//            default:
//                throw new NotSupportedException($"Condition type '{condition.ConditionType}' is not supported.");
//        }
//    }

//    private object? GetPropertyValue(TCustomer customer, string propertyName)
//    {
//        var property = typeof(TCustomer).GetProperty(propertyName);
//        return property?.GetValue(customer);
//    }


//    private string GetConditionDescription(Func<TCustomer, bool> condition)
//    {
//        // Provide a more human-readable description of the condition for logging or reporting
//        // Example: "Age > 18"

//        if (condition.Target != null)
//        {
//            var type = condition.Target.GetType();
//            var methodName = condition.Method.Name;

//            // Extracting parameter information
//            var parameters = condition.Method.GetParameters();
//            var parameterDescriptions = parameters.Select(p => $"{p.ParameterType} {p.Name}");

//            return $"{type.Name}.{methodName}({string.Join(", ", parameterDescriptions)})";
//        }

//        // Fallback to the default ToString() if condition.Target is null
//        return condition.Method.ToString();
//    }


//    private void HandleFailedConditions(IRule<TCustomer> rule, TCustomer customer, List<string> failedConditions)
//    {
//        // Handle what to do with the failed conditions (e.g., log, store, etc.)
//        // You can customize this based on your needs
//        // Example: Log the failed conditions for the given rule and customer
//        Console.WriteLine($"Failed conditions for rule: {rule}, customer: {customer}");
//        foreach (var failedCondition in failedConditions)
//        {
//            Console.WriteLine($"  - {failedCondition}");
//        }
//    }
//}

////public void AddRule(List<Condition> conditions, List<Action<TCustomer>> actions)
////{
////    var rule = new LsRule<TCustomer>(conditions.AsEnumerable(), actions);
////    _rules.Add(rule);
////}
////public void AddRule(List<Condition> conditions) {
////    var rule = new LsRule<TCustomer>(conditions.AsEnumerable(), new List<Action<TCustomer>>());

////}
////    public void ApplyRules(TCustomer customer)
////    {
////        foreach (var rule in _rules)
////        {
////            if (rule.Conditions(customer))
////            {
////                rule.Actions(customer);
////            }
////        }
////    }

////    private void SetPropertyValue(TCustomer customer, string propertyName, object value)
////    {
////        var property = customer.GetType().GetProperty(propertyName);
////        if (property != null && property.PropertyType == typeof(bool))
////        {
////            property.SetValue(customer, value);
////        }
////        // Optionally, you might want to handle other property types or log a warning for unexpected types.
////    }

////    private LsRule<TCustomer> GenerateRuleFromCondition(Condition condition)
////    {

////        Func<TCustomer, bool> conditionFunc = customer =>
////        {
////            var value = customer?.GetType().GetProperty(condition.Name)?.GetValue(customer);
////            // Call ExecuteCondition method to evaluate the condition
////            return condition.ExecuteCondition(condition.Name, value, value);
////        };

////        Action<TCustomer> action = customer =>
////        {
////            // Implement the action based on the condition object
////            // For simplicity, this example sets the IsKYCValid property to true
////            SetPropertyValue(customer, "IsKYCValid", true);
////        };

////        return new LsRule<TCustomer>(conditionFunc, action);

////    }
////}


