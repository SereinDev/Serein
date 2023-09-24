using Serein.Base;
using Serein.Utils.Output;
using System;
using System.IO;
using System.Text;

namespace Serein.Utils.IO
{
    internal static class Log
    {
        /// <summary>
        /// 控制台日志
        /// </summary>
        /// <param name="line">行文本</param>
        public static void Console(string line)
        {
            if (Global.Settings.Server.EnableLog)
            {
                Directory.CreateDirectory(Path.Combine("logs", "console"));
                try
                {
                    lock (FileLock.Console)
                    {
                        File.AppendAllText(
                            Path.Combine("logs", "console", $"{DateTime.Now:yyyy-MM-dd}.log"),
                            LogPreProcessing.Filter(line.TrimEnd('\n', '\r')) + Environment.NewLine,
                            Encoding.UTF8
                        );
                    }
                }
                catch (Exception e)
                {
                    Logger.Output(LogType.Debug, e);
                }
            }
        }

        /// <summary>
        /// 机器人消息日志
        /// </summary>
        /// <param name="line">行文本</param>
        public static void Msg(string line)
        {
            if (Global.Settings.Bot.EnableLog)
            {
                Directory.CreateDirectory(Path.Combine("logs", "msg"));
                try
                {
                    lock (FileLock.Msg)
                    {
                        File.AppendAllText(
                            Path.Combine("logs", "msg", $"{DateTime.Now:yyyy-MM-dd}.log"),
                            LogPreProcessing.Filter(line.TrimEnd('\n', '\r')) + Environment.NewLine,
                            Encoding.UTF8
                        );
                    }
                }
                catch (Exception e)
                {
                    Logger.Output(LogType.Debug, e);
                }
            }
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="line">行文本</param>
        public static void Debug(string line)
        {
            Directory.CreateDirectory(Path.Combine("logs", "debug"));
            try
            {
                lock (IO.Log.FileLock.Debug)
                {
                    File.AppendAllText(
                        Path.Combine("logs", "debug", $"{DateTime.Now:yyyy-MM-dd}.log"),
                        $"{DateTime.Now:T} {line}\n",
                        Encoding.UTF8
                    );
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        /// <summary>
        /// 文件读写锁
        /// </summary>
        public struct FileLock
        {
            public static readonly object Console = new(),
                Msg = new(),
                Crash = new(),
                Debug = new();
        }
    }
}
