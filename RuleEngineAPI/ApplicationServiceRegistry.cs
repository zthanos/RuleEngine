using EventStore.Client;
using Microsoft.Azure.Cosmos;
using RuleEngineAPI.Application.Services;
using RuleEngineAPI.Domain.Interfaces;
using RuleEngineAPI.Infrastructure.Interfaces;
using RuleEngineAPI.Infrastructure.Services;
using RuleEngineAPI.Services;

namespace RuleEngineAPI;
public static class ApplicationServiceRegistry
{
    public static void ConfigureCoreFrameworkServices(IServiceCollection services)
    {
        // Core services like API explorers, Swagger, MediatR...
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
    }

    public static void ConfigureRuleDomainServices(IServiceCollection services)
    {
        // Domain-specific services for rule storage and repositories...
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

    public static void ConfigureEventSourcingServices(IServiceCollection services, IConfiguration configuration)
    {
        // EventStoreDB client and related services...
        var clientSettings = EventStoreClientSettings.Create(Environment.GetEnvironmentVariable("ESDB"));
        var client = new EventStoreClient(clientSettings);
        services.AddSingleton(client);

        services.AddTransient<IEventStore, EventStoreService>();
        services.AddEventStoreClient(configuration
                                    .GetSection("EventStore")
                                    .Get<string>()!);
    }

    public static void AddRuleManagementInfrastructure(IServiceCollection services)
    {
        // Infrastructure services for rule management and processing...
        services.AddSingleton<IRuleManagerService, RuleManagerService>();
        services.AddTransient<IRuleProcessingService, RuleProcessingService>();
    }

    public static void ConfigureCorsPolicy(IServiceCollection services)
    {
        // CORS policy setup...
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
