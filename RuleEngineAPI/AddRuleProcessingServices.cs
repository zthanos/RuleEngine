using EventStore.Client;
using Microsoft.Azure.Cosmos;
using RuleEngineAPI.Application.Services;
using RuleEngineAPI.Domain.Interfaces;
using RuleEngineAPI.Infrastructure.Interfaces;
using RuleEngineAPI.Infrastructure.Services;
using RuleEngineAPI.Services;

namespace RuleEngineAPI;

public static class ServiceRegistrations
{
    public static void AddCommonServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
    }
    public static void AddRuleStorage(IServiceCollection services)
    {
        services.AddSingleton<IRuleStorageService, RuleStorageService>();
        services.AddTransient<IRuleSetRepository, RuleSetRepository>();
        services.AddSingleton<CosmosClient>(sp =>
        {
            CosmosClientOptions options = new()
            {
                HttpClientFactory = () => new HttpClient(new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                }),
                ConnectionMode = ConnectionMode.Gateway,
            };
            var client = new CosmosClient(
            accountEndpoint: Environment.GetEnvironmentVariable("COSMOS_ENDPOINT"),
            authKeyOrResourceToken: "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
            options);
            return client; ;
        });
    }
    public static void AddEventStore(IServiceCollection services, IConfiguration configuration) 
    {
        var clientSettings = EventStoreClientSettings.Create(Environment.GetEnvironmentVariable("ESDB"));
        var client = new EventStoreClient(clientSettings);
        services.AddSingleton(client);

        services.AddTransient<IEventStore, EventStoreService>();
        services.AddEventStoreClient(configuration
                                    .GetSection("EventStore")
                                    .Get<string>()!);
    }

    public static void AddInfrastructure(IServiceCollection services)
    {
        services.AddSingleton<IRuleManagerService, RuleManagerService>();
        services.AddTransient<IRuleProcessingService, RuleProcessingService>();
    }
    public static void AddCors(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                builder =>
                {
                    builder
                    .AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
        });
    }
}