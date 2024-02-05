using System;
using System.Diagnostics;

namespace Serein.Core.Utils.Extensions;

public static class UrlExtension
{
    public static void OpenInBrowser(this string url)
    {
        if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            return;

        try
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        catch { }
    }

    public static void OpenInBrowser(this Uri uri)
    {
        uri.ToString().OpenInBrowser();
    }
}
