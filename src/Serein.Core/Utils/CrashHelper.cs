using System;
using System.IO;
using System.Reflection;
using System.Text;
using Sentry;

namespace Serein.Core.Utils;

internal static class CrashHelper
{
    public static string CreateLog(Exception e)
    {
        if (SentrySdk.IsEnabled)
        {
            SentrySdk.CaptureException(e);
        }

        try
        {
            Directory.CreateDirectory(PathConstants.LogDirectory + "/crash");

            var date = DateTime.Now;
            var fileName = PathConstants.LogDirectory + $"/crash/{date:yyyy-MM-dd}.log";

            var sb = new StringBuilder();
            var app = SereinApp.GetCurrentApp();

            sb.AppendLine($"Serein.{app.Type}");
            sb.AppendLine("版本：" + app.FullVersion);
            sb.AppendLine("程序集：" + Assembly.GetEntryAssembly()?.FullName);
            sb.AppendLine("时间：" + date.ToString("s"));
            sb.AppendLine("文件路径：" + AppDomain.CurrentDomain.BaseDirectory);
            sb.AppendLine("操作系统：" + Environment.OSVersion);
            sb.AppendLine("CLR版本：" + Environment.Version);
            sb.AppendLine("已加载的程序集：");

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    sb.AppendLine("  " + assembly.FullName);
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"  [{ex.GetType()}: {ex.Message}]");
                }
            }

            sb.AppendLine();
            sb.AppendLine(e.ToString());
            sb.AppendLine();

            File.AppendAllText(fileName, sb.ToString());
            return fileName;
        }
        catch { }
        return string.Empty;
    }
}
