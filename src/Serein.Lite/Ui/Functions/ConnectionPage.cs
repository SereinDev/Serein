using System;
using System.Windows.Forms;
using Serein.Core.Services.Network.Connection;
using Serein.Core.Utils.Extensions;
using Serein.Lite.Utils;

namespace Serein.Lite.Ui.Functions;

public partial class ConnectionPage : UserControl
{
    private readonly ConnectionManager _connectionManager;
    private readonly System.Timers.Timer _timer;

    public ConnectionPage(ConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
        InitializeComponent();

        _timer = new(1000);
        _timer.Elapsed += (_, _) => Invoke(UpadteInfo);
        _timer.Start();

        _sentCountDynamicLabel.DataBindings.Add(
            nameof(_sentCountDynamicLabel.Text),
            _connectionManager,
            nameof(_connectionManager.Sent)
        );
        _recvCountDynamicLabel.DataBindings.Add(
            nameof(_recvCountDynamicLabel.Text),
            _connectionManager,
            nameof(_connectionManager.Received)
        );
        _connectionManager.PropertyChanged += (_, e) =>
        {
            if (
                e.PropertyName == nameof(_connectionManager.IsActive)
                || e.PropertyName == nameof(_connectionManager.ConnectedAt)
            )
            {
                Invoke(UpadteInfo);
            }
        };
    }

    private void OpenButton_Click(object sender, EventArgs e)
    {
        ConsoleWebBrowser.ClearLines();

        try
        {
            _connectionManager.Start();
        }
        catch (Exception ex)
        {
            MessageBoxHelper.ShowWarningMsgBox($"开启失败：\r\n{ex.Message}");
        }
    }

    private void CloseButton_Click(object sender, EventArgs e)
    {
        try
        {
            _connectionManager.Stop();
        }
        catch (Exception ex)
        {
            MessageBoxHelper.ShowWarningMsgBox($"关闭失败：\r\n{ex.Message}");
        }
    }

    private void UpadteInfo()
    {
        _statusDynamicLabel.Text = _connectionManager.IsActive ? "开启" : "关闭";
        _timeDynamicLabel.Text = _connectionManager.IsActive
            ? (DateTime.Now - _connectionManager.ConnectedAt).ToCommonString()
            : "-";
    }
}
