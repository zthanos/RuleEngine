
namespace RuleEngineTester.RuleEngine;
public interface IRule
{
    void ApplyRules(object target);
    //IList<Action<T>> Actions { get; }
    //IList<Func<T, bool>> Conditions { get; }
}