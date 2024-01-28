using MediatR;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RuleEngineAPI.Application.Commands;
using RuleEngineAPI.Application.Interfaces;
using RuleEngineAPI.Application.Services;
using RuleEngineAPI.Commands;
using RuleEngineAPI.Infrastructure.Interfaces;
using RuleEngineAPI.Services;
using RuleEngineAPI.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddSingleton<IRuleManagerService, RuleManagerService>();
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
    var client = new CosmosClient(
    accountEndpoint: "https://localhost:8081/",
    authKeyOrResourceToken: "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
);
    //var configuration = sp.GetRequiredService<IConfiguration>();
    //string connectionString = @"AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
    //return new CosmosClient(connectionString);
    return client; ;
});
builder.Services.AddSingleton<IRuleStorageService, RuleStorageService>();

// Configure logging
builder.Logging.AddConsole();



var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHsts();
}
app.UseCors(); // Enable CORS
app.UseHttpsRedirection();
app.UseRouting();
// Initialize Cosmos DB
var ruleStorageService = app.Services.GetRequiredService<IRuleStorageService>();
await ruleStorageService.InitializeCosmosClientAsync();


// Map endpoints
app.MapPost("/add-rule", async (AddRuleCommand command, IMediator mediator) =>
{
    var result = await mediator.Send(command);
    return result.Match(
        rules => Results.Ok(rules.Select(r => new RuleViewModel(r.Name, r.TypeToApplyRule, r.Conditions.Select(c => c.Description), r.Actions.Select(a => $"{a.PropertyName} = {a.Expression}")))), // Handle IEnumerable<IRule>
        errorMsg => Results.BadRequest(errorMsg) // Handle error string
    );
})
.WithOpenApi();

app.MapPost("/execute-rule", async (ExecuteRuleCommand command, IMediator mediator) =>
{
    return await mediator.Send(command);
}).WithOpenApi(); 



app.Run();

