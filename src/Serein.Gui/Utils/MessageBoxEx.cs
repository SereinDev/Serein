using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MessageBox = iNKORE.UI.WPF.Modern.Controls.MessageBox;

namespace Serein.Gui.Utils;

internal static class MessageBoxEx
{
    private static string GetExceptionMessage(Exception ex)
    {
        if (ex.InnerException is null)
        {
            return $"""
                {ex.Message}

                → {ex.GetType().FullName}
                """;
        }

        var list = new List<Exception>();
        AddInner(ex);
        list.Reverse();

        var sb = new StringBuilder();

        foreach (var e in list)
        {
            sb.AppendLine(e.Message);
            sb.AppendLine($" → {e.GetType().FullName}");
            sb.AppendLine();
        }

        return sb.ToString().TrimEnd('\r', '\n');

        void AddInner(Exception e)
        {
            if (e.InnerException is not null)
            {
                AddInner(e.InnerException);
            }

            list.Add(e);
        }
    }

    public static void ShowException(Exception ex, string? title)
    {
        MessageBox.Show(
            GetExceptionMessage(ex),
            title ?? "发生未处理的异常",
            MessageBoxButton.OK,
            MessageBoxImage.Error
        );
    }

    public static async Task ShowExceptionAsync(Exception ex, string? title)
    {
        await MessageBox.ShowAsync(
            GetExceptionMessage(ex),
            title ?? "发生未处理的异常",
            MessageBoxButton.OK,
            MessageBoxImage.Error
        );
    }
}
