using System.Globalization;
using System.Windows.Controls;

namespace Serein.Plus.Validations;

public class NotEmptyValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                return ValidationResult.ValidResult;
            }
            return new(false, "Value cannot be empty.");
        }
        return new(false, "Value must be a string.");
    }
}
