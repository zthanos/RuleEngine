using Newtonsoft.Json.Schema;

namespace RuleEngineTester.RuleEngine.Parser.Common.Resolvers;

public static class Resolvers
{
    public static object ResolveOperandType(JSchema schema, string leftOperand, string rightOperandRaw)
    {
        // Retrieve the expected type for leftOperand from the JSON schema
        JSchema propertySchema = schema.Properties[leftOperand];

        if (propertySchema != null)
        {
            // Check the type defined in the schema and parse/convert accordingly
            switch (propertySchema.Type)
            {
                case JSchemaType.Integer:
                    // Parse as integer
                    return int.TryParse(rightOperandRaw, out int intValue) ? intValue : default(int);

                case JSchemaType.Number:
                    // Parse as double or float, depending on required precision
                    return double.TryParse(rightOperandRaw, out double doubleValue) ? doubleValue : default(double);

                case JSchemaType.Boolean:
                    // Parse as boolean
                    return bool.TryParse(rightOperandRaw, out bool boolValue) ? boolValue : default(bool);


                case JSchemaType.String:
                    // If the schema indicates a date/datetime format, parse the string accordingly
                    if (propertySchema.Format == "date" || propertySchema.Format == "date-time")
                    {
                        // Parse as DateTime
                        if (DateTime.TryParse(rightOperandRaw, out DateTime dateTimeValue))
                        {
                            return dateTimeValue;
                        }
                        else
                        {
                            // Handle or log the error in parsing date
                            // Depending on how you want to handle parse errors, you might return null, a default DateTime, or throw an exception
                            return null; // or some default value, or throw an exception
                        }
                    }
                    // No conversion needed for other strings
                    return rightOperandRaw;

                default:
                    // If the type is not handled, return the raw string or implement a fallback
                    return rightOperandRaw;
            }
        }

        // If the property is not found in the schema or type handling is not defined, return raw string
        return rightOperandRaw;
    }
}