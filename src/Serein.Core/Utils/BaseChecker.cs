using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Serein.Core.Utils;

internal static partial class BaseChecker
{
    public static List<Process> CheckConflictProcesses()
    {
        var current = Process.GetCurrentProcess();
        var dir = Path.GetDirectoryName(current.MainModule?.FileName);
        var id = current.Id;
        var list = new List<Process>();

        foreach (var process in Process.GetProcesses())
        {
            try
            {
                if (
                    process.Id == id
                    || !process.ProcessName.Contains(
                        "serein",
                        StringComparison.InvariantCultureIgnoreCase
                    )
                    || Path.GetDirectoryName(process.MainModule?.FileName) != dir
                )
                {
                    continue;
                }

                list.Add(process);
            }
            catch { }
        }

        return list;
    }

    public static bool CheckTempFolder()
    {
        var temp = Path.GetTempPath();
        var dir = Directory.GetCurrentDirectory();
        return dir.StartsWith(temp, StringComparison.InvariantCultureIgnoreCase)
            || dir.Contains(@"AppData\Local\Temp\");
    }
}
