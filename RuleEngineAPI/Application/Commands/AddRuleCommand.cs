using LsRuleEngine.Interfaces;
using MediatR;
using Newtonsoft.Json.Schema;
using OneOf;
using RuleEngineAPI.Application.Interfaces;

namespace RuleEngineAPI.Commands;

public record AddRuleCommand(string RuleContent, string JsonSchema) : IRequest<OneOf<IEnumerable<IRule>, string>>;

public class AddRuleCommandHandler(IRuleManagerService ruleManagerService, IRuleStorageService storageService, ILogger<AddRuleCommandHandler> logger) : IRequestHandler<AddRuleCommand, OneOf<IEnumerable<IRule>, string>>
{
    private readonly IRuleManagerService _ruleManagerService = ruleManagerService;
    private readonly IRuleStorageService _storageService = storageService;
    private readonly ILogger<AddRuleCommandHandler> _logger = logger;

    public Task<OneOf<IEnumerable<IRule>, string>> Handle(AddRuleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var schema = JSchema.Parse(request.JsonSchema);
            var rules = _ruleManagerService.ParseRules(request.RuleContent, schema);
            // Store or process rules as needed
            _storageService.StoreRule(request.RuleContent, request.JsonSchema);

            return Task.FromResult(OneOf<IEnumerable<IRule>, string>.FromT0(rules));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Creating rule");
            return Task.FromResult(OneOf<IEnumerable<IRule>, string>.FromT1("Error message"));
        }
    }


}