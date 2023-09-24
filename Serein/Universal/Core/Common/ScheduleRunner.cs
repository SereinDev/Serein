using Serein.Base;
using System;
using System.Timers;

namespace Serein.Core.Common
{
    internal static class TaskRunner
    {
        /// <summary>
        /// 检查定时器
        /// </summary>
        private static readonly Timer _timer = new(2000) { AutoReset = true, };

        /// <summary>
        /// 启动计时器
        /// </summary>
        public static void Start()
        {
            _timer.Elapsed += (_, _) => Run();
            _timer.Start();
        }

        /// <summary>
        /// 遍历所有任务并运行
        /// </summary>
        private static void Run()
        {
            lock (Global.Schedules)
            {
                foreach (Schedule schedule in Global.Schedules)
                {
                    if (
                        !schedule.IsRunning
                        && schedule.Enable
                        && DateTime.Compare(schedule.NextTime, DateTime.Now) <= 0
                    )
                    {
                        schedule.Run();
                    }
                }
            }
        }
    }
}
