using RuleEngineTester.RuleEngine.Actions;
using RuleEngineTester.RuleEngine.Parser.Common.Types;
using RuleEngineTester.RuleEngine.Rule;
using RuleEngineTester.RuleEngine.Rule.Interfaces;

namespace RuleEngineTester.RuleEngine.Parser.Common;

public class RuleParserBase<T>
{
    protected const string InvokeAddConditions = "AddConditions";
    protected const string InvokeAddActions = "AddActions";

    public static IList<IRule> ProcessRuleSet(RuleSet ruleSet)
    {
        var parsedRules = new List<IRule>();
        try
        {
            foreach (var rule in ruleSet.Rules)
            {
                if (string.IsNullOrWhiteSpace(rule.AppliesTo))
                {
                    Console.WriteLine("Target is not defined");
                    continue;
                }

                Type type = Type.GetType(rule.AppliesTo)!;

                // Ensure the type implements IRuleApplicable at compile-time
                if (typeof(IRuleApplicable).IsAssignableFrom(type))
                {
                    var lsRuleType = typeof(LsRule<>).MakeGenericType(type);
                    var lsRuleInstance = (IRule)Activator.CreateInstance(lsRuleType)!;

                    var conditions = rule.RuleConditions;
                    var actions = rule.Actions!.Select(action => new RuleAction(action.PropertyName!, true, false));

                    // Add conditions and actions to lsRuleInstance
                    lsRuleInstance.AddConditions(conditions!);
                    lsRuleInstance.AddActions(actions.ToList());

                    parsedRules.Add(lsRuleInstance);
                }
                else
                {
                    Console.WriteLine($"The type '{type}' doesn't implement IRuleApplicable");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        return parsedRules;
    }
}
