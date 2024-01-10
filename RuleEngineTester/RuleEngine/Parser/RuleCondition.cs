namespace RuleEngineTester.RuleEngine.Parser;

public class RuleCondition
{
    public string Property { get; set; }
    public string Type { get; set; }
    public string Operator { get; set; }
    public object? Value { get; set; }
}
