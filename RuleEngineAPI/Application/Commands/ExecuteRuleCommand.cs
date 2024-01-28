using MediatR;
using RuleEngineAPI.Application.Interfaces;
using RuleEngineAPI.Application.Services;
using RuleEngineAPI.Infrastructure.Interfaces;

namespace RuleEngineAPI.Application.Commands;
public record ExecuteRuleCommand(string JsonData, string TypeToApplyRule) : IRequest<string>; // or IRequest<ResultType>
public class ExecuteRuleCommandHandler(IRuleManagerService ruleManagerService, IRuleStorageService ruleStorageService) : IRequestHandler<ExecuteRuleCommand, string> // or IRequestHandler<ExecuteRuleCommand, ResultType>
{
    private readonly IRuleManagerService _ruleManagerService = ruleManagerService;
    private readonly IRuleStorageService _ruleStorageService = ruleStorageService;

    public async Task<string> Handle(ExecuteRuleCommand request, CancellationToken cancellationToken)
    {
        // Retrieve the rule from Cosmos DB
        var rules = await _ruleStorageService.GetRulesByTypeAsync(request.TypeToApplyRule);

        // Execute the rule
        return _ruleManagerService.ExecuteRules(request.JsonData, rules);
    }
}
