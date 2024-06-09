using System.Text;
using System.Text.RegularExpressions;

namespace Serein.Core.Utils;

public partial class OutputFilter
{
    public static string RemoveControlChars(string input)
    {
        var stringBuilder = new StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            var c = input[i];
            if (c > 31 && c != 127 || c == 0x1b)
                stringBuilder.Append(input[i]);
        }
        return stringBuilder.ToString();
    }

    private static readonly Regex ColorCharsPatten = GetColorCharsPatten();

    public static string RemoveColorChars(string input)
    {
        return ColorCharsPatten.Replace(input, string.Empty);
    }

    public static string Clear(string input)
    {
        return RemoveColorChars(RemoveControlChars(input));
    }

    [GeneratedRegex(@"\x1b\[.*?m", RegexOptions.Compiled)]
    private static partial Regex GetColorCharsPatten();
}
