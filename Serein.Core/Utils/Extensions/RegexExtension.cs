using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Serein.Core.Utils.Extensions;

public static class RegexExtension
{
    public static bool TryParse(
        this string? pattern,
        RegexOptions options,
        [NotNullWhen(true)] out Regex? regex
    )
    {
        try
        {
            if (pattern is null)
                throw new ArgumentNullException(nameof(pattern));

            regex = new(pattern, options);
            return true;
        }
        catch
        {
            regex = null;
            return false;
        }
    }
}