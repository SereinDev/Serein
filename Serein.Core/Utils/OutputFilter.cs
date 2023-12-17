using System.Text;
using System.Text.RegularExpressions;

namespace Serein.Core.Utils;

public class OutputFilter
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

    public static string RemoveColorChars(string input)
    {
        return Regex.Replace(input, @"\x1b\[.*?m", string.Empty);
    }

    public static string Clear(string input)
    {
        return RemoveColorChars(RemoveControlChars(input));
    }
}
