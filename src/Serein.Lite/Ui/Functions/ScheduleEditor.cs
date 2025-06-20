using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using NCrontab;
using Serein.Core.Models.Commands;
using Serein.Core.Services.Commands;

namespace Serein.Lite.Ui.Functions;

public partial class ScheduleEditor : Form
{
    private readonly Schedule _schedule;
    private DateTime[]? _nextOccurrences;

    public ScheduleEditor(Schedule schedule)
    {
        _schedule = schedule;
        _nextOccurrences = [];

        InitializeComponent();

        _cronTextBox.Text = _schedule.Expression;
        _commandTextBox.Text = _schedule.Command;
        __descriptionTextBox.Text = _schedule.Description;
        _enableCheckBox.Checked = _schedule.IsEnabled;
    }

    private void CronTextBox_TextChanged(object sender, EventArgs e)
    {
        _nextOccurrences = null;
        try
        {
            _nextOccurrences = CrontabSchedule
                .Parse(_cronTextBox.Text)
                .GetNextOccurrences(DateTime.Now, DateTime.Now.AddYears(1))
                .ToArray();
        }
        catch { }

        _toolStripStatusLabel.Text =
            _nextOccurrences?.Length > 0
                ? $"预计下一次执行时间：{_nextOccurrences.First():f}"
                : "预计下一次执行时间：-";
        _toolStripStatusLabel.ToolTipText =
            _nextOccurrences?.Length > 0
                ? string.Join("\r\n", _nextOccurrences.Select(d => d.ToString("f")))
                : null;
    }

    private void CronTextBox_Leave(object sender, EventArgs e)
    {
        _cronErrorProvider.Clear();
    }

    private void CronTextBox_Validating(object sender, CancelEventArgs e)
    {
        try
        {
            _nextOccurrences = CrontabSchedule
                .Parse(_cronTextBox.Text)
                .GetNextOccurrences(DateTime.Now, DateTime.Now.AddYears(1))
                .ToArray();
        }
        catch (Exception ex)
        {
            _cronErrorProvider.SetError(_cronTextBox, $"Cron语法不正确：\r\n{ex.Message}");
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

    private void ConfirmButton_Click(object sender, EventArgs e)
    {
        _schedule.Expression = _cronTextBox.Text;
        _schedule.Command = _commandTextBox.Text;
        _schedule.Description = __descriptionTextBox.Text;
        _schedule.IsEnabled = _enableCheckBox.Checked;

        DialogResult = DialogResult.OK;
        Close();
    }
}
