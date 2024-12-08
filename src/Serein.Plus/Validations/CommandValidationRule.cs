using System;
using System.Globalization;
using System.Windows.Controls;
using Serein.Core.Models.Commands;
using Serein.Core.Services.Commands;

namespace Serein.Plus.Validations;

public class CommandValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is string s)
        {
            try
            {
                CommandParser.Parse(CommandOrigin.Null, s, true);
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
