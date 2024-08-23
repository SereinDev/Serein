using System;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;
using Serein.Pro.Services;

namespace Serein.Pro.Controls;

public sealed partial class ServerPanel : UserControl
{
    private readonly string _id;
    private readonly Server _server;
    private readonly InfoBarProvider _infoBarProvider;

    public ServerPanel(string id, Server server, InfoBarProvider infoBarProvider)
    {
        _id = id;
        _server = server;
        _infoBarProvider = infoBarProvider;

        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button { Tag: string tag })
            return;

        switch (tag)
        {
            case "Start":
                try
                {
                    _server.Start();
                }
                catch (Exception ex)
                {
                    _infoBarProvider.ShowInfoBar("启动服务器失败", ex.Message, InfoBarSeverity.Warning);
                }
                break;

            case "Restart":
                break;

            case "Stop":
                try
                {
                    _server.Stop();
                }
                catch (Exception ex)
                {
                    _infoBarProvider.ShowInfoBar("关闭服务器失败", ex.Message, InfoBarSeverity.Warning);
                }
                break;

            case "Terminate":
                try
                {
                    _server.Terminate();
                }
                catch (Exception ex)
                {
                    _infoBarProvider.ShowInfoBar("强制结束服务器失败", ex.Message, InfoBarSeverity.Warning);
                }
                break;

            case "Send":
                SendInput();
                break;
            case "OpenInExplorer":
                break;
            case "OpenPluginManager":
                break;

            default:
                throw new NotSupportedException();
        }
    }

    private void SendInput()
    {
        if (_server.Status == ServerStatus.Running)
        {
            _server.Input(InputBox.Text, fromUser: true);
            InputBox.Text = string.Empty;
        }
    }

    private void InputBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            e.Handled = true;
            SendInput();
        }
    }
}
