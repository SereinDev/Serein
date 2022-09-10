using Serein.Items;
using System;
using System.Timers;

namespace Serein.Base
{
    internal static class TaskRunner
    {
        private static Timer Timer = new Timer(2000)
        {
            AutoReset = true,
        };

        /// <summary>
        /// 启动计时器
        /// </summary>
        public static void Start()
        {
            Timer.Elapsed += (sender, e) => Run();
            Timer.Start();
        }

        /// <summary>
        /// 遍历所有任务并运行
        /// </summary>
        private static void Run()
        {
            foreach (Task Item in Global.TaskItems)
            {
                if (Item.Enable && DateTime.Compare(Item.NextTime, DateTime.Now) <= 0)
                {
                    Item.Run();
                }
            }
        }
    }
}
