using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Serein
{
    class TaskManager
    {
        public static Thread RunnerThread = new Thread(Runner) { IsBackground = true };
        public static void Runner()
        {
            while (true)
            {
                foreach (TaskItem Item in Global.TaskItems)
                {
                    if (Item.Enable && DateTime.Compare(Item.NextTime, DateTime.Now) <= 0)
                    {
                        Item.Run();
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}
