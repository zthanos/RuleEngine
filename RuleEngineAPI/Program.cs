using RuleEngineAPI;
using RuleEngineAPI.Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();
builder.WebHost.UseUrls("http://*:5000", "https://*:5001");

ApplicationServiceRegistry.ConfigureCoreFrameworkServices(builder.Services);
ApplicationServiceRegistry.ConfigureRuleDomainServices(builder.Services);
ApplicationServiceRegistry.ConfigureEventSourcingServices(builder.Services, builder.Configuration);
ApplicationServiceRegistry.AddRuleManagementInfrastructure(builder.Services);
ApplicationServiceRegistry.ConfigureCorsPolicy(builder.Services);

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

