using Newtonsoft.Json;

namespace RuleEngineTester.RuleEngine.Parser;
public class JsonRuleParser<T> : RuleParserBase<T>
{
    public List<IRule<IRuleApplicable>> Parse(string fn)
    {
        var data = File.ReadAllText(fn);
        var ruleSet = JsonConvert.DeserializeObject<RuleSet>(data);
        if (ruleSet != null)
        {

            return (List<IRule<IRuleApplicable>>)ProcessRuleSet(ruleSet);
        }
        else
            return [];
    }

}
