using RuleEngineTester.RuleEngine.Actions;
using RuleEngineTester.RuleEngine.Conditions;

namespace RuleEngineTester.RuleEngine;

public class JsonRule
{
    public string? Name { get; set; }
    public string? AppliesTo { get; set; }
    public int Order { get; set; }
    public List<Condition>? RuleConditions { get; set; }

    public List<RuleAction>? Actions { get; set; }
}
