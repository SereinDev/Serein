using System;
using System.IO;

namespace Serein.Tests.Utils;

public static class TestHelper
{
    public static void Initialize(string caller)
    {
        string? path = null;
        var i = 0;

        while (string.IsNullOrEmpty(path) || Directory.Exists(path))
        {
            i++;
            path = Path.Join(
                AppDomain.CurrentDomain.BaseDirectory,
                "tests",
                Path.GetFileNameWithoutExtension(caller) + "#" + i
            );
        }

        Directory.CreateDirectory(path);
        Directory.SetCurrentDirectory(path);
    }
}
