using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Serein.Core.Models.Commands;
using Serein.Core.Services.Commands;

namespace Serein.Lite.Ui.Functions;

public partial class MatchEditor : Form
{
    private readonly Core.Models.Commands.Match _match;

    public MatchEditor(Core.Models.Commands.Match match)
    {
        _match = match;
        InitializeComponent();

        _regexTextBox.Text = match.RegExp;
        _fieldTypeComboBox.SelectedIndex = (int)match.FieldType;
        _requireAdminCheckBox.Enabled =
            match.FieldType is MatchFieldType.GroupMsg or MatchFieldType.PrivateMsg;
        _requireAdminCheckBox.Checked = match.RequireAdmin && _requireAdminCheckBox.Enabled;
        _commandTextBox.Text = match.Command;
        _descriptionTextBox.Text = match.Description;
        _exclusionsTextBox.Text = match.Exclusions;
    }

    private void FieldType_SelectedIndexChanged(object sender, EventArgs e)
    {
        _requireAdminCheckBox.Enabled = _fieldTypeComboBox.SelectedIndex is 3 or 4;
        if (!_requireAdminCheckBox.Enabled)
        {
            _requireAdminCheckBox.Checked = false;
        }
    }

    private void ConfirmButton_Click(object sender, EventArgs e)
    {
        _match.RegExp = _regexTextBox.Text;
        _match.FieldType = (MatchFieldType)_fieldTypeComboBox.SelectedIndex;
        _match.RequireAdmin = _requireAdminCheckBox.Checked;
        _match.Command = _commandTextBox.Text;
        _match.Description = _descriptionTextBox.Text;
        _match.Exclusions = _exclusionsTextBox.Text;

        DialogResult = DialogResult.OK;
    }

    private void RegexTextBox_Enter(object sender, EventArgs e)
    {
        _regexErrorProvider.Clear();
    }

    private void Regex_Validating(object sender, CancelEventArgs e)
    {
        _regexErrorProvider.Clear();
        if (string.IsNullOrEmpty(_regexTextBox.Text))
        {
            _regexErrorProvider.SetError(_regexTextBox, "正则内容为空");
        }
        else
        {
            try
            {
                _ = new Regex(_regexTextBox.Text);
            }
            catch (Exception ex)
            {
                _regexErrorProvider.SetError(_regexTextBox, "正则语法不正确：\r\n" + ex.Message);
            }
        }
    }

    private void CommandTextBox_Enter(object sender, EventArgs e)
    {
        _commandErrorProvider.Clear();
    }

    private void CommandTextBox_Validating(object sender, CancelEventArgs e)
    {
        _commandErrorProvider.Clear();
        if (string.IsNullOrEmpty(_commandTextBox.Text))
        {
            _commandErrorProvider.SetError(_commandTextBox, "命令内容为空");
        }
        else
        {
            try
            {
                CommandParser.Parse(CommandOrigin.Null, _commandTextBox.Text, true);
            }
            catch (Exception ex)
            {
                _commandErrorProvider.SetError(_commandTextBox, ex.Message);
            }
        }
    }
}
