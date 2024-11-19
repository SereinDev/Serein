using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using NCrontab;

using Serein.Core.Models.Commands;
using Serein.Core.Services.Commands;

namespace Serein.Lite.Ui.Function;

public partial class ScheduleEditor : Form
{
    private readonly Schedule _schedule;
    private DateTime[]? _nextOccurrences;

    public ScheduleEditor(Schedule schedule)
    {
        _schedule = schedule;
        _nextOccurrences = [];

        InitializeComponent();

        CronTextBox.Text = _schedule.Expression;
        CommandTextBox.Text = _schedule.Command;
        DescriptionTextBox.Text = _schedule.Description;
        EnableCheckBox.Checked = _schedule.IsEnabled;
    }

    private void CronTextBox_TextChanged(object sender, EventArgs e)
    {
        _nextOccurrences = null;
        try
        {
            _nextOccurrences = CrontabSchedule
                .Parse(CronTextBox.Text)
                .GetNextOccurrences(DateTime.Now, DateTime.Now.AddYears(1))
                .ToArray();
        }
        catch { }

        ToolStripStatusLabel.Text =
            _nextOccurrences?.Length > 0
                ? $"预计下一次执行时间：{_nextOccurrences.First():f}"
                : "预计下一次执行时间：-";
        ToolStripStatusLabel.ToolTipText =
            _nextOccurrences?.Length > 0
                ? string.Join("\r\n", _nextOccurrences.Select(d => d.ToString("f")))
                : null;
    }

    private void CronTextBox_Leave(object sender, EventArgs e)
    {
        CronErrorProvider.Clear();
    }

    private void CronTextBox_Validating(object sender, CancelEventArgs e)
    {
        try
        {
            _nextOccurrences = CrontabSchedule
                .Parse(CronTextBox.Text)
                .GetNextOccurrences(DateTime.Now, DateTime.Now.AddYears(1))
                .ToArray();
        }
        catch (Exception ex)
        {
            CronErrorProvider.SetError(CronTextBox, $"Cron语法不正确：\r\n{ex.Message}");
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
        {
            CommandErrorProvider.SetError(CommandTextBox, "命令内容为空");
        }
        else
        {
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

    private void ConfirmButton_Click(object sender, EventArgs e)
    {
        _schedule.Expression = CronTextBox.Text;
        _schedule.Command = CommandTextBox.Text;
        _schedule.Description = DescriptionTextBox.Text;
        _schedule.IsEnabled = EnableCheckBox.Checked;

        DialogResult = DialogResult.OK;
        Close();
    }
}
