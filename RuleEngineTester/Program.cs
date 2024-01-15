using Cocona;
using RuleEngineTester;

var builder = CoconaApp.CreateBuilder();

var app = builder.Build();


app.AddCommands<TestCommands>();

app.Run();