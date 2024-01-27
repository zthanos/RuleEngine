using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RuleEngineAPI.Application.Interfaces;
using RuleEngineAPI.Application.Services;
using RuleEngineAPI.Commands;
using RuleEngineAPI.Services;
using RuleEngineAPI.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddSingleton<IRuleStorageService, RuleStorageService>();
builder.Services.AddSingleton<IRuleManagerService, RuleManagerService>();

// Configure logging
builder.Logging.AddConsole();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



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


app.Run();

