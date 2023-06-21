using NCrontab;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Serein.Base
{
    [JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
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
        [JsonIgnore]
        public bool IsRunning { get; private set; }

        /// <summary>
        /// 是否启用 - 文本
        /// </summary>
        [JsonIgnore]
        public string Enable_Text => Enable ? "启用" : "禁用";

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
                Core.Generic.Command.Run(CommandOrigin.Schedule, Command);
                CrontabSchedule crontabSchedule;
                if ((crontabSchedule = CrontabSchedule.TryParse(Cron)) is not null)
                {
                    NextTime = crontabSchedule.GetNextOccurrences(DateTime.Now, DateTime.Now.AddYears(1)).ToList()[0];
                }
                else
                {
                    Enable = false;
                }
                IsRunning = false;
            });
        }

        /// <summary>
        /// 检查是否合法
        /// </summary>
        /// <returns>是否合法</returns>
        public bool Check()
        {
            CrontabSchedule crontabSchedule;
            if (Core.Generic.Command.GetType(Command) != CommandType.Invalid && (crontabSchedule = CrontabSchedule.TryParse(Cron)) != null)
            {
                NextTime = crontabSchedule.GetNextOccurrences(DateTime.Now, DateTime.Now.AddYears(1)).ToList()[0];
                return true;
            }
            else
            {
                Enable = false;
                return false;
            }
        }

        public static Schedule FromText(string text)
        {
            string[] args = text.Split('\t');
            if (args.Length != 4)
            {
                return null;
            }
            return new()
            {
                Cron = args[0],
                Enable = args[1] == "True",
                Remark = args[2],
                Command = args[3]
            };
        }
    }
}
