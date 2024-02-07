namespace RuleEngineAPI.Application.Commands;

public record CreateRuleForTypeCommand(
    Guid RuleSetId,
    string TypeToApply,
    string InitialRules,
    string Schema,
    int Version);




