using System;
using System.IO;

namespace Serein.Tests.Utils;

public static class TestHelper
{
    public static void Initialize(string caller)
    {
        var i = 0;

        string? path;
        do
        {
            i++;
            path = Path.Join(
                AppDomain.CurrentDomain.BaseDirectory,
                "tests",
                Path.GetFileNameWithoutExtension(caller) + "#" + i
            );
        } while (string.IsNullOrEmpty(path) || Directory.Exists(path));

        Directory.CreateDirectory(path);
        Directory.SetCurrentDirectory(path);
    }
}
