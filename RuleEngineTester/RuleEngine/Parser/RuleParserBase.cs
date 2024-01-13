using System.Reflection;

namespace RuleEngineTester.RuleEngine.Parser;

public class RuleParserBase
{
    protected const string InvokeAddConditions = "AddConditions";
    protected const string InvokeAddActions = "AddActions";

    public static IList<IRule> ProcessRuleSet(RuleSet ruleSet)
    {
        var parsedRules = new List<IRule>();
        foreach (var rule in ruleSet.Rules)
        {
            if (string.IsNullOrWhiteSpace(rule.AppliesTo))
            {
                Console.WriteLine("Tatget is not defined");
                continue;
            }

            Type type = Type.GetType(rule.AppliesTo)!;
            if (typeof(IRuleApplicable).IsAssignableFrom(type))
            {
                Type lsRuleType = typeof(LsRule<>).MakeGenericType(type);
                var lsRuleInstance = Activator.CreateInstance(lsRuleType);
                var conditions = rule.RuleConditions;

                var actions = rule.Actions!.Select(action => new Action(action.PropertyName!, true, false));
                MethodInfo? addConditionMethod = lsRuleType.GetMethod(InvokeAddConditions, BindingFlags.Instance | BindingFlags.Public);
                MethodInfo? addActionsMethod = lsRuleType.GetMethod(InvokeAddActions, BindingFlags.Instance | BindingFlags.Public);

                if (addConditionMethod != null && addActionsMethod != null)
                {
                    addConditionMethod?.Invoke(lsRuleInstance, new object[] { conditions.ToList() });
                    addActionsMethod?.Invoke(lsRuleInstance, new object[] { actions.ToList() });
                    parsedRules.Add((IRule)lsRuleInstance!);
                }
                else
                {
                    Console.WriteLine($"Cannot invoke {InvokeAddConditions} or {InvokeAddActions}");
                }
            }
            else
            {
                // Handle cases where the type doesn't implement IRuleApplicable
                Console.WriteLine("The type doesn't implement IRuleApplicable");
            }
        }
        return parsedRules;
    }
}
