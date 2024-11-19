using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;
using Serein.Core.Utils.Extensions;
using Serein.Lite.Utils;

namespace Serein.Lite.Ui.Servers;

public partial class Panel : UserControl
{
    private readonly Server _server;
    private readonly MainForm _mainForm;
    private readonly System.Timers.Timer _timer;
    private readonly object _lock = new();
    private readonly Lazy<PluginManagerForm> _pluginManagerForm;

    public Panel(Server server, MainForm mainForm)
    {
        InitializeComponent();
        Dock = DockStyle.Fill;

        _timer = new(1000);
        _timer.Elapsed += (_, _) => Invoke(UpdateInfoLabels);

        _server = server;
        _mainForm = mainForm;
        _server.ServerOutput += OnServerOutput;
        _server.ServerStatusChanged += (_, _) =>
        {
            Invoke(UpdateInfoLabels);

            if (_server.Status)
            {
                Invoke(ConsoleBrowser.ClearLines);
                _timer.Start();
            }
            else
            {
                _timer.Stop();
            }
        };

        _pluginManagerForm = new(() => new(_server));
    }

    private void OnServerOutput(object? sender, ServerOutputEventArgs e)
    {
        Invoke(() =>
        {
            switch (e.OutputType)
            {
                case ServerOutputType.Raw:
                    lock (_lock)
                    {
                        ConsoleBrowser.AppendHtmlLine(
                        LogColorizer.ColorLine(e.Data, _server.Configuration.OutputStyle)
                    );
                    }
                    break;

                case ServerOutputType.InputCommand:
                    if (_server.Configuration.OutputCommandUserInput)
                    {
                        lock (_lock)
                        {
                            ConsoleBrowser.AppendHtmlLine($">{LogColorizer.EscapeLog(e.Data)}");
                        }
                    }
                    break;

                case ServerOutputType.Information:
                    lock (_lock)
                    {
                        ConsoleBrowser.AppendNotice(e.Data);
                    }
                    break;

                case ServerOutputType.Error:
                    lock (_lock)
                    {
                        ConsoleBrowser.AppendError(e.Data);
                    }
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
            MessageBoxHelper.ShowWarningMsgBox($"启动服务器失败\r\n原因：{ex.Message}");
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
            MessageBoxHelper.ShowWarningMsgBox($"停止服务器失败\r\n原因：{ex.Message}");
        }
    }

    private void RestartButton_Click(object sender, EventArgs e)
    {
        try
        {
            _server.RequestRestart();
        }
        catch (Exception ex)
        {
            MessageBoxHelper.ShowWarningMsgBox($"重启服务器失败\r\n原因：{ex.Message}");
        }
    }

    private void TerminateButton_Click(object sender, EventArgs e)
    {
        try
        {
            _server.Terminate();
        }
        catch (Exception ex)
        {
            MessageBoxHelper.ShowWarningMsgBox($"强制结束服务器失败\r\n原因：{ex.Message}");
        }
    }

    private void EnterButton_Click(object sender, EventArgs e)
    {
        EnterCommand();
    }

    private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.Enter:
                e.Handled = true;
                EnterCommand();
                break;

            case Keys.Up:
                if (_server.CommandHistoryIndex > 0)
                {
                    _server.CommandHistoryIndex--;
                }

                if (
                    _server.CommandHistoryIndex >= 0
                    && _server.CommandHistoryIndex < _server.CommandHistory.Count
                )
                {
                    InputTextBox.Text = _server.CommandHistory[_server.CommandHistoryIndex];
                }

                e.Handled = true;
                InputTextBox.SelectionStart = InputTextBox.Text.Length;
                break;

            case Keys.Down:
                if (_server.CommandHistoryIndex < _server.CommandHistory.Count)
                {
                    _server.CommandHistoryIndex++;
                }

                if (
                    _server.CommandHistoryIndex >= 0
                    && _server.CommandHistoryIndex < _server.CommandHistory.Count
                )
                {
                    InputTextBox.Text = _server.CommandHistory[_server.CommandHistoryIndex];
                }
                else if (
                    _server.CommandHistoryIndex == _server.CommandHistory.Count
                    && _server.CommandHistory.Count != 0
                )
                {
                    InputTextBox.Text = string.Empty;
                }

                e.Handled = true;
                InputTextBox.SelectionStart = InputTextBox.Text.Length;
                break;
        }
    }

    private void EnterCommand()
    {
        if (_server.Status)
        {
            _server.Input(InputTextBox.Text, fromUser: true);
            InputTextBox.Text = string.Empty;
        }
    }

    private void UpdateInfoLabels()
    {
        StatusDynamicLabel.Text = _server.Status ? "运行中" : "未启动";
        VersionDynamicLabel.Text =
            _server.Status ? _server.Info.Stat?.Version ?? "-" : "-";
        PlayerCountDynamicLabel.Text =
            _server.Status
                ? $"{_server.Info.Stat?.CurrentPlayers}/{_server.Info.Stat?.MaximumPlayers}"
                : "-";
        RunTimeDynamicLabel.Text =
            _server.Status && _server.Info.StartTime is not null
                ? (DateTime.Now - _server.Info.StartTime).ToCommonString()
                : "-";

        CPUPercentDynamicLabel.Text =
            _server.Status
                ? _server.Info.CPUUsage.ToString("N2") + "%"
                : "-";
    }

    protected override void OnLoad(EventArgs e)
    {
        _timer.Start();
    }

    private void StartPluginManagerButton_Click(object sender, EventArgs e)
    {
        _server.PluginManager.Update();
        _pluginManagerForm.Value.ShowDialog();
    }

    private void OpenDirectoryButton_Click(object sender, EventArgs e)
    {
        if (File.Exists(_server.Configuration.FileName))
        {
            _server.Configuration.FileName.OpenInExplorer();
        }
        else
        {
            MessageBoxHelper.ShowWarningMsgBox("启动文件不存在，无法打开其所在文件夹");
        }
    }

    private void Panel_DragEnter(object sender, DragEventArgs e)
    {
        e.Effect =
            e.Data?.GetDataPresent(DataFormats.FileDrop) == true
                ? DragDropEffects.Copy
                : DragDropEffects.None;
    }

    private void Panel_DragDrop(object sender, DragEventArgs e)
    {
        _mainForm.FocusWindow();

        var files = e.Data?.GetData(DataFormats.FileDrop) as string[];

        if (files?.Length > 0)
        {
            var acceptable = files.Where(
                (f) => ServerPluginManager.AcceptableExtensions.Contains(Path.GetExtension(f))
            );
            if (
                acceptable.Any()
                && MessageBoxHelper.ShowQuestionMsgBox(
                    "是否将以下文件作为插件导入到服务器的插件文件夹？\r\n"
                        + string.Join("\r\n", files.Select((f) => Path.GetFileName(f)))
                )
            )
            {
                try
                {
                    _server.PluginManager.Add(acceptable.ToArray());
                }
                catch (Exception ex)
                {
                    MessageBoxHelper.ShowWarningMsgBox("导入失败：\r\n" + ex.Message);
                }
            }
        }
    }
}
