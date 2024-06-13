using System;
using System.Windows.Forms;

using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;
using Serein.Core.Utils;

namespace Serein.Lite.Ui.Servers;

public partial class ConfigurationEditor : Form
{
    private readonly Configuration _configuration;

    public string Id => IdTextBox.Text;


    public ConfigurationEditor(Configuration configuration, string? id = null)
    {
        InitializeComponent();

        if (!string.IsNullOrEmpty(id))
        {
            IdTextBox.ReadOnly = true;
            IdTextBox.Text = id;
        }

        _configuration = configuration;
    }

    private void FileName_DoubleClick(object sender, EventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Title = "选择启动文件"
        };
        openFileDialog.ShowDialog();

        if (!string.IsNullOrEmpty(openFileDialog.FileName))
            FileNameTextBox.Text = openFileDialog.FileName;
    }

    private void ConfirmButton_Click(object sender, EventArgs e)
    {
        try
        {
            EncodingMap.EncodingType input, output;
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
            _configuration.LineTerminator = LineTerminatorTextBox.Text;
            _configuration.StartWhenSettingUp = StartWhenSettingUpCheckBox.Checked;
            _configuration.OutputCommandUserInput = OutputCommandUserInputCheckBox.Checked;
            _configuration.OutputStyle = outputStyle;
            _configuration.StopCommands = StopCommandsTextBox.Text.Replace("\r", null).Split('\n');
            _configuration.InputEncoding = input;
            _configuration.OutputEncoding = output;

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Serein.Lite", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
