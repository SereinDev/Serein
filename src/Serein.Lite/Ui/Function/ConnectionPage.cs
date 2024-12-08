using System;
using System.Windows.Forms;
using Serein.Core.Services.Network.Connection;
using Serein.Core.Utils.Extensions;
using Serein.Lite.Utils;

namespace Serein.Lite.Ui.Function;

public partial class ConnectionPage : UserControl
{
    private readonly WsConnectionManager _wsConnectionManager;
    private readonly System.Timers.Timer _timer;

    public ConnectionPage(WsConnectionManager wsConnectionManager)
    {
        _wsConnectionManager = wsConnectionManager;
        InitializeComponent();

        _timer = new(1000);
        _timer.Elapsed += (_, _) => Invoke(UpadteInfo);
        _timer.Start();

        SentCountDynamicLabel.DataBindings.Add(
            nameof(SentCountDynamicLabel.Text),
            _wsConnectionManager,
            nameof(_wsConnectionManager.Sent)
        );
        RecvCountDynamicLabel.DataBindings.Add(
            nameof(RecvCountDynamicLabel.Text),
            _wsConnectionManager,
            nameof(_wsConnectionManager.Received)
        );
        _wsConnectionManager.PropertyChanged += (_, e) =>
        {
            if (
                e.PropertyName == nameof(_wsConnectionManager.Active)
                || e.PropertyName == nameof(_wsConnectionManager.ConnectedAt)
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
            _wsConnectionManager.Start();
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
            _wsConnectionManager.Stop();
        }
        catch (Exception ex)
        {
            MessageBoxHelper.ShowWarningMsgBox($"关闭失败：\r\n{ex.Message}");
        }
    }

    private void UpadteInfo()
    {
        StatusDynamicLabel.Text = _wsConnectionManager.Active ? "开启" : "关闭";
        TimeDynamicLabel.Text = _wsConnectionManager.Active
            ? (DateTime.Now - _wsConnectionManager.ConnectedAt).ToCommonString()
            : "-";
    }
}
