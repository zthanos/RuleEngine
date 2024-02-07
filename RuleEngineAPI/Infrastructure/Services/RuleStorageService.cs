using Microsoft.Azure.Cosmos;
using RuleEngineAPI.Domain.Aggregates;
using RuleEngineAPI.Infrastructure.Interfaces;

namespace RuleEngineAPI.Services;
public class RuleStorageService(CosmosClient cosmosClient, ILogger<RuleStorageService> logger) : IRuleStorageService
{
    private readonly ILogger<RuleStorageService> _logger = logger;
    private readonly CosmosClient _cosmosClient = cosmosClient;
    private Database? _database;
    private Container? _container;

    public async Task InitializeCosmosClientAsync()
    {

        try
        {
            _database = await _cosmosClient.CreateDatabaseIfNotExistsAsync("lsruleengine");
            _container = await _database.CreateContainerIfNotExistsAsync("rules", "/id");

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing Cosmos DB");
            throw; // Or handle this error appropriately
        }
    }

    // Implement storage logic here
    public async Task<bool> StoreRule(int version, string typeToApplyRule, string ruleContent, string jsonSchema, IEnumerable<AvailableRule> availableRules)
    {
        _logger.LogInformation("Storing rule: {ruleContent}", ruleContent);

        // Use typeToApplyRule as the partition key value
        try
        {
            //var a = await _container.UpsertItemAsync(item, new PartitionKey(item.TypeToApplyRule));
            var testItem = new { id = $"{typeToApplyRule}", typeToApplyRule, ruleContent, jsonSchema, version, availableRules };
            if (_container is not null)
            {
                await _container.UpsertItemAsync(testItem, new PartitionKey(testItem.id));
            }
            return true;
        }
        catch (CosmosException ex)
        {
            _logger.LogError(
                "Cosmos DB Error: {message}. StatusCode: {status}. SubStatusCode: {substatus}. ActivityId: {ActivityId}",
                ex.Message,
                ex.StatusCode,
                ex.SubStatusCode,
                ex.ActivityId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Unable to store rule {ex}", ex.StackTrace);
            return false;
        }

    }

    // Add a method to retrieve rules by typeToApplyRule
    public async Task<IEnumerable<RuleItem>> GetRulesByTypeAsync(string id)
    {
        var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id")
           .WithParameter("@id", id);

        var iterator = _container?.GetItemQueryIterator<RuleItem>(query, requestOptions: new QueryRequestOptions
        {
            PartitionKey = new PartitionKey(id)
        });

        var results = new List<RuleItem>();
        if (iterator is null) return [];
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }

    public async Task<IEnumerable<RuleItem>> GetAllRulesAsync()
    {
        var query = new QueryDefinition("SELECT * FROM c");

        // Assuming _container is correctly instantiated and represents your Cosmos DB container
        var iterator = _container.GetItemQueryIterator<RuleItem>(query);

        var results = new List<RuleItem>();
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response.Resource); // Use .Resource to access the actual items
        }

        return results;
    }

}
public class RuleItem(string typeToApplyRule, string ruleContent, string jsonSchema, int version = 1)
{
    public string Id { get; set; } = $"{typeToApplyRule}-{version}";
    public int Version { get; set; } = version;
    public string TypeToApplyRule { get; set; } = typeToApplyRule;
    public string RuleContent { get; set; } = ruleContent;
    public string JsonSchema { get; set; } = jsonSchema;
    public IEnumerable<AvailableRule> AvailableRules { get; set; }
}

