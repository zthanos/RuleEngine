namespace RuleEngineAPI.ViewModels;
public record RuleViewModel(string RuleName, string TypeToApply, IEnumerable<string> Conditions, IEnumerable<string> Actions);
