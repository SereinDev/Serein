﻿using System.Threading.Tasks;

using iNKORE.UI.WPF.Modern.Controls;

namespace Serein.Plus.Dialogs;

public static class DialogHelper
{
    public static async Task<bool> ShowDeleteConfirmation(string message)
    {
        return await new ContentDialog
        {
            Content = message + "\r\n这将会永远失去！（真的很久！）",
            PrimaryButtonText = "确认",
            CloseButtonText = "取消",
            DefaultButton = ContentDialogButton.Close
        }.ShowAsync() == ContentDialogResult.Primary;
    }
}