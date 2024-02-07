using EventStore.Client;
using MediatR;
using Microsoft.Azure.Cosmos;
using RuleEngineAPI;
using RuleEngineAPI.Application.Commands;
using RuleEngineAPI.Application.Queries;
using RuleEngineAPI.Application.Services;
using RuleEngineAPI.Commands;
using RuleEngineAPI.Domain.Interfaces;
using RuleEngineAPI.Infrastructure.Interfaces;
using RuleEngineAPI.Infrastructure.Services;
using RuleEngineAPI.Services;
using RuleEngineAPI.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddSingleton<IRuleManagerService, RuleManagerService>();

var clientSettings = EventStoreClientSettings.Create(Environment.GetEnvironmentVariable("ESDB"));
//var clientSettings = EventStoreClientSettings.Create("esdb://192.168.2.5:2113?tls=false");
//var esdb = Environment.GetEnvironmentVariable("ESDB");
//var clientSettings = EventStoreClientSettings.Create("esdb://localhost?tls=false");
var client = new EventStoreClient(clientSettings);
builder.Services.AddSingleton(client);
//builder.Services.AddSingleton<IEventStore, EventStoreDBService>();

// Add CORS services
builder.Services.AddCors(options =>
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


// Add services to the container.
builder.Services.AddSingleton<CosmosClient>(sp =>
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

// Configure logging
builder.Logging.AddConsole();
builder.Services
       .AddEventStoreClient(builder.Configuration
                                    .GetSection("EventStore")
                                    .Get<string>()!);
builder.Services.AddSingleton<IRuleStorageService, RuleStorageService>();
builder.Services.AddTransient<IRuleSetRepository, RuleSetRepository>();
builder.Services.AddTransient<IEventStore, EventStoreService>();


builder.Services.AddTransient<IRuleProcessingService, RuleProcessingService>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseCors(); // Enable CORS

// Initialize Cosmos DB
var ruleStorageService = app.Services.GetRequiredService<IRuleStorageService>();
await ruleStorageService.InitializeCosmosClientAsync();

// Map grouped endpoints
RuleEndpoints.MapRuleEndpoints(app);

app.Run();

