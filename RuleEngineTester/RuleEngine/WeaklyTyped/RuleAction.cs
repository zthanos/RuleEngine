using Newtonsoft.Json.Linq;
using NCalc;

namespace RuleEngineTester.RuleEngine.WeaklyTyped;

public class RuleAction
{
    public string PropertyName { get; }
    private readonly object _value;
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
            context[PropertyName] = JToken.FromObject(_value);
        }
    }
    //public JToken Execute(JToken currentValue)
    //{
    //    // Example: Concatenate the current value with the action value
    //    if (currentValue is JValue jValue)
    //    {
    //        return new JValue(Value);
    //    }

    //    // Handle other scenarios or types as needed

    //    // If no match, return the current value unchanged
    //    return currentValue;
    //}
    // Method to set an expression
    //public void SetExpression(string expression)
    //{
    //    Expression = expression;
    //}

    //public JToken Execute(JToken currentValue)
    //{
    //    if (Expression != null)
    //    {
    //        // Evaluate the expression and return the result
    //        // Placeholder for expression evaluation logic
    //        // This might involve parsing the expression and executing it
    //        // based on the currentValue and other parameters
    //        return EvaluateExpression(currentValue);
    //    }
    //    else if (currentValue is JValue jValue)
    //    {
    //        // Existing simple value assignment logic
    //        return new JValue(Value);
    //    }

    //    // If no match, return the current value unchanged
    //    return currentValue;
    //}

    //private JToken EvaluateExpression(JToken currentValue)
    //{
    //    // Placeholder for expression evaluation logic
    //    // Implement a simple interpreter or use a library for complex expressions
    //    // For example, you might use NCalc or a similar library for mathematical expressions

    //    // Example implementation (pseudo-code):
    //    // var result = ExpressionInterpreter.Evaluate(Expression, currentValue, additionalParameters);
    //    // return new JValue(result);

    //    throw new NotImplementedException("Expression evaluation not implemented yet.");
    //}
}

