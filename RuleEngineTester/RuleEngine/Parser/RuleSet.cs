namespace RuleEngineTester.RuleEngine.Parser;

public class RuleSet
{
    public RuleSet()
    {
        Rules = [];
    }
    public List<JsonRule> Rules { get; set; }
}