using RuleEngineTester.RuleEngine.Parser;

namespace RuleEngineTester.RuleEngine;

public class JsonRule
{
    public string? Name { get; set; }
    public string? AppliesTo { get; set; }
    public int Order { get; set; }
    public List<RuleCondition>? Conditions { get; set; }
    public List<RuleAction>? Actions { get; set; }
}
