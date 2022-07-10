using Serein.Items;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Serein.Base
{
    internal class TaskManager
    {
        public static Task RunnerThread = new Task(Runner);
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
