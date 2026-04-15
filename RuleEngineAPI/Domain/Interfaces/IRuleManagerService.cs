using LsRuleEngine.Interfaces;
using Newtonsoft.Json.Schema;
using RuleEngineAPI.Domain.Aggregates;

namespace RuleEngineAPI.Domain.Interfaces;
public interface IRuleManagerService
{
    IEnumerable<IRule> ParseRules(string ruleContent, JSchema schema);
    RuleExecutionResults ExecuteRules(Guid id, RuleSet ruleset, string jsonData);
}
