using System;
using System.Runtime.InteropServices;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Xaml;

using Windows.UI;
using Windows.UI.ViewManagement;

namespace Serein.Pro.Helpers;

public partial class TitleBarHelper
{
    private const int WAINACTIVE = 0x00;
    private const int WAACTIVE = 0x01;
    private const int WMACTIVATE = 0x0006;

    [LibraryImport("user32.dll")]
    private static partial IntPtr GetActiveWindow();

    [LibraryImport("user32.dll")]
    private static partial IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

    public static void UpdateTitleBar(ElementTheme theme)
    {
        var mainWindow = (Application.Current as App)?.Host.Services.GetRequiredService<MainWindow>();
        if (mainWindow is not null && mainWindow.ExtendsContentIntoTitleBar)
        {
            if (theme == ElementTheme.Default)
            {
                var uiSettings = new UISettings();
                var background = uiSettings.GetColorValue(UIColorType.Background);

                theme = background == Colors.White ? ElementTheme.Light : ElementTheme.Dark;
            }

            if (theme == ElementTheme.Default)
            {
                theme = Application.Current.RequestedTheme == ApplicationTheme.Light ? ElementTheme.Light : ElementTheme.Dark;
            }

            mainWindow.AppWindow.TitleBar.ButtonForegroundColor = theme switch
            {
                ElementTheme.Dark => Colors.White,
                ElementTheme.Light => Colors.Black,
                _ => Colors.Transparent
            };

            mainWindow.AppWindow.TitleBar.ButtonHoverForegroundColor = theme switch
            {
                ElementTheme.Dark => Colors.White,
                ElementTheme.Light => Colors.Black,
                _ => Colors.Transparent
            };

            mainWindow.AppWindow.TitleBar.ButtonHoverBackgroundColor = theme switch
            {
                ElementTheme.Dark => Color.FromArgb(0x33, 0xFF, 0xFF, 0xFF),
                ElementTheme.Light => Color.FromArgb(0x33, 0x00, 0x00, 0x00),
                _ => Colors.Transparent
            };

            mainWindow.AppWindow.TitleBar.ButtonPressedBackgroundColor = theme switch
            {
                ElementTheme.Dark => Color.FromArgb(0x66, 0xFF, 0xFF, 0xFF),
                ElementTheme.Light => Color.FromArgb(0x66, 0x00, 0x00, 0x00),
                _ => Colors.Transparent
            };

            mainWindow.AppWindow.TitleBar.BackgroundColor = Colors.Transparent;

            //var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(mainWindow);
            //if (hwnd == GetActiveWindow())
            //{
            //    SendMessage(hwnd, WMACTIVATE, WAINACTIVE, IntPtr.Zero);
            //    SendMessage(hwnd, WMACTIVATE, WAACTIVE, IntPtr.Zero);
            //}
            //else
            //{
            //    SendMessage(hwnd, WMACTIVATE, WAACTIVE, IntPtr.Zero);
            //    SendMessage(hwnd, WMACTIVATE, WAINACTIVE, IntPtr.Zero);
            //}
        }
    }
}
