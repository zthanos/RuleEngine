using LsRuleEngine;
using LsRuleEngine.Interfaces;
using Newtonsoft.Json.Schema;
using RuleEngineAPI.Domain.Aggregates;
using RuleEngineAPI.Services;

namespace RuleEngineAPI.Domain.Interfaces;
public interface IRuleManagerService
{
    IEnumerable<IRule> ParseRules(string ruleContent, JSchema schema);
    RuleExecutionResults ExecuteRules(RuleSet ruleset, string jsonData);
}
