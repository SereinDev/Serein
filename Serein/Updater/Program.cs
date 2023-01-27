using System.Text;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Serein.Updater
{
    static class Program
    {
        [STAThread]
        private static void Main()
        {
            ConsoleColor @default = Console.ForegroundColor;
            try
            {
                Console.OutputEncoding = Encoding.UTF8;
                Console.ForegroundColor = ConsoleColor.White;
                string[] args = Environment.GetCommandLineArgs();
                int pid = int.Parse(args[1]);
                int i = 0;
                while (true)
                {
                    if (IsExited(pid))
                    {
                        break;
                    }
                    Task.Delay(100).GetAwaiter().GetResult();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"进程{pid}未退出，即将进行第{i}次尝试");
                    Console.ForegroundColor = ConsoleColor.White;
                    i++;
                    if (i > 20)
                    {
                        throw new TimeoutException();
                    }
                }
                Task.Delay(100).GetAwaiter().GetResult();
                foreach (string file in Directory.GetFiles("cache", "*.*", SearchOption.TopDirectoryOnly))
                {
                    if (Path.GetExtension(file.ToLowerInvariant()) != ".zip")
                    {
                        File.Copy(file, Path.GetFileName(file), true);
                        Console.WriteLine($"{Path.GetFileName(file)}复制成功");
                    }
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.ToString());
            }
            Console.ForegroundColor = @default;
            Task.Delay(500).GetAwaiter().GetResult();
        }

        private static bool IsExited(int pid)
        {
            try
            {
                Process mainProcess = Process.GetProcessById(pid);
                return mainProcess.HasExited;
            }
            catch (ArgumentException)
            {
                return true;
            }
        }
    }
}
