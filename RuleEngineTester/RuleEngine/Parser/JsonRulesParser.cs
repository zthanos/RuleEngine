using Newtonsoft.Json;

namespace RuleEngineTester.RuleEngine.Parser;
public class JsonRuleParser : RuleParserBase
{
    public List<IRule> Parse(string fn)
    {
        var data = File.ReadAllText(fn);
        var ruleSet = JsonConvert.DeserializeObject<RuleSet>(data);
        if (ruleSet != null)
        {

            return (List<IRule>)ProcessRuleSet(ruleSet);
        }
        else
            return [];
    }

}
