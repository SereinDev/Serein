﻿using System;
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

        NameTextBox.Text = _configuration.Name;
        FileNameTextBox.Text = _configuration.FileName;
        ArgumentTextBox.Text = _configuration.Argument;
        AutoRestartCheckBox.Checked = _configuration.AutoRestart;
        SaveLogCheckBox.Checked = _configuration.SaveLog;
        AutoStopWhenCrashingCheckBox.Checked = _configuration.AutoStopWhenCrashing;
        PortNumericUpDown.Value = _configuration.IPv4Port;
        LineTerminatorTextBox.Text = _configuration.LineTerminator
            .Replace("\r", "\\r")
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

            _configuration.Name = NameTextBox.Text;
            _configuration.FileName = FileNameTextBox.Text;
            _configuration.Argument = ArgumentTextBox.Text;
            _configuration.AutoRestart = AutoRestartCheckBox.Checked;
            _configuration.SaveLog = SaveLogCheckBox.Checked;
            _configuration.AutoStopWhenCrashing = AutoStopWhenCrashingCheckBox.Checked;
            _configuration.IPv4Port = (short)PortNumericUpDown.Value;
            _configuration.LineTerminator = LineTerminatorTextBox.Text
                .Replace("\\r", "\r")
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
}
