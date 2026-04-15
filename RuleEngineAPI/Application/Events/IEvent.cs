namespace RuleEngineAPI.Application.Events;
public interface IEvent
{
    Guid Id { get; }
    DateTimeOffset OccurredOn { get; }
}
