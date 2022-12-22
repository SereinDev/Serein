using Serein.Items;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Serein
{
    internal static class Logger
    {
        public static readonly int Type = 0;
        private static readonly object Lock = new object();

        public static void Out(LogType Type, params object[] objects)
        {
            string Line = string.Empty;
            foreach (var o in objects)
            {
                if (o != null)
                    Line += o.ToString() + " ";
            }
            Line = Line.TrimEnd();
            switch (Type)
            {
                case LogType.Debug:
                    if (Global.Settings.Serein.Debug)
                    {
                        StackTrace st = new StackTrace(true);
                        Line =
                            $"[{st.GetFrame(1).GetMethod().DeclaringType}" +
                            $"{(Global.Settings.Serein.DetailDebug ? " " + st.GetFrame(1).GetMethod() : "." + st.GetFrame(1).GetMethod().Name)}] " +
                            $"{Line}";
                        WriteLine(4, Line);
                        if (!Directory.Exists("logs/debug"))
                            Directory.CreateDirectory("logs/debug");
                        try
                        {
                            File.AppendAllText(
                                $"logs/debug/{DateTime.Now:yyyy-MM-dd}.log",
                                $"{DateTime.Now:T} {Line}\n",
                                Encoding.UTF8
                                );
                        }
                        catch { }
                    }
                    break;
                case LogType.Info:
                case LogType.Server_Notice:
                case LogType.Bot_Notice:
                case LogType.Plugin_Notice:
                    WriteLine(1, Line, true);
                    break;
                case LogType.Warn:
                    WriteLine(2, Line, true);
                    break;
                case LogType.Plugin_Warn:
                    WriteLine(2, Line);
                    break;
                case LogType.Error:
                case LogType.Bot_Error:
                    WriteLine(3, Line, true);
                    break;
                case LogType.Plugin_Error:
                    WriteLine(3, Line);
                    break;
                case LogType.Null:
                case LogType.Server_Output:
                case LogType.Bot_Output:
                    WriteLine(0, Line);
                    break;
                case LogType.Bot_Receive:
                    WriteLine(1, $"\x1b[92m[↓]\x1b[0m {Line}");
                    break;
                case LogType.Bot_Send:
                    WriteLine(1, $"\x1b[36m[↑]\x1b[0m {Line}");
                    break;
                case LogType.Plugin_Info:
                    WriteLine(1, $"{Line}");
                    break;
                case LogType.Version_New:
                    WriteLine(1, $"当前版本：{Global.VERSION} （发现新版本:{Line}，你可以打开" +
                        $"\x1b[4m\x1b https://github.com/Zaitonn/Serein/releases/latest \x1b[0m获取最新版）");
                    break;
                case LogType.Version_Latest:
                    WriteLine(1, "获取更新成功，当前已是最新版:)");
                    break;
                case LogType.Version_Failure:
                    WriteLine(3, "更新获取异常：\n" + Line);
                    break;
            }
        }

        /// <summary>
        /// 处理输出消息
        /// </summary>
        /// <param name="Level">输出等级</param>
        /// <param name="Line">输出行</param>
        private static void WriteLine(int Level, string Line, bool SereinTitle = false)
        {
            if (Line == "#clear" || string.IsNullOrEmpty(Line) || string.IsNullOrWhiteSpace(Line))
                return;
            if (Line.Contains("\r\n"))
            {
                Line.Split('\n').ToList().ForEach((i) => WriteLine(Level, i.Replace("\r", string.Empty), SereinTitle));
                return;
            }
            System.Console.ForegroundColor = ConsoleColor.Gray;
            string Prefix = $"{DateTime.Now:T} ";
            lock (Lock)
            {
                switch (Level)
                {
                    case 1:
                        Prefix += "\x1b[97m[Info]\x1b[0m ";
                        break;
                    case 2:
                        Prefix += "\x1b[1m\x1b[93m[Warn]\x1b[0m\x1b[93m ";
                        break;
                    case 3:
                        Prefix += "\x1b[1m\x1b[91m[Error]\x1b[0m\x1b[91m";
                        break;
                    case 4:
                        Prefix += "\x1b[95m[Debug]\x1b[0m";
                        break;
                    default:
                        Prefix += "\x1b[97m";
                        break;
                }
                if (SereinTitle)
                    if (Level == 1)
                        Prefix += "\x1b[96m[Serein]\x1b[0m ";
                    else if (Level <= 3)
                        Prefix += "[Serein] ";
                if (Level >= 1)
                    Line = Prefix + Line;
                if (!Global.Settings.Serein.ColorfulLog)
                    System.Console.WriteLine(System.Text.RegularExpressions.Regex.Replace(Line, @"\[.*?m", string.Empty));
                else
                    System.Console.WriteLine(Line + "\x1b[0m");
                System.Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static bool MsgBox(string Text, string Caption, int Buttons, int Icon)
        {
            Text = Text.Replace(":(", string.Empty).Trim('\r', '\n');
            switch (Icon)
            {
                case 48:
                    Out(LogType.Warn, Text);
                    break;
                case 16:
                    Out(LogType.Error, Text);
                    break;
            }
            return true;
        }
    }
}
