using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;
using Serein.Core.Utils;
using Serein.Lite.Utils;

namespace Serein.Lite.Ui.Servers;

public partial class ConfigurationEditor : Form
{
    private readonly ServerManager _serverManager;
    private readonly Configuration _configuration;

    public string Id => _idTextBox.Text;

    public ConfigurationEditor(
        ServerManager serverManager,
        Configuration configuration,
        string? id = null
    )
    {
        InitializeComponent();

        if (!string.IsNullOrEmpty(id))
        {
            _idTextBox.ReadOnly = true;
            _idTextBox.Text = id;
        }
        _serverManager = serverManager;
        _configuration = configuration;

        SyncData();
    }

    private void SyncData()
    {
        _nameTextBox.Text = _configuration.Name;
        _fileNameTextBox.Text = _configuration.FileName;
        _argumentTextBox.Text = _configuration.Argument;
        _autoRestartCheckBox.Checked = _configuration.AutoRestart;
        _saveLogCheckBox.Checked = _configuration.SaveLog;
        _autoStopWhenCrashingCheckBox.Checked = _configuration.AutoStopWhenCrashing;
        _portNumericUpDown.Value = _configuration.PortIPv4;
        _lineTerminatorTextBox.Text = _configuration
            .LineTerminator.Replace("\r", "\\r")
            .Replace("\n", "\\n");
        _startWhenSettingUpCheckBox.Checked = _configuration.StartWhenSettingUp;
        _outputCommandUserInputCheckBox.Checked = _configuration.OutputCommandUserInput;
        _stopCommandsTextBox.Text = string.Join("\r\n", _configuration.StopCommands);
        _outputStyleComboBox.SelectedIndex = (int)_configuration.OutputStyle;
        _inputEncondingComboBox.SelectedIndex = (int)_configuration.InputEncoding;
        _outputEncondingComboBox.SelectedIndex = (int)_configuration.OutputEncoding;
        _useUnicodeCharsCheckBox.Checked = _configuration.UseUnicodeChars;
        _usePtyCheckBox.Checked = _forceWinPtyCheckBox.Enabled = _configuration.Pty.IsEnabled;
        _forceWinPtyCheckBox.Checked = _configuration.Pty.ForceWinPty;
    }

    private void ConfirmButton_Click(object sender, EventArgs e)
    {
        try
        {
            EncodingMap.EncodingType input;
            EncodingMap.EncodingType output;
            OutputStyle outputStyle;

            ServerManager.ValidateId(_idTextBox.Text);
            if (!_idTextBox.ReadOnly && _serverManager.Servers.ContainsKey(Id))
            {
                throw new InvalidOperationException("此Id已被占用");
            }

            if (_inputEncondingComboBox.SelectedIndex < 0)
            {
                _inputEncondingComboBox.SelectedIndex = 0;
            }

            if (_outputEncondingComboBox.SelectedIndex < 0)
            {
                _outputEncondingComboBox.SelectedIndex = 0;
            }

            if (_outputStyleComboBox.SelectedIndex < 0)
            {
                _outputStyleComboBox.SelectedIndex = 0;
            }

            input = (EncodingMap.EncodingType)_inputEncondingComboBox.SelectedIndex;
            output = (EncodingMap.EncodingType)_outputEncondingComboBox.SelectedIndex;
            outputStyle = (OutputStyle)_outputStyleComboBox.SelectedIndex;

            _configuration.Name =
                string.IsNullOrEmpty(_nameTextBox.Text)
                || string.IsNullOrWhiteSpace(_nameTextBox.Text)
                    ? "未命名"
                    : _nameTextBox.Text;
            _configuration.Pty.IsEnabled = _usePtyCheckBox.Checked;
            _configuration.Pty.ForceWinPty = _forceWinPtyCheckBox.Checked;
            _configuration.FileName = _fileNameTextBox.Text;
            _configuration.Argument = _argumentTextBox.Text;
            _configuration.AutoRestart = _autoRestartCheckBox.Checked;
            _configuration.SaveLog = _saveLogCheckBox.Checked;
            _configuration.AutoStopWhenCrashing = _autoStopWhenCrashingCheckBox.Checked;
            _configuration.PortIPv4 = (ushort)_portNumericUpDown.Value;
            _configuration.LineTerminator = _lineTerminatorTextBox
                .Text.Replace("\\r", "\r")
                .Replace("\\n", "\n");
            _configuration.StartWhenSettingUp = _startWhenSettingUpCheckBox.Checked;
            _configuration.OutputCommandUserInput = _outputCommandUserInputCheckBox.Checked;
            _configuration.UseUnicodeChars = _useUnicodeCharsCheckBox.Checked;
            _configuration.OutputStyle = outputStyle;
            _configuration.StopCommands = _stopCommandsTextBox.Text.Replace("\r", null).Split('\n');
            _configuration.InputEncoding = input;
            _configuration.OutputEncoding = output;

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBoxHelper.ShowWarningMsgBox(ex.Message);
        }
    }

    private void IdTextBox_Enter(object sender, EventArgs e)
    {
        _errorProvider.Clear();
    }

    private void IdTextBox_Validating(object sender, CancelEventArgs e)
    {
        if (string.IsNullOrEmpty(_idTextBox.Text))
        {
            _errorProvider.SetError(_idTextBox, "Id不能为空");
        }
        else if (!IdRegex().IsMatch(_idTextBox.Text))
        {
            _errorProvider.SetError(_idTextBox, "Id只能由字母、数字和下划线组成");
        }
        else if (_idTextBox.Text.Length <= 2)
        {
            _errorProvider.SetError(_idTextBox, "Id长度太短");
        }
    }

    [GeneratedRegex(@"^\w+$")]
    private static partial Regex IdRegex();

    private void OpenFileButton_Click(object sender, EventArgs e)
    {
        var openFileDialog = new OpenFileDialog { Title = "选择启动文件" };

        if (
            openFileDialog.ShowDialog() == DialogResult.OK
            && !string.IsNullOrEmpty(openFileDialog.FileName)
        )
        {
            _fileNameTextBox.Text = openFileDialog.FileName;
        }
    }

    private void UsePtyCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        _forceWinPtyCheckBox.Enabled = _usePtyCheckBox.Checked;
    }
}
