using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Serein.Core.Utils;

public static class CrashHelper
{
    public static string CreateLog(Exception e)
    {
        try
        {
            Directory.CreateDirectory(PathConstants.LogDirectory + "/crash");

            var date = DateTime.Now;
            var fileName = PathConstants.LogDirectory + $"/crash/{date:yyyy-MM-dd}.log";

            var sb = new StringBuilder();

            sb.AppendLine($"Serein.{SereinApp.Type}");
            sb.AppendLine("版本：" + SereinApp.FullVersion);
            sb.AppendLine("程序集：" + Assembly.GetEntryAssembly()?.FullName);
            sb.AppendLine("时间：" + date.ToString("s"));
            sb.AppendLine("文件路径：" + AppDomain.CurrentDomain.BaseDirectory);
            sb.AppendLine("操作系统：" + Environment.OSVersion.ToString());
            sb.AppendLine("CLR版本：" + Environment.Version);
            sb.AppendLine(e.ToString());
            sb.AppendLine();

            File.AppendAllText(fileName, sb.ToString());
            return fileName;
        }
        catch { }
        return string.Empty;
    }
}
