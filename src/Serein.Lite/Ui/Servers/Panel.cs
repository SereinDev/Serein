using System;
using System.Windows.Forms;

using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;
using Serein.Core.Utils.Extensions;
using Serein.Lite.Utils;

namespace Serein.Lite.Ui.Servers;

public partial class Panel : UserControl
{
    private readonly Server _server;
    private readonly System.Timers.Timer _timer;
    private readonly object _lock = new();

    public Panel(Server server)
    {
        InitializeComponent();
        Dock = DockStyle.Fill;

        _timer = new(1000);
        _timer.Elapsed += (_, _) => Invoke(UpdateInfoLabels);

        _server = server;
        _server.ServerOutput += OnServerOutput;
        _server.ServerStatusChanged += (_, _) =>
        {
            Invoke(UpdateInfoLabels);

            if (_server.Status == ServerStatus.Running)
            {
                Invoke(ConsoleBrowser.ClearLines);
                _timer.Start();
            }
            else
                _timer.Stop();
        };
    }

    private void OnServerOutput(object? sender, ServerOutputEventArgs e)
    {
        Invoke(() =>
        {
            switch (e.OutputType)
            {
                case ServerOutputType.Raw:
                    if (!string.IsNullOrEmpty(e.Data))
                        lock (_lock)
                            ConsoleBrowser.AppendHtmlLine(
                                LogColorizer.ColorLine(e.Data, _server.Configuration.OutputStyle)
                            );
                    break;

                case ServerOutputType.InputCommand:
                    if (
                        _server.Configuration.OutputCommandUserInput
                        && !string.IsNullOrEmpty(e.Data)
                    )
                        lock (_lock)
                            ConsoleBrowser.AppendHtmlLine($">{LogColorizer.EscapeLog(e.Data)}");
                    break;

                case ServerOutputType.Information:
                    if (!string.IsNullOrEmpty(e.Data))
                        lock (_lock)
                            ConsoleBrowser.AppendNotice(e.Data);
                    break;

                default:
                    throw new NotSupportedException();
            }
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
                MessageBoxIcon.Warning
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
                MessageBoxIcon.Warning
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
                MessageBoxIcon.Warning
            );
        }
    }

    private void EnterButton_Click(object sender, EventArgs e)
    {
        EnterCommand();
    }

    private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        e.Handled = true;
        if (e.KeyCode == Keys.Enter)
        {
            EnterCommand();
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

    private void UpdateInfoLabels()
    {
        StatusDynamicLabel.Text = _server.Status == ServerStatus.Running ? "运行中" : "未启动";
        VersionDynamicLabel.Text =
            _server.Status == ServerStatus.Running ? _server.ServerInfo.Stat?.Version ?? "-" : "-";
        PlayerCountDynamicLabel.Text =
            _server.Status == ServerStatus.Running
                ? $"{_server.ServerInfo.Stat?.CurrentPlayers}/{_server.ServerInfo.Stat?.MaximumPlayers}"
                : "-";
        RunTimeDynamicLabel.Text =
            _server.Status == ServerStatus.Running && _server.ServerInfo.StartTime is not null
                ? (DateTime.Now - _server.ServerInfo.StartTime).ToCommonString()
                : "-";

        CPUPercentDynamicLabel.Text =
            _server.Status == ServerStatus.Running
                ? _server.ServerInfo.CPUUsage.ToString("N2") + "%"
                : "-";
    }

    protected override void OnLoad(EventArgs e)
    {
        _timer.Start();
    }
}
