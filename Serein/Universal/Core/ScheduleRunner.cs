using Serein.Base;
using System;
using System.Timers;

namespace Serein.Core
{
    internal static class TaskRunner
    {
        /// <summary>
        /// 检查定时器
        /// </summary>
        private static readonly Timer timer = new(2000)
        {
            AutoReset = true,
        };

        /// <summary>
        /// 启动计时器
        /// </summary>
        public static void Start()
        {
            timer.Elapsed += (_, _) => Run();
            timer.Start();
        }

        /// <summary>
        /// 遍历所有任务并运行
        /// </summary>
        private static void Run()
        {
            foreach (Schedule schedule in Global.Schedules)
            {
                if (!schedule.IsRunning && DateTime.Compare(schedule.NextTime, DateTime.Now) <= 0)
                {
                    schedule.Run();
                }
            }
        }
    }
}
