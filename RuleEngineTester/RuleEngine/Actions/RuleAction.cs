namespace RuleEngineTester.RuleEngine.Actions;


public class RuleAction
{
    public RuleAction(string propertyName, object? validValue, object? invalidValue)
    {
        PropertyName = propertyName;
        ValidValue = validValue;
        InvalidValue = invalidValue;
    }
    public string? Type { get; set; }
    public string PropertyName { get; set; }
    public object? ValidValue { get; set; }
    public object? InvalidValue { get; set; }
}

