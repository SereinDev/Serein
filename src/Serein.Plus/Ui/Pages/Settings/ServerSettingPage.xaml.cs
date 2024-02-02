using System;
using System.IO;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;

using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Ui.Pages.Settings;

public partial class ServerSettingPage : Page
{
    private readonly IHost _host;

    private IServiceProvider Services => _host.Services;
    private SettingProvider SettingProvider => Services.GetRequiredService<SettingProvider>();
    private ServerSetting ServerSetting => SettingProvider.Value.Server;

    public ServerSettingPage(IHost host)
    {
        _host = host;
        InitializeComponent();
        DataContext = SettingProvider;

        try
        {
            UseRelativePath.IsChecked = !Path.IsPathFullyQualified(ServerSetting.FileName);
        }
        catch
        {
            UseRelativePath.IsChecked = false;
        }
    }

    private void OnPropertyChanged(object? sender, EventArgs e)
    {
        if (IsLoaded)
            SettingProvider.SaveAsyncWithDebounce();
    }

    private void Select_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            InitialDirectory =
                !string.IsNullOrEmpty(ServerSetting.FileName) && File.Exists(ServerSetting.FileName)
                    ? Path.GetDirectoryName(ServerSetting.FileName)
                    : AppDomain.CurrentDomain.BaseDirectory,
            Title = "选择启动文件"
        };

        if (dialog.ShowDialog() ?? false)
        {
            ServerSetting.FileName = dialog.FileName;
            UseRelativePath.IsChecked = false;
            SettingProvider.Save();
        }
    }

    private void FileName_TextChanged(object sender, RoutedEventArgs e)
    {
        OnPropertyChanged(sender, e);
        try
        {
            UseRelativePath.IsChecked = !Path.IsPathFullyQualified(ServerSetting.FileName);
        }
        catch
        {
            UseRelativePath.IsChecked = false;
        }
    }

    private void UseRelativePath_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(ServerSetting.FileName))
            try
            {
                ServerSetting.FileName =
                    UseRelativePath.IsChecked == true
                        ? Path.GetRelativePath(
                            AppDomain.CurrentDomain.BaseDirectory,
                            ServerSetting.FileName
                        )
                        : Path.GetFullPath(
                            ServerSetting.FileName,
                            AppDomain.CurrentDomain.BaseDirectory
                        );
                OnPropertyChanged(sender, e);
            }
            catch (Exception)
            {
                e.Handled = true;
            }
    }
}
