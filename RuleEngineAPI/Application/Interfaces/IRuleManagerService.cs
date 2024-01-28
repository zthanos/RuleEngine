using LsRuleEngine.Interfaces;
using Newtonsoft.Json.Schema;
using RuleEngineAPI.Services;

namespace RuleEngineAPI.Application.Interfaces;
public interface IRuleManagerService
{
    IEnumerable<IRule> ParseRules(string ruleContent, JSchema schema);
    string ExecuteRules(string jsonData, IEnumerable<RuleItem> rules);
}
