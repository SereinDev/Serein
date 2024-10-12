using System;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Force.DeepCloner;

using iNKORE.UI.WPF.Modern.Controls;

using MineStatLib;

using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;
using Serein.Plus.Dialogs;
using Serein.Plus.Pages;
using Serein.Plus.Services;
using Serein.Plus.ViewModels;
using Serein.Plus.Windows;

namespace Serein.Plus.Controls;

public partial class PanelTabItem : TabItem
{
    private readonly string _id;
    private readonly ServerManager _serverManager;
    private readonly ServerPage _page;
    private readonly MainWindow _mainWindow;
    private readonly InfoBarProvider _infoBarProvider;
    private readonly Timer _timer;
    public Server Server { get; }
    public PanelViewModel ViewModel { get; }

    public PanelTabItem(
        string id,
        Server server,
        ServerManager serverManager,
        ServerPage page,
        MainWindow mainWindow,
        InfoBarProvider infoBarProvider
    )
    {
        _id = id;
        Server = server;
        _serverManager = serverManager;
        _page = page;
        _mainWindow = mainWindow;
        _infoBarProvider = infoBarProvider;
        ViewModel = new() { Status = Server.Status };

        DataContext = this;
        InitializeComponent();
        UpdateInfo();
        Console.EnableAnsiColor();
        Console.EnableLogLevelHighlight(true);

        _timer = new(2500) { Enabled = true };
        _timer.Elapsed += (_, _) => UpdateInfo();
        Server.ServerStatusChanged += OnServerStatusChanged;
        Server.ServerOutput += Output;
    }

    private void Output(object? sender, ServerOutputEventArgs e)
    {
        switch (e.OutputType)
        {
            case ServerOutputType.Raw:
                Dispatcher.Invoke(
                    () =>
                        Console.AppendLine(
                            Server.Configuration.OutputStyle == OutputStyle.Plain
                                ? OutputFilter.RemoveColorChars(e.Data)
                                : e.Data
                        )
                );
                break;

            case ServerOutputType.InputCommand:
                if (Server.Configuration.OutputCommandUserInput)
                    Dispatcher.Invoke(() => Console.AppendLine($">{e.Data}"));
                break;

            case ServerOutputType.Information:
                Dispatcher.Invoke(() => Console.AppendNoticeLine(e.Data));
                break;

            case ServerOutputType.Error:
                Dispatcher.Invoke(() => Console.AppendErrorLine(e.Data));
                break;
        }
    }

    private void ControlButton_Click(object sender, RoutedEventArgs e)
    {
        var tag = (sender as Control)?.Tag?.ToString();

        try
        {
            switch (tag)
            {
                case "Start":
                    Server.Start();
                    break;

                case "Stop":
                    Server.Stop();
                    break;

                case "Restart":
                    Server.RequestRestart();
                    break;

                case "Terminate":
                    TerminateFlyout.Hide();
                    Server.Terminate();
                    break;

                case "OpenInExplorer":
                    if (File.Exists(Server.Configuration.FileName))
                        Server.Configuration.FileName.OpenInExplorer();
                    else
                        throw new InvalidOperationException("启动文件不存在，无法打开其所在文件夹");
                    break;

                case "PluginManager":
                    var window = new ServerPluginManagerWindow(Server) { Owner = _mainWindow };
                    window.ShowDialog();
                    break;
            }
        }
        catch (Exception ex)
        {
            _infoBarProvider.Enqueue("操作失败", ex.Message, InfoBarSeverity.Error);
        }
    }

    private void Enter_Click(object sender, RoutedEventArgs e)
    {
        Input();
        InputBox.Focus();
    }

    private void InputBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Enter:
                Input();
                e.Handled = true;
                break;

            case Key.Up:
                if (Server.CommandHistoryIndex > 0)
                    Server.CommandHistoryIndex--;

                if (
                    Server.CommandHistoryIndex >= 0
                    && Server.CommandHistoryIndex < Server.CommandHistory.Count
                )
                    InputBox.Text = Server.CommandHistory[Server.CommandHistoryIndex];

                e.Handled = true;
                InputBox.SelectionStart = InputBox.Text.Length;
                break;

            case Key.Down:
                if (Server.CommandHistoryIndex < Server.CommandHistory.Count)
                    Server.CommandHistoryIndex++;

                if (
                    Server.CommandHistoryIndex >= 0
                    && Server.CommandHistoryIndex < Server.CommandHistory.Count
                )
                    InputBox.Text = Server.CommandHistory[Server.CommandHistoryIndex];
                else if (
                    Server.CommandHistoryIndex == Server.CommandHistory.Count
                    && Server.CommandHistory.Count != 0
                )
                    InputBox.Text = string.Empty;

                e.Handled = true;
                InputBox.SelectionStart = InputBox.Text.Length;
                break;
        }
    }

    private void Input()
    {
        Server.Input(InputBox.Text, fromUser: true);
        InputBox.Text = string.Empty;
        InputBox.Focus();
    }

    private void OnServerStatusChanged(object? sender, EventArgs e)
    {
        if (Server.Status == ServerStatus.Running)
            Dispatcher.Invoke(Console.Clear);

        UpdateInfo();
    }

    private void UpdateInfo()
    {
        ViewModel.Status = Server.Status;

        if (Server.Status == ServerStatus.Running)
        {
            ViewModel.CPUUsage = Server.Info.CPUUsage;
            ViewModel.Version =
                Server.Info.Stat?.ConnectionStatus == ConnStatus.Success
                    ? Server.Info.Stat.Version
                    : null;
            ViewModel.PlayerCount =
                Server.Info.Stat?.ConnectionStatus == ConnStatus.Success
                    ? $"{Server.Info.Stat.CurrentPlayers}/{Server.Info.Stat.MaximumPlayers}"
                    : null;
            ViewModel.RunTime = DateTime.Now - Server.Info.StartTime;
        }
        else
        {
            ViewModel.CPUUsage = null;
            ViewModel.Version = null;
            ViewModel.PlayerCount = null;
            ViewModel.RunTime = null;
        }
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        switch ((sender as MenuItem)?.Tag?.ToString())
        {
            case "Edit":
                var config = Server.Configuration.ShallowClone();
                var editor = new ServerConfigurationEditor(_serverManager, config, _id);

                if (editor.ShowDialog() != true)
                    return;

                editor.Configuration.ShallowCloneTo(Server.Configuration);
                Header = Server.Configuration.Name;
                _serverManager.SaveAll();
                break;

            case "Remove":
                DialogHelper
                    .ShowDeleteConfirmation($"确定要删除服务器配置（Id: {_id}）吗？")
                    .ContinueWith(
                        (task) =>
                        {
                            if (task.Result)
                                try
                                {
                                    _serverManager.Remove(_id);
                                }
                                catch (Exception ex)
                                {
                                    _infoBarProvider.Enqueue(
                                        "删除服务器失败",
                                        ex.Message,
                                        InfoBarSeverity.Error
                                    );
                                }
                        }
                    );
                break;

            default:
                _page.RaiseMenuItemClickEvent(sender, e);
                break;
        }
    }

    private void TabItem_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
        e.Handled = Mouse.GetPosition(this).Y > 32;
    }
}
