using RuleEngineTester.RuleEngine.Conditions;

namespace RuleEngineTester.RuleEngine.Rule.Interfaces
{
    public interface IConditionParser
    {
        List<Condition> Parse();

    }
}
