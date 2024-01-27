using LsRuleEngine.Interfaces;
using Newtonsoft.Json.Schema;

namespace RuleEngineAPI.Application.Interfaces;
public interface IRuleManagerService
{
    IEnumerable<IRule> ParseRules(string ruleContent, JSchema schema);
    // Additional methods for adding and executing rules can be defined here
}
