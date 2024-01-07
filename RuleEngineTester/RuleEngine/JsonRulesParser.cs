using Newtonsoft.Json;
using System.Reflection;

namespace RuleEngineTester.RuleEngine;

public class JsonRuleParser
{
    private const string InvokeAddConditions = "AddConditions";
    private const string InvokeAddActions = "AddActions";

    public static List<IRule> Parse(string fn)
    {
        var rules = new List<IRule>();
        var data = File.ReadAllText(fn);
        var ruleSet = JsonConvert.DeserializeObject<RuleSet>(data);
        foreach (var rule in ruleSet.Rules)
        {
            if (string.IsNullOrWhiteSpace(rule.AppliesTo))
            {
                Console.WriteLine("Tatget is not defined");
                continue;
            }

            Type type = Type.GetType(rule.AppliesTo);
            if (typeof(IRuleApplicable).IsAssignableFrom(type))
            {
                Type lsRuleType = typeof(LsRule<>).MakeGenericType(type);
                var lsRuleInstance = Activator.CreateInstance(lsRuleType);
                var conditions = rule.Conditions.Select(condition => new Condition(
                    1,
                    condition.Property,
                    condition.Value,
                    condition.Type,
                    condition.Operator));
                var actions = rule.Actions.Select(action => new Action(action.Property, true, false));
                MethodInfo? addConditionMethod = lsRuleType.GetMethod(InvokeAddConditions);
                MethodInfo? addActionsMethod = lsRuleType.GetMethod(InvokeAddActions);
                if (addConditionMethod != null && addActionsMethod != null)
                {
                    addConditionMethod?.Invoke(lsRuleInstance, new object[] { conditions.ToList() });
                    addActionsMethod?.Invoke(lsRuleInstance, new object[] { actions.ToList() });
                    rules.Add((IRule)lsRuleInstance!);
                }
                else
                {
                    Console.WriteLine($"Cannot invoke {InvokeAddConditions}");
                }
            }
            else
            {
                // Handle cases where the type doesn't implement IRuleApplicable
            }
        }

        return rules;
    }

}


public class RuleCondition
{
    public string Property { get; set; }
    public string Type { get; set; }
    public string Operator { get; set; }
    public Object? Value { get; set; }
}

public class RuleAction
{
    public string Type { get; set; }
    public string Property { get; set; }
    public object Value { get; set; }
}

public class JsonRule
{
    public string Name { get; set; }
    public string AppliesTo { get; set; }
    public int Order { get; set; }
    public List<RuleCondition> Conditions { get; set; }
    public List<RuleAction> Actions { get; set; }
}

public class RuleSet
{
    public List<JsonRule> Rules { get; set; }
}