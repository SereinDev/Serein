using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Serein.Updater
{
    static class Program
    {
        [STAThread]
        private static void Main()
        {
            ConsoleColor @default = Console.ForegroundColor;
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                Console.Title = "Updater";
            }
            try
            {
                Console.OutputEncoding = Encoding.UTF8;
                Console.WriteLine(" [ A Updater For Serein ]\r\nAll is ready to start.\r\n");
                Task.Delay(1000).GetAwaiter().GetResult();
                foreach (string file in Directory.GetFiles("cache", "*.*", SearchOption.TopDirectoryOnly))
                {
                    if (Path.GetExtension(file.ToLowerInvariant()) != ".zip")
                    {
                        File.Copy(file, Path.GetFileName(file), true);
                        File.Delete(file);
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
            Task.Delay(1000).GetAwaiter().GetResult();
        }
    }
}
