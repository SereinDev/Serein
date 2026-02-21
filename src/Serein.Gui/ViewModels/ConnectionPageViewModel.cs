using System;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using Serein.Core.Services.Network.Connection;
using Serein.Core.Utils.Extensions;
using Serein.Gui.Commands;
using Serein.Gui.Utils;

namespace Serein.Gui.ViewModels;

public class ConnectionPageViewModel
{
    private readonly ConnectionManager _connectionManager;
    private readonly Timer _timer;

    public ConnectionPageViewModel(ConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
        _timer = new(1000) { AutoReset = true, Enabled = true };

        OpenCommand = new(Open);
        CloseCommand = new(Close);

        UpdateRunTimeText();

        _timer.Elapsed += (_, _) => Application.Current.Dispatcher.Invoke(UpdateRunTimeText);
        _connectionManager.PropertyChanged += ConnectionManager_PropertyChanged;
    }

    public ConnectionManager Manager => _connectionManager;

    public string RunTimeText { get; set; } = "-";

    public RelayCommand OpenCommand { get; }

    public RelayCommand CloseCommand { get; }

    public event Action? RequestClearConsole;

    public void OnLoaded()
    {
        _timer.Start();
        UpdateRunTimeText();
    }

    public void OnUnloaded()
    {
        _timer.Stop();
    }

    private void ConnectionManager_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_connectionManager.StartedAt))
        {
            Application.Current.Dispatcher.Invoke(UpdateRunTimeText);
        }
    }

    private void Open()
    {
        try
        {
            _connectionManager.Start();
            RequestClearConsole?.Invoke();
        }
        catch (Exception ex)
        {
            MessageBoxEx.ShowException(ex, "操作失败");
        }
    }

    private void Close()
    {
        try
        {
            _connectionManager.Stop();
        }
        catch (Exception ex)
        {
            MessageBoxEx.ShowException(ex, "操作失败");
        }
    }

    private void UpdateRunTimeText()
    {
        RunTimeText = _connectionManager.IsActive
            ? (
                _connectionManager.StartedAt is { } startedAt
                    ? ((TimeSpan?)(DateTime.Now - startedAt)).ToCommonString() ?? "-"
                    : "-"
            )
            : "-";
    }
}
