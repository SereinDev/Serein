using System;
using System.Windows.Forms;

using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;

namespace Serein.Lite.Ui.Servers;

public partial class Panel : UserControl
{
    private readonly Server _server;

    public Panel(Server server)
    {
        InitializeComponent();
        _server = server;
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
        _server.Start();
    }

    private void StopButton_Click(object sender, EventArgs e)
    {
        _server.Stop();
    }

    private void RestartButton_Click(object sender, EventArgs e)
    {

    }

    private void TerminateButton_Click(object sender, EventArgs e)
    {
        _server.Terminate();
    }

    private void EnterButton_Click(object sender, EventArgs e)
    {
        EnterCommand();
    }

    private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        EnterCommand();
    }

    private void EnterCommand()
    {
        if (_server.Status == ServerStatus.Running)
        {
            _server.Input(InputTextBox.Text, fromUser: true);
            InputTextBox.Text = string.Empty;
        }
    }
}
