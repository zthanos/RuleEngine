using RuleEngineTester.RuleEngine.Parser.Common;
using RuleEngineTester.RuleEngine.Parser.Common.Resolvers;
using RuleEngineTester.RuleEngine.Parser.Common.Types;
using RuleEngineTester.RuleEngine.Rule.Interfaces;

namespace RuleEngineTester.RuleEngine.Parser.PlainText;

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

    protected readonly string _RuleText;
    public PlainTextRules(string ruleText)
    {
        _RuleText = ruleText;
    }
    private string GetTypeFromDescription(string description) => ConditionOperatorResolver.ResolveOperator(description);


    public static List<IRule> Parse(string ruleText)
    {
        RuleSet ruleSet = new();

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