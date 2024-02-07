using MediatR;
using OneOf;
using RuleEngineAPI.Infrastructure.Interfaces;
using RuleEngineAPI.Services;

namespace RuleEngineAPI.Application.Queries;

public class GetAllRulesQuery : IRequest<OneOf<IEnumerable<RuleItem>, string>>{ }

public class GetAllRulesQueryHandler : IRequestHandler<GetAllRulesQuery, OneOf<IEnumerable<RuleItem>, string>>
{
    private readonly IRuleStorageService _storageService;
    private readonly ILogger<GetRulesByTypeQueryHandler> _logger;

    public GetAllRulesQueryHandler(IRuleStorageService storageService, ILogger<GetRulesByTypeQueryHandler> logger)
    {
        _storageService = storageService;
        _logger = logger;
    }

    public async Task<OneOf<IEnumerable<RuleItem>, string>> Handle(GetAllRulesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var rules = await _storageService.GetAllRulesAsync();
            return OneOf<IEnumerable<RuleItem>, string>.FromT0(rules);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving rules");
            return OneOf<IEnumerable<RuleItem>, string>.FromT1("Error retrieving rules");
        }
    }

}