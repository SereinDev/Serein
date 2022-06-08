using System;
using System.Threading;

namespace Serein.baseFunction
{
    internal class TaskManager
    {
        public static Thread RunnerThread = new(Runner) { IsBackground = true };
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
                Thread.Sleep(2000);
            }
        }
    }
}
