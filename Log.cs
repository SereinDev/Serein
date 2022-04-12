using System;
using System.Text.RegularExpressions;
using System.Collections;

namespace Log
{
    public class Log
    {
        public  string OutputRecognition(string Input)
        {
            string Result;
            Result = Regex.Replace(Input, @"\[.+?m", "");
            Result = Regex.Replace(Result, @"", "");
            Result = Regex.Replace(Result, @"\s+?$", "");
            return Result;
        }
        public string EscapeLog(string Input)
        {
            string Result;
            Result = Regex.Replace(Input, "/", "&#47;");
            Result = Regex.Replace(Result, "\"", "&quot;");
            Result = Regex.Replace(Result, ",", "&#44;");
            Result = Regex.Replace(Result, ";", "&#58;");
            Result = Regex.Replace(Result, "<", "&lt;");
            Result = Regex.Replace(Result, ">", "&gt;");
            return Result;
        }
        public string ColorLog(string Input,int Type)
        {
            Input = Regex.Replace(Input, @"^>\s+?", "");
            Input = Regex.Replace(Input, @"\s+?$", "");
            if (Type==1 || Type == 3)
            {
                string Pattern = @"\[([^]+?)m([^]*)";
                if (! Regex.IsMatch(Input, Pattern))
                { 
                    return Input;
                }
                else
                {
                    string Style, SpanClass, ChildArg, Arg,Color;
                    string Output = "";
                    string[] ArgList;
                    string[] ColorList = {
                        "30", "31", "32", "33", "34", "35", "36", "37",
                        "40", "41", "42", "43", "44", "45", "46", "47",
                        "90", "91", "92", "93", "94", "95", "96", "97",
                        "100","101","102","103","104","105","106","107"
                    };
                    foreach (Match Match in Regex.Matches(Input, Pattern))
                    {
                        Arg = Match.Groups[1].Value;
                        if (Match.Groups[2].Value==""){
                            continue;
                        }
                        Style = "";
                        SpanClass = "";
                        Color = "";
                        ArgList = Arg.Split(';');
                        for (int ChildArgIndex = 0; ChildArgIndex < ArgList.Length; ChildArgIndex++)
                        {
                            ChildArg = ArgList[ChildArgIndex];
                            if (ChildArg == "1")
                            {
                                Style += "font-weight:bold;";
                            }
                            else if(ChildArg == "3") 
                            {
                                Style += "font-style: italic;";
                            }
                            else if (ChildArg == "4")
                            {
                                Style += "text-decoration: underline;";
                            }
                            else if (ChildArg == "38" && ArgList[ChildArgIndex + 1] == "2" && ChildArgIndex+4 <= ArgList.Length)
                            {
                                Color = $"rgb({ArgList[ChildArgIndex + 2]},{ArgList[ChildArgIndex + 3]},{ArgList[ChildArgIndex + 4]})";
                                Style += $"color:{Color}";
                                SpanClass += "colored";
                            }
                            else if (ChildArg == "48" && ArgList[ChildArgIndex + 1] == "2" && ChildArgIndex + 4 <= ArgList.Length)
                            {
                                Color = $"rgb({ArgList[ChildArgIndex + 2]},{ArgList[ChildArgIndex + 3]},{ArgList[ChildArgIndex + 4]})";
                                Style += $"background-color:{Color}";
                                SpanClass += "colored";
                            }
                            else if (((IList)ColorList).Contains(ChildArg))
                            {
                                SpanClass += "color" + ChildArg + " ";
                                if (ChildArg == "37" && SpanClass.Contains("colored"))
                                {
                                    SpanClass += "noColor";
                                }
                                else
                                {
                                    SpanClass += "colored";
                                }
                            }
                        }
                        if (SpanClass == "")
                        {
                            SpanClass += "noColor";
                        }
                        Output += $"<span style='{Style}' class='{SpanClass}'>{Match.Groups[2].Value}</span>";
                    }
                    if (Type == 3)
                    {
                        Output = Regex.Replace(Output,  @"\[(SERVER|server|Server)\]", "[<span class='server'>$1</span>]");
                        Output = Regex.Replace(Output, @"\[([A-Za-z0-9\s-]+?)\]", "[<span class='plugins $1'>$1</span>]");
                        Output = Regex.Replace(Output, @"([0-9A-Za-z\._-]+\.)(py|jar|dll|exe|bat|json|lua|js|yaml|jpeg|png|jpg|csv|log)", "<span class='file'>$1$2</span>");
                        Output = Regex.Replace(Output, @"(\d{5,})", "<span class='int'>$1</span>");
                    }
                    return Output;
                }
            }
            else if (Type == 2)
            {
                Input = Regex.Replace(Input, @"\[.+?m", "");
                Input = Regex.Replace(Input, @"", "");
                Input = Regex.Replace(Input, @"([\[\s])(INFO|info|Info)", "$1<span class='info'>$2</span>");
                Input = Regex.Replace(Input, @"([\[\s])(WARNING|warning|Warning)", "$1<span class='warn'><b>$2</b></span>");
                Input = Regex.Replace(Input, @"([\[\s])(WARN|warn|Warn)", "$1<span class='warn'><b>$2</b></span>");
                Input = Regex.Replace(Input, @"([\[\s])(ERROR|error|Error)", "$1<span class='error'><b>$2</b></span>");
                Input = Regex.Replace(Input, @"\[(SERVER|server|Server)\]", "[<span class='server'>$1</span>]");
                Input = Regex.Replace(Input, @"\[([A-Za-z0-9\s-]+?)\]", "[<span class='plugins $1'>$1</span>]");
                Input = Regex.Replace(Input, @"([0-9A-Za-z\._-]+\.)(py|jar|dll|exe|bat|json|lua|js|yaml|jpeg|png|jpg|csv|log)", "<span class='file'>$1$2</span>");
                Input = Regex.Replace(Input, @"(\d{5,})", "<span class='int'>$1</span>"); 
                return Input;
            }
            else
            {
                return Input;
            }
        }
    }
}