using System.Text;
using System.Text.RegularExpressions;

namespace Serein.Core.Utils;

/// <summary>
/// 输出过滤
/// </summary>
public static partial class OutputFilter
{
    /// <summary>
    /// 移除控制字符
    /// </summary>
    /// <param name="input">输入</param>
    /// <returns>移除后的文本</returns>
    public static string RemoveControlChars(string input)
    {
        var stringBuilder = new StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            var c = input[i];
            if (c > 31 && c != 127 || c == 0x1b)
            {
                stringBuilder.Append(input[i]);
            }
        }
        return stringBuilder.ToString();
    }

    private static readonly Regex ANSIEscapePattern = GetANSIEscapePattern();

    /// <summary>
    /// 移除颜色字符
    /// </summary>
    /// <param name="input">输入</param>
    /// <returns>移除后的文本</returns>
    public static string RemoveANSIEscapeChars(string input)
    {
        return ANSIEscapePattern.Replace(input, string.Empty);
    }

    /// <summary>
    /// 移除颜色字符和控制字符
    /// </summary>
    /// <param name="input">输入</param>
    /// <returns>移除后的文本</returns>
    public static string Clean(string input)
    {
        return RemoveANSIEscapeChars(RemoveControlChars(input));
    }

    [GeneratedRegex(@"\x1B\[[0-9;\?=]*[ABCDEFGHJKSTfmnsulh]")]
    private static partial Regex GetANSIEscapePattern();
}
