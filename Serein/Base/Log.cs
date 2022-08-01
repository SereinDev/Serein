using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Serein.Base
{
    public class Log
    {
        /// <summary>
        /// 颜色代码列表
        /// </summary>
        private static List<string> ColorList = new List<string>
        {
            "30", "31", "32", "33", "34", "35", "36", "37",
            "40", "41", "42", "43", "44", "45", "46", "47",
            "90", "91", "92", "93", "94", "95", "96", "97",
            "100","101","102","103","104","105","106","107"
        };

        /// <summary>
        /// 去除彩色字符和控制字符
        /// </summary>
        /// <param name="Input">输入文本</param>
        /// <returns>处理后的文本</returns>
        public static string OutputRecognition(string Input)
        {
            string Result;
            Result = Regex.Replace(Input, @"\[.*?m", string.Empty);
            Result = Regex.Replace(Result, @"", string.Empty);
            Result = Regex.Replace(Result, @"\s+?$", string.Empty);
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < Result.Length; i++)
            {
                int Unicode = Result[i];
                if (Unicode > 31 && Unicode != 127)
                {
                    sBuilder.Append(Result[i].ToString());
                }
            }
            return sBuilder.ToString();
        }

        /// <summary>
        /// 将字符串转为HTML编码的字符串
        /// </summary>
        /// <param name="Input">输入文本</param>
        /// 将字符串转为HTML编码的字符串
        /// <returns>转义后的HTML</returns>
        public static string EscapeLog(string Input)
        {
            return WebUtility.HtmlEncode(Input);
        }

        /// <summary>
        /// 彩色文本转义
        /// </summary>
        /// <param name="Input">输入文本</param>
        /// <param name="Type">输出样式</param>
        /// <returns>转义后的HTML文本</returns>
        public static string ColorLog(string Input, int Type)
        {
            Input = EscapeLog(Input);
            Input = Regex.Replace(Input, @"^>\s+?", string.Empty);
            Input = Input.Replace("[m", "[0m");
            if (Type == 1 || Type == 3)
            {
                string Output = Input;
                string Pattern = @"\[([^]+?)m([^]*)";
                if (Regex.IsMatch(Input, Pattern))
                {
                    Output = string.Empty;
                    foreach (Match Match in Regex.Matches(Input, Pattern))
                    {
                        string Arg = Match.Groups[1].Value;
                        if (string.IsNullOrEmpty(Match.Groups[2].Value))
                        {
                            continue;
                        }
                        bool Colored = true;
                        string Style = string.Empty;
                        string SpanClass = string.Empty;
                        string[] ArgList = Arg.Split(';');
                        for (int ChildArgIndex = 0; ChildArgIndex < ArgList.Length; ChildArgIndex++)
                        {
                            string ChildArg = ArgList[ChildArgIndex];
                            if (ChildArg == "1")
                            {
                                Style += "font-weight:bold;";
                            }
                            else if (ChildArg == "3")
                            {
                                Style += "font-style: italic;";
                            }
                            else if (ChildArg == "4")
                            {
                                Style += "text-decoration: underline;";
                            }
                            else if (ChildArg == "38" && ArgList[ChildArgIndex + 1] == "2" && ChildArgIndex + 4 <= ArgList.Length)
                            {
                                Style += $"color:rgb({ArgList[ChildArgIndex + 2]},{ArgList[ChildArgIndex + 3]},{ArgList[ChildArgIndex + 4]})";
                                Colored = true;
                            }
                            else if (ChildArg == "48" && ArgList[ChildArgIndex + 1] == "2" && ChildArgIndex + 4 <= ArgList.Length)
                            {
                                Style += $"background-color:rgb({ArgList[ChildArgIndex + 2]},{ArgList[ChildArgIndex + 3]},{ArgList[ChildArgIndex + 4]})";
                                Colored = true;
                            }
                            else if (ColorList.Contains(ChildArg))
                            {
                                SpanClass += "vanillaColor" + ChildArg + " ";
                                Colored = !(ChildArg == "37" || ChildArg == "47" || ChildArg == "97" || ChildArg == "107");
                            }
                        }
                        if (!Colored)
                        {
                            SpanClass += "noColored";
                        }
                        Output += $"<span style='{Style}' class='{SpanClass}'>{Match.Groups[2].Value}</span>";
                    }
                    if (Type == 3)
                    {
                        Output = Regex.Replace(Output, @"\[(SERVER|server|Server)\]", "[<span class='server'>$1</span>]");
                        Output = Regex.Replace(Output, @"\[([A-Za-z0-9\s-]+?)\]", "[<span class='plugins $1'>$1</span>]");
                        Output = Regex.Replace(Output, @"([0-9A-Za-z\._-]+\.)(py|jar|dll|exe|bat|json|lua|js|yaml|jpeg|png|jpg|csv|log)", "<span class='file'>$1$2</span>");
                        Output = Regex.Replace(Output, @"(\d{5,})", "<span class='int'>$1</span>");
                    }
                }
                else
                {
                    Output = $"<span class=\"noColored\">{Output}</span>";
                }
                if (Type == 3)
                {
                    Output = Regex.Replace(Output, @"\[(SERVER|server|Server)\]", "[<span class='server'>$1</span>]");
                    Output = Regex.Replace(Output, @"\[([A-Za-z0-9\s-]+?)\]", "[<span class='plugins $1'>$1</span>]");
                    Output = Regex.Replace(Output, @"([0-9A-Za-z\._-]+\.)(py|jar|dll|exe|bat|json|lua|js|yaml|jpeg|png|jpg|csv|log)", "<span class='file'>$1$2</span>");
                    Output = Regex.Replace(Output, @"(\d{5,})", "<span class='int'>$1</span>");
                }
                return Output;
            }
            else if (Type == 2)
            {
                Input = Regex.Replace(Input, @"\[.*?m", string.Empty);
                Input = Regex.Replace(Input, @"", string.Empty);
                Input = Regex.Replace(Input, @"([\[\s])(INFO|info|Info)", "$1<span class='info'>$2</span>");
                Input = Regex.Replace(Input, @"([\[\s])(WARNING|warning|Warning)", "$1<span class='warn'><b>$2</b></span>");
                Input = Regex.Replace(Input, @"([\[\s])(WARN|warn|Warn)", "$1<span class='warn'><b>$2</b></span>");
                Input = Regex.Replace(Input, @"([\[\s])(ERROR|error|Error)", "$1<span class='error'><b>$2</b></span>");
                Input = Regex.Replace(Input, @"\[(SERVER|server|Server)\]", "[<span class='server'>$1</span>]");
                Input = Regex.Replace(Input, @"\[([A-Za-z0-9\s-]+?)\]", "[<span class='plugins $1'>$1</span>]");
                Input = Regex.Replace(Input, @"([0-9A-Za-z\._-]+\.)(py|jar|dll|exe|bat|json|lua|js|yaml|jpeg|png|jpg|csv|log)", "<span class='file'>$1$2</span>");
                Input = Regex.Replace(Input, @"(\d{5,})", "<span class='int'>$1</span>");
                Input = $"<span class=\"noColored\">{Input}</span>";
                return Input;
            }
            else
            {
                Input = OutputRecognition(Input);
                return Input;
            }
        }
    }
}