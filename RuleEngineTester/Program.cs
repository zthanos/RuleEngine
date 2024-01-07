using Cocona;
using RuleEngineTester;
using RuleEngineTester.RuleEngine;

var builder = CoconaApp.CreateBuilder();

var app = builder.Build();


app.AddCommands<TestCommands>();

app.Run();