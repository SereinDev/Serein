using System;
using System.Windows.Forms;

using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;
using Serein.Lite.Extensions;

namespace Serein.Lite.Ui.Servers;

public partial class Panel : UserControl
{
    private readonly Server _server;
    private readonly object _lock = new();

    public Panel(Server server)
    {
        InitializeComponent();
        _server = server;
        Dock = DockStyle.Fill;

        _server.ServerOutput += OnServerOutput;
    }

    private void OnServerOutput(object? sender, ServerOutputEventArgs e)
    {
        Invoke(() =>
        {
            switch (e.OutputType)
            {
                case ServerOutputType.Raw:
                    lock (_lock)
                        ConsoleRichTextBox.AppendText(e.Data + "\r\n");
                    break;

                case ServerOutputType.InputCommand:
                    if (_server.Configuration.OutputCommandUserInput)
                        lock (_lock)
                            ConsoleRichTextBox.AppendText($">{e.Data}" + "\r\n");
                    break;

                case ServerOutputType.Information:
                    lock (_lock)
                    {
                        ConsoleRichTextBox.AppendTextWithColor("[Serein]", MainForm.PrimaryColor);
                        ConsoleRichTextBox.AppendText(e.Data + "\r\n");
                    }
                    break;

                case ServerOutputType.Clear:
                    Invoke(ConsoleRichTextBox.Clear);
                    break;
            }
            ConsoleRichTextBox.ScrollToEnd();
        });

    }

    private void StartButton_Click(object sender, EventArgs e)
    {
        try
        {
            _server.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"启动服务器失败\r\n原因：{ex.Message}",
                "Serein",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }

    private void StopButton_Click(object sender, EventArgs e)
    {
        try
        {
            _server.Stop();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"停止服务器失败\r\n原因：{ex.Message}",
                "Serein",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }

    private void RestartButton_Click(object sender, EventArgs e) { }

    private void TerminateButton_Click(object sender, EventArgs e)
    {
        try
        {
            _server.Terminate();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"强制结束服务器失败\r\n原因：{ex.Message}",
                "Serein",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }

    private void EnterButton_Click(object sender, EventArgs e)
    {
        EnterCommand();
    }

    private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            EnterCommand();
            e.Handled = true;
        }
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
