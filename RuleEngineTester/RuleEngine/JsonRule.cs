using RuleEngineTester.RuleEngine.Parser;

namespace RuleEngineTester.RuleEngine;

public class JsonRule
{
    public string? Name { get; set; }
    public string? AppliesTo { get; set; }
    public int Order { get; set; }
    public List<RuleCondition>? Conditions { get; set; }
    public List<Condition>? RuleConditions { get; set; }
    public List<RuleAction>? RuleActions { get; set; }
    public List<Action>? Actions { get; set; }
}
