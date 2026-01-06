using System;
using System.Globalization;
using System.Windows.Controls;
using NCrontab;

namespace Serein.Gui.Validations;

public class CronExpressionValidationRule : ValidationRule
{
    public override ValidationResult Validate(object? value, CultureInfo cultureInfo)
    {
        if (value is string s)
        {
            try
            {
                CrontabSchedule.Parse(s);
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
