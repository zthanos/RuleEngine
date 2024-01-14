namespace RuleEngineTester.RuleEngine.Parser.Common.Types;

public class RuleSet
{
    public RuleSet()
    {
        Rules = [];
    }
    public List<JsonRule> Rules { get; set; }
}