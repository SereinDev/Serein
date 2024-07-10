using System;
using System.Windows.Forms;

using Serein.Core.Models.Network.Connection.OneBot.Messages;
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
                || e.PropertyName == nameof(_wsConnectionManager.ConnectedTime)
            )
                Invoke(UpadteInfo);
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
            MessageBox.Show(
                $"开启失败：\n{ex.Message}",
                "Serein",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
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
            MessageBox.Show(
                $"关闭失败：\n{ex.Message}",
                "Serein",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }
    }

    private void UpadteInfo()
    {
        StatusDynamicLabel.Text = _wsConnectionManager.Active ? "开启" : "关闭";
        TimeDynamicLabel.Text = _wsConnectionManager.Active
            ? (DateTime.Now - _wsConnectionManager.ConnectedTime).ToCommonString()
            : "-";
    }
}
