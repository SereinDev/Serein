using System;
using System.Windows.Forms;

using Serein.Core.Services.Networks.Connection;

namespace Serein.Lite.Ui.Function;

public partial class ConnectionPage : UserControl
{
    private readonly WsConnectionManager _wsConnectionManager;

    public ConnectionPage(WsConnectionManager wsConnectionManager)
    {
        InitializeComponent();
        _wsConnectionManager = wsConnectionManager;
    }

    private void OpenButton_Click(object sender, EventArgs e)
    {
        try
        {
            _wsConnectionManager.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"连接失败：{ex.Message}",
                "Serein",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
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
                $"断开失败：{ex.Message}",
                "Serein",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }
}
