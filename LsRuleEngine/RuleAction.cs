using Newtonsoft.Json.Linq;
using NCalc;

namespace LsRuleEngine;

public class RuleAction
{
    public string PropertyName { get; }
    private readonly object? _value;
    public string? Expression { get; } // New field to store the expression
    private readonly bool _isExpression;

    public RuleAction(string propertyName, string expression)
    {
        _isExpression = true;
        PropertyName = propertyName;
        Expression = expression;
    }
    // Constructor for direct value assignment
    public RuleAction(string propertyName, object value)
    {
        _isExpression = false;
        PropertyName = propertyName;
        _value = value;
        _isExpression = false;
    }

    public void Execute(JObject context)
    {
        if (_isExpression)
        {
            // Create an NCalc expression
            var expr = new Expression(Expression);

            // Set parameters for the expression from the context
            foreach (var property in context.Properties())
            {
                expr.Parameters[property.Name] = property.Value.ToObject<object>();
            }

            // Evaluate the expression
            var result = expr.Evaluate();

            // Update the context based on the property name and result
            context[PropertyName] = JToken.FromObject(result);
        }
        else
        {
            // Direct value assignment
            context[PropertyName] = JToken.FromObject(_value!);
        }
    }
  
}

