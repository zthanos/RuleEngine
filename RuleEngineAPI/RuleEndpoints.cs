using MediatR;
using RuleEngineAPI.Application.Commands;
using RuleEngineAPI.Application.Queries;
using RuleEngineAPI.Application.Services;
using RuleEngineAPI.Commands;
using RuleEngineAPI.ViewModels;

namespace RuleEngineAPI;

public static class RuleEndpoints 
{
    public static void MapRuleEndpoints(WebApplication app)
    {

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

        app.MapPost("/execute-rule", async (ExecuteRuleCommand command, IRuleProcessingService processor) =>
        {
            var executionResult = await processor.ProcessRuleExecution(command);
            return Results.Ok(executionResult);
        }).WithOpenApi();


        app.MapGet("/api/rules/{type}", async (string type, IMediator mediator) =>
        {
            var query = new GetRulesByTypeQuery(type);
            var result = await mediator.Send(query);

            return result.Match(
                rules => Results.Ok(rules),
                error => Results.Problem(error) // or Results.BadRequest(error) based on your error handling strategy
            );
        });


        app.MapGet("/api/rules/", async (IMediator mediator) =>
        {
            var query = new GetAllRulesQuery();
            var result = await mediator.Send(query);

            return result.Match(
                rules => Results.Ok(rules),
                error => Results.Problem(error) // or Results.BadRequest(error) based on your error handling strategy
            );
        });





        app.MapPost("/create-rule", async (CreateRuleForTypeCommand command, IRuleProcessingService processor) =>
        {
            await processor.ProcessCreateRuleForType(command);

        });


    }
}
