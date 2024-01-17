using Cocona;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RuleEngineTester;

var builder = CoconaApp.CreateBuilder();
builder.Logging.AddDebug();
builder.Services.AddTransient<TestCommands>();


var app = builder.Build();


app.AddCommands<TestCommands>();

app.Run();