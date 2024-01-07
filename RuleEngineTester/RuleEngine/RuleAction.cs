namespace RuleEngineTester.RuleEngine;


public class Action
{
    public Action(string propertyName, object? validValue, object? invalidValue)
    {
        PropertyName = propertyName;
        ValidValue = validValue;
        InvalidValue = invalidValue;
    }
    public string Type { get; set; }
    public string PropertyName { get; set; }
    public object? ValidValue { get; set; }
    public object? InvalidValue { get; set; }
}

