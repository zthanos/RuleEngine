using RuleEngineTester.RuleEngine.Parser;

namespace RuleEngineTester.RuleEngine;

/// <summary>
/// Rule Name: KYCValidationRule

//When:
//  - Age > 18
//  - HomeAddress is not null
//  - Email is not null and IsValidEmail(Customer.Email)
//  - Phone is not null and IsValidPhoneNumber(Customer.Phone)

//Then:
//  - Set Customer.IsKYCValid to true
/// </summary>
public class PlainTextRules<T> : RuleParserBase<T> where T : class
{
    private readonly RuleSet ruleSet = new();

    protected readonly string _RuleText;
    public PlainTextRules(string ruleText)
    {
        _RuleText = ruleText;
    }
    private string GetTypeFromDescription(string description) => ConditionOperatorResolver.ResolveOperator(description);


    public List<IRule> Parse(string ruleText)
    {
        var ruleToAdd = new JsonRule
        {
            Name = RegexHelper.ExtractRuleName(ruleText),
            AppliesTo = RegexHelper.ExtractClassName(ruleText),
            RuleConditions = RegexHelper.ExtractConditions(ruleText),
            Actions = RegexHelper.ExtractActions(ruleText)
        };
        ruleSet.Rules.Add(ruleToAdd);
        return (List<IRule>)ProcessRuleSet(ruleSet);
    }
}