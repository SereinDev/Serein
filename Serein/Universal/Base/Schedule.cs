using NCrontab;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Serein.Base
{
    [JsonObject(MemberSerialization.OptOut)]
    internal class Schedule
    {
        /// <summary>
        /// Cron表达式
        /// </summary>
        public string Cron { get; set; } = string.Empty;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = string.Empty;

        /// <summary>
        /// 命令
        /// </summary>
        public string Command { get; set; } = string.Empty;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; } = true;

        /// <summary>
        /// 是否正在运行
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// 是否启用 - 文本
        /// </summary>
        [JsonIgnore]
        public string Enable_Text
        {
            get
            {
                return Enable ? "启用" : "禁用";
            }
        }

        /// <summary>
        /// 预计执行时间
        /// </summary>
        [JsonIgnore]
        public DateTime NextTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 运行
        /// </summary>
        public void Run()
        {
            IsRunning = true;
            Task.Run(() =>
            {
                Core.Command.Run(3, Command);
                NextTime = CrontabSchedule.Parse(Cron).GetNextOccurrences(DateTime.Now, DateTime.Now.AddYears(1)).ToList()[0];
                IsRunning = false;
            });
        }

        /// <summary>
        /// 检查是否合法
        /// </summary>
        /// <returns>是否合法</returns>
        public bool Check()
        {
            if (
                !(string.IsNullOrWhiteSpace(Cron) || string.IsNullOrEmpty(Cron) ||
                string.IsNullOrWhiteSpace(Command) || string.IsNullOrEmpty(Command)
                ))
            {
                if (Core.Command.GetType(Command) == CommandType.Invalid)
                {
                    return false;
                }
                try
                {
                    NextTime = CrontabSchedule.Parse(Cron).GetNextOccurrences(DateTime.Now, DateTime.Now.AddYears(1)).ToList()[0];
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public void FromText(string text)
        {
            string[] args = text.Split('\t');
            if (args.Length != 4)
            {
                return;
            }
            Cron = args[0];
            Enable = args[1] == "True";
            Remark = args[2];
            Command = args[3];
            NextTime = CrontabSchedule.Parse(Cron).GetNextOccurrences(DateTime.Now, DateTime.Now.AddYears(1)).ToList()[0];
        }
    }
}
