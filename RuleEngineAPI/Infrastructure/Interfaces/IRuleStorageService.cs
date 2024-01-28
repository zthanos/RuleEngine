using RuleEngineAPI.Services;

namespace RuleEngineAPI.Infrastructure.Interfaces;

public interface IRuleStorageService
{
    Task InitializeCosmosClientAsync();
    Task<bool> StoreRule(int version, string typeToApplyRule, string ruleContent, string jsonSchema);
    Task<IEnumerable<RuleItem>> GetRulesByTypeAsync(string id);

}