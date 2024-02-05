using LsRuleEngine.Interfaces;
using MediatR;
using Newtonsoft.Json.Schema;
using OneOf;
using RuleEngineAPI.Domain.Interfaces;
using RuleEngineAPI.Infrastructure.Interfaces;

namespace RuleEngineAPI.Commands;

public record AddRuleCommand(string TypeToApplyRule, string RuleContent, string JsonSchema, int Version = 1) : IRequest<OneOf<IEnumerable<IRule>, string>>;

public class AddRuleCommandHandler(IRuleManagerService ruleManagerService, IRuleStorageService storageService, ILogger<AddRuleCommandHandler> logger) : IRequestHandler<AddRuleCommand, OneOf<IEnumerable<IRule>, string>>
{
    private readonly IRuleManagerService _ruleManagerService = ruleManagerService;
    private readonly IRuleStorageService _storageService = storageService;
    private readonly ILogger<AddRuleCommandHandler> _logger = logger;

    public async Task<OneOf<IEnumerable<IRule>, string>> Handle(AddRuleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var schema = JSchema.Parse(request.JsonSchema);
            var rules = _ruleManagerService.ParseRules(request.RuleContent, schema);
            // Store or process rules as needed
            await _storageService.StoreRule(request.Version, request.TypeToApplyRule, request.RuleContent, request.JsonSchema);

   
            return OneOf<IEnumerable<IRule>, string>.FromT0(rules);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Creating rule");
            return OneOf<IEnumerable<IRule>, string>.FromT1("Error message");
        }
    }


}