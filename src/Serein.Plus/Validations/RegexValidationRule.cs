using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Serein.Plus.Validations;

public class RegexValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is string s)
        {
            try
            {
                _ = new Regex(s);
                return ValidationResult.ValidResult;
            }
            catch (Exception e)
            {
                return new(false, e.Message);
            }
        }
        return new(false, "Value must be a string.");
    }
}
