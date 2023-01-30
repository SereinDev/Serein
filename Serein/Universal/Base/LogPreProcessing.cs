using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Serein.Base
{
    internal static class LogPreProcessing
    {
        /// <summary>
        /// 颜色代码列表
        /// </summary>
        private static readonly List<string> ColorList = new()
        {
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "90",
            "91",
            "92",
            "93",
            "94",
            "95",
            "96",
            "97",
            "100",
            "101",
            "102",
            "103",
            "104",
            "105",
            "106",
            "107"
        };

        /// <summary>
        /// 过滤彩色字符和控制字符
        /// </summary>
        /// <param name="input">输入文本</param>
        /// <returns>处理后的文本</returns>
        public static string Filter(string input)
        {
            string result = Regex.Replace(input, @"\x1b\[.*?m", string.Empty);
            result = Regex.Replace(result, @"\x1b", string.Empty);
            StringBuilder stringBuilder = new();
            for (int i = 0; i < result.Length; i++)
            {
                int unicode = result[i];
                if (unicode > 31 && unicode != 127)
                {
                    stringBuilder.Append(result[i].ToString());
                }
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 将字符串转为HTML编码的字符串
        /// </summary>
        /// <param name="input">输入文本</param>
        /// 将字符串转为HTML编码的字符串
        /// <returns>转义后的HTML</returns>
        public static string EscapeLog(string input)
            => Regex.Replace(WebUtility.HtmlEncode(input).Replace("\n", "<br>"), @"\s", "&nbsp;");

        /// <summary>
        /// 彩色文本转义
        /// </summary>
        /// <param name="input">输入文本</param>
        /// <param name="type">输出样式</param>
        /// <returns>转义后的HTML文本</returns>
        public static string Color(string input, int type)
        {
            input = EscapeLog(input);
            input = Regex.Replace(input, @"^>\s+?", string.Empty);
            input = input.Replace("\x1b[m", "\x1b[0m");
            if (type == 1 || type == 3)
            {
                string output;
                string pattern = @"\x1b\[([^\x1b]+?)m([^\x1b]*)";
                if (Regex.IsMatch(input, pattern))
                {
                    StringBuilder mainStringBuild = new();
                    foreach (Match match in Regex.Matches(input, pattern))
                    {
                        string arg = match.Groups[1].Value;
                        if (string.IsNullOrEmpty(match.Groups[2].Value))
                        {
                            continue;
                        }
                        bool isColored = false;
                        string @class = string.Empty;
                        string[] argList = arg.Split(';');
                        StringBuilder styleStringBuilder = new();
                        for (int childArgIndex = 0; childArgIndex < argList.Length; childArgIndex++)
                        {
                            string childArg = argList[childArgIndex];
                            switch (int.TryParse(childArg, out int integerArg) ? integerArg : 0)
                            {
                                case 1:
                                    styleStringBuilder.Append("font-weight:bold;");
                                    break;
                                case 3:
                                    styleStringBuilder.Append("font-style: italic;");
                                    break;
                                case 4:
                                    styleStringBuilder.Append("text-decoration: underline;");
                                    break;
                                case 38:
                                    if (argList[childArgIndex + 1] == "2" && childArgIndex + 4 <= argList.Length)
                                    {
                                        styleStringBuilder.Append($"color:rgb({argList[childArgIndex + 2]},{argList[childArgIndex + 3]},{argList[childArgIndex + 4]});");
                                        isColored = true;
                                    }
                                    break;
                                case 48:
                                    if (argList[childArgIndex + 1] == "2" && childArgIndex + 4 <= argList.Length)
                                    {
                                        styleStringBuilder.Append($"background-color:rgb({argList[childArgIndex + 2]},{argList[childArgIndex + 3]},{argList[childArgIndex + 4]});");
                                        isColored = true;
                                    }
                                    break;
                                default:
                                    if (ColorList.Contains(childArg))
                                    {
                                        @class = $"vanillaColor{childArg}";
                                        isColored = !(childArg == "37" || childArg == "47" || childArg == "97" || childArg == "107");
                                    }
                                    break;
                            }
                        }
                        if (!isColored)
                        {
                            @class = "noColored";
                        }
                        if (string.IsNullOrEmpty(styleStringBuilder.ToString()))
                        {
                            mainStringBuild.Append($"<span class=\"{@class}\">{match.Groups[2].Value}</span>");
                        }
                        else
                        {
                            mainStringBuild.Append($"<span style=\"{styleStringBuilder}\" class=\"{@class}\">{match.Groups[2].Value}</span>");
                        }
                    }
                    output = mainStringBuild.ToString();
                }
                else
                {
                    output = $"<span class=\"noColored\">{input}</span>";
                }
                if (type == 3)
                {
                    output = Regex.Replace(output, @"\[(SERVER)\]", "[<span class='server'>$1</span>]", RegexOptions.IgnoreCase);
                    output = Regex.Replace(output, @"(INFO)", "<span class='info'>$1</span>", RegexOptions.IgnoreCase);
                    output = Regex.Replace(output, @"(WARN(ING)?)", "<span class='warn'>$1</span>", RegexOptions.IgnoreCase);
                    output = Regex.Replace(output, @"(ERROR)", "<span class='error'>$1</span>", RegexOptions.IgnoreCase);
                    output = Regex.Replace(output, @"\[([A-Za-z0-9\s-]+?)\]", "[<span class='plugins $1'>$1</span>]", RegexOptions.IgnoreCase);
                    output = Regex.Replace(output, @"(\d{5,})", "<span class='int'>$1</span>");
                }
                return output;
            }
            else if (type == 2)
            {
                input = Regex.Replace(input, @"\x1b\[.*?m", string.Empty);
                input = Regex.Replace(input, @"(INFO)", "<span class='info'>$1</span>", RegexOptions.IgnoreCase);
                input = Regex.Replace(input, @"(WARN(ING)?)", "<span class='warn'><b>$1</b></span>", RegexOptions.IgnoreCase);
                input = Regex.Replace(input, @"(ERROR)", "<span class='error'><b>$1</b></span>", RegexOptions.IgnoreCase);
                input = Regex.Replace(input, @"\[(SERVER)\]", "[<span class='server'>$1</span>]", RegexOptions.IgnoreCase);
                input = Regex.Replace(input, @"\[([A-Za-z0-9\s-]+?)\]", "[<span class='plugins $1'>$1</span>]", RegexOptions.IgnoreCase);
                input = Regex.Replace(input, @"(\d{5,})", "<span class='int'>$1</span>");
                input = $"<span class=\"noColored\">{input}</span>";
                return input;
            }
            else
            {
                input = Filter(input);
                return input;
            }
        }
    }
}