using MediatR;
using OneOf;
using RuleEngineAPI.Infrastructure.Interfaces;
using RuleEngineAPI.Services;

namespace RuleEngineAPI.Application.Queries
{
    public record GetRulesByTypeQuery(string TypeToApplyRule) : IRequest<OneOf<IEnumerable<RuleItem>, string>>;

    public class GetRulesByTypeQueryHandler : IRequestHandler<GetRulesByTypeQuery, OneOf<IEnumerable<RuleItem>, string>>
    {
        private readonly IRuleStorageService _storageService;
        private readonly ILogger<GetRulesByTypeQueryHandler> _logger;

        public GetRulesByTypeQueryHandler(IRuleStorageService storageService, ILogger<GetRulesByTypeQueryHandler> logger)
        {
            _storageService = storageService;
            _logger = logger;
        }

        public async Task<OneOf<IEnumerable<RuleItem>, string>> Handle(GetRulesByTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var rules = await _storageService.GetRulesByTypeAsync(request.TypeToApplyRule);
                return OneOf<IEnumerable<RuleItem>, string>.FromT0(rules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving rules");
                return OneOf<IEnumerable<RuleItem>, string>.FromT1("Error retrieving rules");
            }
        }
    }
}
