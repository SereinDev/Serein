using System;
using System.Diagnostics;
using System.Text;

namespace Serein.Core.Utils.Extensions;

public static class StringExtension
{
    public static string ToUnicode(this string text)
    {
        var stringBuilder = new StringBuilder();
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] < 127)
            {
                stringBuilder.Append(text[i]);
            }
            else
            {
                stringBuilder.Append(string.Format("\\u{0:x4}", (int)text[i]));
            }
        }
        return stringBuilder.ToString();
    }

    public static string ToSizeString(this long size)
    {
        if (size < 1024)
        {
            return size.ToString() + " B";
        }

        if (size < 1024 * 1024)
        {
            return ((double)size / 1024).ToString("N1") + " KB";
        }

        if (size < 1024 * 1024 * 1024)
        {
            return ((double)size / 1024 / 1024).ToString("N1") + " MB";
        }

        if (size < (long)1024 * 1024 * 1024 * 1024)
        {
            return ((double)size / 1024 / 1024 / 1024).ToString("N1") + " GB";
        }

        return ((double)size / 1024 / 1024 / 1024 / 1024).ToString("N1") + " TB";
    }

    public static void OpenInExplorer(this string path)
    {
        if (Environment.OSVersion.Platform != PlatformID.Win32NT)
        {
            throw new PlatformNotSupportedException();
        }

        Process.Start(
            new ProcessStartInfo("explorer.exe") { Arguments = $"/e,/select,\"{path}\"" }
        );
    }

    public static string GetHexString(this byte[] targetData)
    {
        var stringBuilder = new StringBuilder();
        for (int i = 0; i < targetData.Length; i++)
        {
            stringBuilder.Append(targetData[i].ToString("x2"));
        }

        return stringBuilder.ToString();
    }
}
