
namespace RuleEngineTester.RuleEngine;
public interface IRule<T>
{
    IReadOnlyCollection<Action<T>> Actions { get; }
    IReadOnlyCollection<Func<T, bool>> Conditions { get; }
}