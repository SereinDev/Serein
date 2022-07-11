using NCrontab;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Serein.Items
{
    [JsonObject(MemberSerialization.OptOut)]
    internal class TaskItem
    {
        public string Cron { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        public bool Enable { get; set; } = true;
        [JsonIgnore]
        public DateTime NextTime { get; set; } = DateTime.Now;
        public void Run()
        {
            Task RunTask = new Task(() =>
            {
                Base.Command.Run(Command);
                List<DateTime> Occurrences = CrontabSchedule.Parse(Cron).GetNextOccurrences(DateTime.Now, DateTime.Now.AddYears(1)).ToList();
                NextTime = Occurrences[0];
            }
            );
            RunTask.Start();
        }
        public bool CheckItem()
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
                    List<DateTime> Occurrences = CrontabSchedule.Parse(Cron).GetNextOccurrences(DateTime.Now, DateTime.Now.AddYears(1)).ToList();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
        public void ConvertToItem(string Text)
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
            List<DateTime> Occurrences = CrontabSchedule.Parse(Cron).GetNextOccurrences(DateTime.Now, DateTime.Now.AddYears(1)).ToList();
            NextTime = Occurrences[0];
        }
    }
}
