using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using Serein.Core.Models.Commands;
using Serein.Core.Services;

namespace Serein.Lite.Ui.Function;

public partial class MatchEditor : Form
{
    private readonly Core.Models.Commands.Match _match;

    public MatchEditor(Core.Models.Commands.Match match)
    {
        _match = match;
        InitializeComponent();

        RegexTextBox.Text = match.RegExp;
        FieldTypeComboBox.SelectedIndex = (int)match.FieldType;
        RequireAdminCheckBox.Enabled =
            match.FieldType is MatchFieldType.GroupMsg or MatchFieldType.PrivateMsg;
        RequireAdminCheckBox.Checked = match.RequireAdmin && RequireAdminCheckBox.Enabled;
        CommandTextBox.Text = match.Command;
        DescriptionTextBox.Text = match.Description;
        RestrictionsTextBox.Text = match.Restrictions;
    }

    private void FieldType_SelectedIndexChanged(object sender, EventArgs e)
    {
        RequireAdminCheckBox.Enabled = FieldTypeComboBox.SelectedIndex is 3 or 4;
        if (!RequireAdminCheckBox.Enabled)
            RequireAdminCheckBox.Checked = false;
    }

    private void ConfirmButton_Click(object sender, EventArgs e)
    {
        _match.RegExp = RegexTextBox.Text;
        _match.FieldType = (MatchFieldType)FieldTypeComboBox.SelectedIndex;
        _match.RequireAdmin = RequireAdminCheckBox.Checked;
        _match.Command = CommandTextBox.Text;
        _match.Description = DescriptionTextBox.Text;
        _match.Restrictions = RestrictionsTextBox.Text;

        DialogResult = DialogResult.OK;
    }

    private void RegexTextBox_Enter(object sender, EventArgs e)
    {
        RegexErrorProvider.Clear();
    }

    private void Regex_Validating(object sender, CancelEventArgs e)
    {
        RegexErrorProvider.Clear();
        if (string.IsNullOrEmpty(RegexTextBox.Text))
            RegexErrorProvider.SetError(RegexTextBox, "正则内容为空");
        else
            try
            {
                _ = new Regex(RegexTextBox.Text);
            }
            catch (Exception ex)
            {
                RegexErrorProvider.SetError(RegexTextBox, "正则语法不正确：\r\n" + ex.Message);
            }
    }

    private void CommandTextBox_Enter(object sender, EventArgs e)
    {
        CommandErrorProvider.Clear();
    }

    private void CommandTextBox_Validating(object sender, CancelEventArgs e)
    {
        CommandErrorProvider.Clear();
        if (string.IsNullOrEmpty(CommandTextBox.Text))
            CommandErrorProvider.SetError(CommandTextBox, "命令内容为空");
        else
            try
            {
                CommandParser.Parse(CommandOrigin.Null, CommandTextBox.Text, true);
            }
            catch (Exception ex)
            {
                CommandErrorProvider.SetError(CommandTextBox, ex.Message);
            }
    }
}
