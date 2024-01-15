using RuleEngineTester.RuleEngine.Actions;
using RuleEngineTester.RuleEngine.ErrorHandling;
using RuleEngineTester.RuleEngine.Parser.Common.Types;
using RuleEngineTester.RuleEngine.Rule;
using RuleEngineTester.RuleEngine.Rule.Interfaces;
using System.Data;

namespace RuleEngineTester.RuleEngine.Parser.Common;

public class RuleParserBase<T>
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
                throw new RuleEngineException("Target is not defined:");
            }

            Type type = Type.GetType(rule.AppliesTo)!;
            if (type is null) {
                throw new RuleEngineException($"The type '{rule.AppliesTo}' cannot be resolved!");
            }

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
                throw new RuleEngineException($"The type '{type}' doesn't implement IRuleApplicable");
            }
        }
        return parsedRules;
    }
}
