namespace LsRuleEngine.Parser;

public class MathExpressionResult
{
    // The numerical result of the math expression
    public double Value { get; private set; }

    // Indicates whether the calculation was successful
    public bool IsSuccessful { get; private set; }

    // Any error message, in case the calculation was not successful
    public string ErrorMessage { get; private set; }

    public MathExpressionResult(double value, bool isSuccessful, string errorMessage = "")
    {
        Value = value;
        IsSuccessful = isSuccessful;
        ErrorMessage = errorMessage;
    }

    // Static method for creating a successful result
    public static MathExpressionResult Success(double value)
    {
        return new MathExpressionResult(value, true);
    }

    // Static method for creating a failed result
    public static MathExpressionResult Failure(string errorMessage)
    {
        return new MathExpressionResult(0, false, errorMessage);
    }
}
