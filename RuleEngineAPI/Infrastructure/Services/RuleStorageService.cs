namespace RuleEngineAPI.Services;
public class RuleStorageService(ILogger<RuleStorageService> logger) : IRuleStorageService
{
    private readonly ILogger<RuleStorageService> _logger = logger;

    // Implement storage logic here
    public void StoreRule(string ruleContent, string jsonSchema)
    {
        _logger.LogInformation("Rule storage service: {ruleContent}", ruleContent);
        // Store the rule and schema
    }
}