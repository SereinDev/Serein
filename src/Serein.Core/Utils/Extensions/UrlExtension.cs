using System;
using System.Diagnostics;

namespace Serein.Core.Utils.Extensions;

public static class UrlExtension
{
    /// <summary>
    /// 在浏览器中打开
    /// </summary>
    public static void OpenInBrowser(this string url)
    {
        if (Environment.OSVersion.Platform != PlatformID.Win32NT)
        {
            return;
        }

        try
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        catch { }
    }

    /// <summary>
    /// 在浏览器中打开
    /// </summary>
    public static void OpenInBrowser(this Uri uri)
    {
        uri.ToString().OpenInBrowser();
    }
}
