using Newtonsoft.Json;
using RuleEngineTester.RuleEngine.Parser.Common;
using RuleEngineTester.RuleEngine.Parser.Common.Types;
using RuleEngineTester.RuleEngine.Rule.Interfaces;

namespace RuleEngineTester.RuleEngine.Parser.Json;
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
