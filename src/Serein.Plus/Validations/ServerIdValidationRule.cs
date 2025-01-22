using System;
using System.Globalization;
using System.Windows.Controls;
using Serein.Core.Services.Servers;

namespace Serein.Plus.Validations;

public sealed class ServerIdValidationRule : ValidationRule
{
    public override ValidationResult Validate(object? value, CultureInfo cultureInfo)
    {
        var id = value?.ToString();

        try
        {
            ServerManager.ValidateId(id);
            return ValidationResult.ValidResult;
        }
        catch (Exception e)
        {
            return new(false, e.Message);
        }
    }
}
