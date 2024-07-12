﻿using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;
using Serein.Core.Utils;

namespace Serein.Lite.Ui.Servers;

public partial class ConfigurationEditor : Form
{
    private readonly ServerManager _serverManager;
    private readonly Configuration _configuration;

    public string Id => IdTextBox.Text;

    public ConfigurationEditor(
        ServerManager serverManager,
        Configuration configuration,
        string? id = null
    )
    {
        InitializeComponent();

        if (!string.IsNullOrEmpty(id))
        {
            IdTextBox.ReadOnly = true;
            IdTextBox.Text = id;
        }
        _serverManager = serverManager;
        _configuration = configuration;

        SyncData();
        SetToolTips();
    }

    private void SetToolTips()
    {
        ToolTip.SetToolTip(
            IdTextBox,
            "用于区分服务器（一经填写无法修改）\r\n" + "· 长度大于或等于3\r\n" + "· 只由数字、字母和下划线组成"
        );
        ToolTip.SetToolTip(NameTextBox, "用于标识服务器，便于管理");
        ToolTip.SetToolTip(FileNameTextBox, "启动进程的文件，通常为可执行文件或批处理文件\r\n你可以双击");
    }

    private void SyncData()
    {
        NameTextBox.Text = _configuration.Name;
        FileNameTextBox.Text = _configuration.FileName;
        ArgumentTextBox.Text = _configuration.Argument;
        AutoRestartCheckBox.Checked = _configuration.AutoRestart;
        SaveLogCheckBox.Checked = _configuration.SaveLog;
        AutoStopWhenCrashingCheckBox.Checked = _configuration.AutoStopWhenCrashing;
        PortNumericUpDown.Value = _configuration.PortIPv4;
        LineTerminatorTextBox.Text = _configuration
            .LineTerminator.Replace("\r", "\\r")
            .Replace("\n", "\\n");
        StartWhenSettingUpCheckBox.Checked = _configuration.StartWhenSettingUp;
        OutputCommandUserInputCheckBox.Checked = _configuration.OutputCommandUserInput;
        StopCommandsTextBox.Text = string.Join("\r\n", _configuration.StopCommands);
        OutputStyleComboBox.SelectedIndex = (int)_configuration.OutputStyle;
        InputEncondingComboBox.SelectedIndex = (int)_configuration.InputEncoding;
        OutputEncondingComboBox.SelectedIndex = (int)_configuration.OutputEncoding;
    }

    private void FileName_DoubleClick(object sender, EventArgs e)
    {
        var openFileDialog = new OpenFileDialog { Title = "选择启动文件" };
        openFileDialog.ShowDialog();

        if (!string.IsNullOrEmpty(openFileDialog.FileName))
            FileNameTextBox.Text = openFileDialog.FileName;
    }

    private void ConfirmButton_Click(object sender, EventArgs e)
    {
        try
        {
            EncodingMap.EncodingType input;
            EncodingMap.EncodingType output;
            OutputStyle outputStyle;

            ServerManager.CheckId(IdTextBox.Text);

            if (InputEncondingComboBox.SelectedIndex < 0)
                InputEncondingComboBox.SelectedIndex = 0;

            if (OutputEncondingComboBox.SelectedIndex < 0)
                OutputEncondingComboBox.SelectedIndex = 0;

            if (OutputStyleComboBox.SelectedIndex < 0)
                OutputStyleComboBox.SelectedIndex = 0;

            input = (EncodingMap.EncodingType)InputEncondingComboBox.SelectedIndex;
            output = (EncodingMap.EncodingType)OutputEncondingComboBox.SelectedIndex;
            outputStyle = (OutputStyle)OutputStyleComboBox.SelectedIndex;

            _configuration.Name = string.IsNullOrEmpty(NameTextBox.Text) || string.IsNullOrWhiteSpace(NameTextBox.Text) ? "未命名" : NameTextBox.Text;
            _configuration.FileName = FileNameTextBox.Text;
            _configuration.Argument = ArgumentTextBox.Text;
            _configuration.AutoRestart = AutoRestartCheckBox.Checked;
            _configuration.SaveLog = SaveLogCheckBox.Checked;
            _configuration.AutoStopWhenCrashing = AutoStopWhenCrashingCheckBox.Checked;
            _configuration.PortIPv4 = (short)PortNumericUpDown.Value;
            _configuration.LineTerminator = LineTerminatorTextBox
                .Text.Replace("\\r", "\r")
                .Replace("\n", "\\n");
            _configuration.StartWhenSettingUp = StartWhenSettingUpCheckBox.Checked;
            _configuration.OutputCommandUserInput = OutputCommandUserInputCheckBox.Checked;
            _configuration.OutputStyle = outputStyle;
            _configuration.StopCommands = StopCommandsTextBox.Text.Replace("\r", null).Split('\n');
            _configuration.InputEncoding = input;
            _configuration.OutputEncoding = output;

            if (!IdTextBox.ReadOnly && _serverManager.Servers.ContainsKey(Id))
                throw new InvalidOperationException("此Id已被占用");

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Serein.Lite",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }
    }

    private void IdTextBox_Enter(object sender, EventArgs e)
    {
        ErrorProvider.Clear();
    }

    private void IdTextBox_Validating(object sender, CancelEventArgs e)
    {
        if (string.IsNullOrEmpty(IdTextBox.Text))
            ErrorProvider.SetError(IdTextBox, "Id不能为空");
        else if (!IdRegex().IsMatch(IdTextBox.Text))
            ErrorProvider.SetError(IdTextBox, "Id只能由字母、数字和下划线组成");
        else if (IdTextBox.Text.Length <= 2)
            ErrorProvider.SetError(IdTextBox, "Id长度太短");
    }

    [GeneratedRegex(@"^\w+$")]
    private static partial Regex IdRegex();
}
