using System;
using System.Diagnostics;
using System.IO;
using System.Text;

Stopwatch stopWatch = new();
stopWatch.Start();
ConsoleColor @default = Console.ForegroundColor;

if (Environment.OSVersion.Platform == PlatformID.Win32NT)
{
    Console.Title = "Updater";
    AppDomain.CurrentDomain.ProcessExit += (_, _) =>
    {
        if (File.Exists("Updater.exe.config"))
        {
            File.Delete("Updater.exe.config");
        }
        Process.Start(new ProcessStartInfo("cmd.exe")
        {
            Arguments = "/k del /q Updater.exe & pause & exit",
            WorkingDirectory = Directory.GetCurrentDirectory(),
            UseShellExecute = false
        });
    };
}

Replace();
stopWatch.Stop();
Console.WriteLine($"\r\n替换更新完毕，用时{stopWatch.ElapsedMilliseconds}ms");
Console.ForegroundColor = @default;


void Replace()
{
    try
    {
        Console.OutputEncoding = Encoding.UTF8;
        foreach (string file in Directory.GetFiles("update", "*.*", SearchOption.TopDirectoryOnly))
        {
            if (Path.GetExtension(file.ToLowerInvariant()) != ".zip")
            {
                File.Copy(file, Path.GetFileName(file), true);
                File.Delete(file);
                Console.WriteLine($"- {Path.GetFullPath(file)} 复制成功");
            }
        }
        Console.ForegroundColor = ConsoleColor.White;
    }
    catch (Exception e)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(e.ToString());
    }
}
