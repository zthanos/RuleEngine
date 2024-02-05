using RuleEngineAPI.Domain.Aggregates;
using RuleEngineAPI.Infrastructure.Interfaces;

namespace RuleEngineAPI.Infrastructure.Services;



public class RuleSetRepository(IEventStore eventStore): IRuleSetRepository
{
    private readonly IEventStore _eventStore = eventStore;

    public async Task<RuleSet> GetByIdAsync(string typeToApplyRule)
    {
        var events = await _eventStore.ReadEventsAsync(typeToApplyRule);
        var ruleSet = new RuleSet(); // Assuming default constructor is available; adjust as needed.
        ruleSet.LoadFromHistory(events); // Ensure RuleSet has a method to apply events.
        return ruleSet;
    }
}

public interface IRuleSetRepository
{
    Task<RuleSet> GetByIdAsync(string typeToApplyRule);
}