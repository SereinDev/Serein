using System.Collections.Generic;
using System.Text;
using RegExp = System.Text.RegularExpressions;
#if WPF
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;
#endif
using Serein.Base;

namespace Serein.Utils
{
    internal static class LogPreProcessing
    {
#if !CONSOLE

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
#endif

        /// <summary>
        /// 过滤彩色字符和控制字符
        /// </summary>
        /// <param name="input">输入文本</param>
        /// <returns>处理后的文本</returns>
        public static string Filter(string input)
        {
            string result = RegExp.Regex.Replace(input, @"\x1b\[.*?m", string.Empty);
            result = RegExp.Regex.Replace(result, @"\x1b", string.Empty);
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

#if WINFORM
        /// <summary>
        /// 将字符串转为HTML编码的字符串
        /// </summary>
        /// <param name="input">输入文本</param>
        /// 将字符串转为HTML编码的字符串
        /// <returns>转义后的HTML</returns>
        public static string EscapeLog(string input)
            => RegExp.Regex.Replace(System.Net.WebUtility.HtmlEncode(input).Replace("\n", "<br>"), @"\s", "&nbsp;");

        /// <summary>
        /// 彩色文本转义
        /// </summary>
        /// <param name="input">输入文本</param>
        /// <param name="type">输出样式</param>
        /// <returns>转义后的HTML文本</returns>
        public static string ColorLine(string input, int type)
        {
            input = EscapeLog(input);
            input = RegExp.Regex.Replace(input, @"^>\s+?", string.Empty);
            input = input.Replace("\x1b[m", "\x1b[0m");
            if (type == 1 || type == 3)
            {
                string output;
                string pattern = @"\x1b\[([^\x1b]+?)m([^\x1b]*)";
                if (RegExp.Regex.IsMatch(input, pattern))
                {
                    StringBuilder mainStringBuild = new();
                    foreach (RegExp.Match match in RegExp.Regex.Matches(input, pattern))
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
                    output = RegExp.Regex.Replace(output, @"\[(SERVER)\]", "[<span class='server'>$1</span>]", RegExp.RegexOptions.IgnoreCase);
                    output = RegExp.Regex.Replace(output, @"(INFO)", "<span class='info'>$1</span>", RegExp.RegexOptions.IgnoreCase);
                    output = RegExp.Regex.Replace(output, @"(WARN(ING)?)", "<span class='warn'>$1</span>", RegExp.RegexOptions.IgnoreCase);
                    output = RegExp.Regex.Replace(output, @"(ERROR)", "<span class='error'>$1</span>", RegExp.RegexOptions.IgnoreCase);
                    output = RegExp.Regex.Replace(output, @"\[([A-Za-z0-9\s-]+?)\]", "[<span class='plugins $1'>$1</span>]", RegExp.RegexOptions.IgnoreCase);
                    output = RegExp.Regex.Replace(output, @"(\d{5,})", "<span class='int'>$1</span>");
                }
                return output;
            }
            else if (type == 2)
            {
                input = RegExp.Regex.Replace(input, @"\x1b\[.*?m", string.Empty);
                input = RegExp.Regex.Replace(input, @"(INFO)", "<span class='info'>$1</span>", RegExp.RegexOptions.IgnoreCase);
                input = RegExp.Regex.Replace(input, @"(WARN(ING)?)", "<span class='warn'><b>$1</b></span>", RegExp.RegexOptions.IgnoreCase);
                input = RegExp.Regex.Replace(input, @"(ERROR)", "<span class='error'><b>$1</b></span>", RegExp.RegexOptions.IgnoreCase);
                input = RegExp.Regex.Replace(input, @"\[(SERVER)\]", "[<span class='server'>$1</span>]", RegExp.RegexOptions.IgnoreCase);
                input = RegExp.Regex.Replace(input, @"\[([A-Za-z0-9\s-]+?)\]", "[<span class='plugins $1'>$1</span>]", RegExp.RegexOptions.IgnoreCase);
                input = RegExp.Regex.Replace(input, @"(\d{5,})", "<span class='int'>$1</span>");
                input = $"<span class=\"noColored\">{input}</span>";
                return input;
            }
            else
            {
                input = Filter(input);
                return input;
            }
        }
#endif

#if WPF
        public static Paragraph Color(LogType level, string line)
        {
            Paragraph paragraph = new()
            {
                Margin = new(0, 0, 0, 0),
                FontFamily = new("Consolas,微软雅黑")
            };
            switch (level)
            {
                case LogType.Server_Output:
                    switch (Global.Settings.Server.OutputStyle)
                    {
                        case 1:
                        case 3:
                            return paragraph.EscapeAnsiCode(line);
                        case 2:
                            return paragraph.Highlight(line);
                    }
                    break;
                case LogType.Plugin_Notice:
                case LogType.Bot_Notice:
                case LogType.Server_Notice:
                    paragraph.Inlines.Add(new Bold(new Run("[Serein]"))
                    {
                        Foreground = Brushes.SteelBlue
                    });
                    paragraph.Inlines.Add(" ");
                    break;
                case LogType.Warn:
                case LogType.Plugin_Warn:
                    paragraph.Inlines.Add(new Bold(new Run("[!]"))
                    {
                        Foreground = Brushes.Peru
                    });
                    paragraph.Inlines.Add(" ");
                    break;
                case LogType.Error:
                case LogType.Bot_Error:
                case LogType.Plugin_Error:
                    paragraph.Inlines.Add(new Bold(new Run("[×]"))
                    {
                        Foreground = Brushes.Crimson
                    });
                    paragraph.Inlines.Add(" ");
                    break;
                case LogType.Bot_Receive:
                    paragraph.Inlines.Add(new Bold(new Run("[↓]"))
                    {
                        Foreground = Brushes.LimeGreen
                    });
                    paragraph.Inlines.Add(" ");
                    break;
                case LogType.Bot_Send:
                    paragraph.Inlines.Add(new Bold(new Run("[↑]"))
                    {
                        Foreground = Brushes.DodgerBlue
                    });
                    paragraph.Inlines.Add(" ");
                    break;
            }
            paragraph.Inlines.Add(line);
            return paragraph;
        }

        private static Paragraph Highlight(this Paragraph paragraph, string line)
        {
            foreach (string words in RegExp.Regex.Split(Filter(line), @"(?<=\b)(info|warn(ing)?|error|debug|\d{5,}|true|false)(?=\b)", RegExp.RegexOptions.IgnoreCase))
            {
                Run run = new(words);
                run.Foreground = words.ToLowerInvariant() switch
                {
                    "info" => Brushes.MediumTurquoise,
                    "warn" => Brushes.Gold,
                    "warning" => Brushes.Gold,
                    "error" => Brushes.Crimson,
                    "debug" => Brushes.DarkOrchid,
                    "true" => Brushes.YellowGreen,
                    "false" => Brushes.Tomato,
                    _ => RegExp.Regex.IsMatch(words, @"^\d+$") ? Brushes.Teal : run.Foreground,
                };
                paragraph.Inlines.Add(run);
            }
            return paragraph;
        }

        private static Paragraph EscapeAnsiCode(this Paragraph paragraph, string line)
        {
            if (!line.Contains("\x1b"))
            {
                paragraph.Inlines.Add(line);
                return paragraph;
            }
            string[] texts = line.TrimStart('\x1b').Split('\x1b');
            for (int i = 0; i < texts.Length; i++)
            {
                RegExp.Match match = RegExp.Regex.Match(texts[i], @"\[(.+?)m(.*)", RegExp.RegexOptions.Compiled);
                if (match.Groups.Count <= 1)
                {
                    continue;
                }
                string[] args = match.Groups[1].Value.Split(';');
                Inline inline = new Run(match.Groups[2].Value);
                bool hasColor = false;
                for (int j = 0; j < args.Length; j++)
                {
                    string childArg = args[j];
                    if (childArg == "1")
                    {
                        inline.FontWeight = FontWeight.FromOpenTypeWeight(700);
                    }
                    else if (childArg == "3")
                    {
                        inline.FontStyle = FontStyles.Italic;
                    }
                    else if (childArg == "4")
                    {
                        inline.TextDecorations.Add(TextDecorations.Underline);
                    }
                    else if (childArg == "38" && args[j + 1] == "2" && j + 4 <= args.Length)
                    {
                        inline.Foreground = new SolidColorBrush()
                        {
                            Color = System.Windows.Media.Color.FromRgb(
                                (byte)(int.TryParse(args[j + 2], out int r) ? r : 0),
                                (byte)(int.TryParse(args[j + 3], out int g) ? g : 0),
                                (byte)(int.TryParse(args[j + 4], out int b) ? b : 0)
                                )
                        };
                        hasColor = true;
                    }
                    else if (childArg == "48" && args[j + 1] == "2" && j + 4 <= args.Length)
                    {
                        inline.Background = new SolidColorBrush()
                        {
                            Color = System.Windows.Media.Color.FromRgb(
                                (byte)(int.TryParse(args[j + 2], out int r) ? r : 0),
                                (byte)(int.TryParse(args[j + 3], out int g) ? g : 0),
                                (byte)(int.TryParse(args[j + 4], out int b) ? b : 0)
                                )
                        };
                        hasColor = true;
                    }
                    else if (ColorList.Contains(childArg) && !(childArg == "37" || childArg == "47" || childArg == "97" || childArg == "107"))
                    {
                        inline.Foreground = childArg switch
                        {
                            "31" => Brushes.DarkRed,
                            "32" => Brushes.DarkGreen,
                            "33" => Brushes.Goldenrod,
                            "34" => Brushes.DarkBlue,
                            "35" => Brushes.DarkMagenta,
                            "36" => Brushes.DarkCyan,
                            "91" => Brushes.Red,
                            "92" => Brushes.Green,
                            "93" => Brushes.Gold,
                            "94" => Brushes.Blue,
                            "95" => Brushes.Magenta,
                            "96" => Brushes.Cyan,
                            _ => inline.Foreground
                        };
                        inline.Background = childArg switch
                        {
                            "41" => Brushes.DarkRed,
                            "42" => Brushes.DarkGreen,
                            "43" => Brushes.Goldenrod,
                            "44" => Brushes.DarkBlue,
                            "45" => Brushes.DarkMagenta,
                            "46" => Brushes.DarkCyan,
                            "101" => Brushes.Red,
                            "102" => Brushes.Green,
                            "103" => Brushes.Gold,
                            "104" => Brushes.Blue,
                            "105" => Brushes.Magenta,
                            "106" => Brushes.Cyan,
                            _ => inline.Background
                        };
                        hasColor = true;
                    }
                }
                if (hasColor || Global.Settings.Server.OutputStyle == 1)
                {
                    paragraph.Inlines.Add(inline);
                }
                else
                {
                    paragraph.Highlight(match.Groups[2].Value);
                }
            }
            return paragraph;
        }

        public class Line
        {
            public LogType LogType;
            public string Text;

            public Line(LogType logType, string text)
            {
                LogType = logType;
                Text = text;
            }
        }
#endif
    }
}
