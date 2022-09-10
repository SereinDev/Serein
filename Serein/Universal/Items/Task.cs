using NCrontab;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Serein.Items
{
    [JsonObject(MemberSerialization.OptOut)]
    internal class Task
    {
        public string Cron { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        public bool Enable { get; set; } = true;

        [JsonIgnore]
        public string Enable_Text
        {
            get
            {
                return Enable ? "启用" : "禁用";
            }
        }

        [JsonIgnore]
        public DateTime NextTime { get; set; } = DateTime.Now;

        public void Run()
        {
            Enable = false;
            System.Threading.Tasks.Task RunTask = new System.Threading.Tasks.Task(() =>
            {
                Base.Command.Run(3, Command);
                NextTime = CrontabSchedule.Parse(Cron).GetNextOccurrences(DateTime.Now, DateTime.Now.AddYears(1)).ToList()[0];
                Enable = true;
            });
            RunTask.Start();
        }

        public bool Check()
        {
            if (
                !(string.IsNullOrWhiteSpace(Cron) || string.IsNullOrEmpty(Cron) ||
                string.IsNullOrWhiteSpace(Command) || string.IsNullOrEmpty(Command)
                ))
            {
                if (Base.Command.GetType(Command) == -1)
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

        public void ToObject(string Text)
        {
            string[] Texts = Text.Split('\t');
            if (Texts.Length != 4)
            {
                return;
            }
            Cron = Texts[0];
            Enable = Texts[1] == "True";
            Remark = Texts[2];
            Command = Texts[3];
            NextTime = CrontabSchedule.Parse(Cron).GetNextOccurrences(DateTime.Now, DateTime.Now.AddYears(1)).ToList()[0];
        }
    }
}
