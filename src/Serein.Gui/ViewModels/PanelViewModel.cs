using System;
using System.IO;
using System.Windows;
using Force.DeepCloner;
using iNKORE.UI.WPF.Modern.Controls;
using MineStatLib;
using Serein.Core.Models.Abstractions;
using Serein.Core.Services.Servers;
using Serein.Core.Utils.Extensions;
using Serein.Gui.Commands;
using Serein.Gui.Utils;
using Serein.Gui.Windows;
using MessageBox = iNKORE.UI.WPF.Modern.Controls.MessageBox;
using Timer = System.Timers.Timer;

namespace Serein.Gui.ViewModels;

public class PanelViewModel : NotifyPropertyChangedModelBase, IDisposable
{
    private readonly string _id;
    private readonly ServerManager _serverManager;
    private readonly MainWindow _mainWindow;
    private readonly Timer _timer;

    public Server Server { get; }

    public PanelViewModel(
        string id,
        Server server,
        ServerManager serverManager,
        MainWindow mainWindow
    )
    {
        _id = id;
        Server = server;
        _serverManager = serverManager;
        _mainWindow = mainWindow;

        Status = Server.Status;
        UpdateInfo();

        _timer = new(2500) { Enabled = true };
        _timer.Elapsed += (_, _) => UpdateInfo();
        Server.StatusChanged += StatusChanged;

        StartCommand = new(Start);
        StopCommand = new(Stop);
        RestartCommand = new(Restart);
        TerminateCommand = new(Terminate);
        OpenInExplorerCommand = new(OpenInExplorer);
        PluginManagerCommand = new(PluginManager);
        EditConfigCommand = new(EditConfig);
        RemoveServerCommand = new(RemoveServer);
        EnterInputOnlyCommand = new(EnterInputOnly);
    }

    public bool Status { get; private set; }

    public TimeSpan? RunTime { get; private set; }

    public string? PlayerCount { get; private set; }

    public string? Version { get; private set; }

    public int? CpuUsage { get; private set; }

    public string InputText { get; set; } = string.Empty;

    public RelayCommand StartCommand { get; }
    public RelayCommand StopCommand { get; }
    public RelayCommand RestartCommand { get; }
    public RelayCommand<Flyout> TerminateCommand { get; }
    public RelayCommand OpenInExplorerCommand { get; }
    public RelayCommand PluginManagerCommand { get; }
    public RelayCommand EditConfigCommand { get; }
    public RelayCommand RemoveServerCommand { get; }
    public RelayCommand EnterInputOnlyCommand { get; }

    public event Action? RequestClearConsole;
    public event Action<string>? RequestInputTextUpdate;

    private void Start()
    {
        try
        {
            Server.Start();
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }

    private void Stop()
    {
        try
        {
            Server.Stop();
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }

    private void Restart()
    {
        try
        {
            Server.RequestRestart();
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }

    private void Terminate(Flyout? flyout)
    {
        try
        {
            flyout?.Hide();
            Server.Terminate();
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }

    private void OpenInExplorer()
    {
        try
        {
            if (File.Exists(Server.Configuration.FileName))
            {
                Server.Configuration.FileName.OpenInExplorer();
            }
            else
            {
                throw new InvalidOperationException("启动文件不存在，无法打开其所在文件夹");
            }
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }

    private void PluginManager()
    {
        try
        {
            var window = new ServerPluginManagerWindow(Server) { Owner = _mainWindow };
            window.ShowDialog();
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }

    private void EditConfig()
    {
        var config = Server.Configuration.ShallowClone();
        var editor = new ServerConfigurationEditor(_serverManager, config, _id)
        {
            Owner = _mainWindow,
        };

        if (editor.ShowDialog() != true)
        {
            return;
        }

        editor.Configuration.ShallowCloneTo(Server.Configuration);
        _serverManager.SaveAll();
    }

    private void RemoveServer()
    {
        DialogFactory
            .ShowDeleteConfirmation($"确定要删除服务器配置（Id: {_id}）吗？")
            .ContinueWith(
                (task) =>
                {
                    if (!task.Result)
                    {
                        return;
                    }

                    try
                    {
                        _mainWindow.Dispatcher.Invoke(() => _serverManager.Remove(_id));
                    }
                    catch (Exception ex)
                    {
                        _mainWindow.Dispatcher.Invoke(() => ShowError(ex));
                    }
                }
            );
    }

    public void Input()
    {
        if (string.IsNullOrEmpty(InputText))
        {
            return;
        }

        Server.Input(InputText, true);
        InputText = string.Empty;
    }

    private void EnterInputOnly()
    {
        Input();
    }

    public void HandleHistoryUp()
    {
        if (Server.CommandHistoryIndex > 0)
        {
            Server.CommandHistoryIndex--;
        }

        if (
            Server.CommandHistoryIndex < 0
            || Server.CommandHistoryIndex >= Server.CommandHistory.Count
        )
        {
            return;
        }

        InputText = Server.CommandHistory[Server.CommandHistoryIndex];
        RequestInputTextUpdate?.Invoke(InputText);
    }

    public void HandleHistoryDown()
    {
        if (Server.CommandHistoryIndex < Server.CommandHistory.Count)
        {
            Server.CommandHistoryIndex++;
        }

        if (
            Server.CommandHistoryIndex >= 0
            && Server.CommandHistoryIndex < Server.CommandHistory.Count
        )
        {
            InputText = Server.CommandHistory[Server.CommandHistoryIndex];
        }
        else if (
            Server.CommandHistoryIndex == Server.CommandHistory.Count
            && Server.CommandHistory.Count != 0
        )
        {
            InputText = string.Empty;
        }

        RequestInputTextUpdate?.Invoke(InputText);
    }

    private void StatusChanged(object? sender, EventArgs e)
    {
        if (Server.Status)
        {
            _mainWindow.Dispatcher.Invoke(() => RequestClearConsole?.Invoke());
        }

        UpdateInfo();
    }

    private void UpdateInfo()
    {
        Status = Server.Status;

        if (Server.Status)
        {
            CpuUsage = Server.Info.CpuUsage;
            Version =
                Server.Info.Stat?.ConnectionStatus == ConnStatus.Success
                    ? Server.Info.Stat.Version
                    : null;
            PlayerCount =
                Server.Info.Stat?.ConnectionStatus == ConnStatus.Success
                    ? $"{Server.Info.Stat.CurrentPlayers}/{Server.Info.Stat.MaximumPlayers}"
                    : null;
            RunTime = DateTime.Now - Server.Info.StartTime;
        }
        else
        {
            CpuUsage = null;
            Version = null;
            PlayerCount = null;
            RunTime = null;
        }
    }

    private static void ShowError(Exception ex)
    {
        MessageBox.Show(
            $"""
            {ex.Message}

            -> {ex.GetType().FullName}
            """,
            "操作失败",
            MessageBoxButton.OK,
            MessageBoxImage.Error
        );
    }

    public void Dispose()
    {
        _timer.Dispose();
        Server.StatusChanged -= StatusChanged;
        GC.SuppressFinalize(this);
    }
}
