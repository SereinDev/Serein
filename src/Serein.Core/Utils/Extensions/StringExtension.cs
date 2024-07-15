using System.Text;

namespace Serein.Core.Utils.Extensions;

public static class StringExtension
{
    public static string ToUnicode(this string text)
    {
        var stringBuilder = new StringBuilder();
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] < 127)
            {
                stringBuilder.Append(text[i]);
            }
            else
            {
                stringBuilder.Append(string.Format("\\u{0:x4}", (int)text[i]));
            }
        }
        return stringBuilder.ToString();
    }
}