using System.Collections.ObjectModel;
namespace RuleEngineTester.RuleEngine;

public class LsRule<T> : IRule<T>
{
    public IReadOnlyCollection<Func<T, bool>> Conditions { get; }
    public IReadOnlyCollection<Action<T>> Actions { get; }

    public LsRule(IEnumerable<Func<T, bool>> conditions, IEnumerable<Action<T>> actions)
    {
        Conditions = new ReadOnlyCollection<Func<T, bool>>(conditions.ToList());
        Actions = new ReadOnlyCollection<Action<T>>(actions.ToList());

    }
}