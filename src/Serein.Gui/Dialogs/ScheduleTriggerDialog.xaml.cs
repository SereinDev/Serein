using System;
using System.Collections.Generic;
using iNKORE.UI.WPF.Modern.Controls;
using NCrontab;
using Serein.Core.Models.Automations.Triggers;

namespace Serein.Gui.Dialogs;

public partial class ScheduleTriggerDialog : ContentDialog
{
    private readonly ScheduleTrigger _trigger;

    public ScheduleTriggerDialog(ScheduleTrigger trigger, bool isEdit)
    {
        _trigger = trigger;
        Title = isEdit ? "编辑定时触发器" : "新建定时触发器";
        DataContext = trigger;
        InitializeComponent();
        RefreshPreview(trigger.CronExpression);
    }

    private void CronExpressionTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        RefreshPreview(CronExpressionTextBox.Text);
    }

    private void RefreshPreview(string? expression)
    {
        var cronExpression = expression ?? string.Empty;
        if (string.IsNullOrWhiteSpace(cronExpression))
        {
            ErrorInfoBar.IsOpen = true;
            ErrorInfoBar.Message = "请输入Cron表达式。";
            OccurrencesListBox.ItemsSource = null;
            IsPrimaryButtonEnabled = false;
            return;
        }

        try
        {
            var schedule = CrontabSchedule.Parse(cronExpression);
            var now = DateTime.Now;
            var occurrences = new List<string>(10);
            var cursor = now;
            for (var i = 0; i < 10; i++)
            {
                cursor = schedule.GetNextOccurrence(cursor);
                occurrences.Add(cursor.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            ErrorInfoBar.IsOpen = false;
            OccurrencesListBox.ItemsSource = occurrences;
            IsPrimaryButtonEnabled = true;
            _trigger.CronExpression = cronExpression;
        }
        catch (Exception e)
        {
            ErrorInfoBar.IsOpen = true;
            ErrorInfoBar.Message = e.Message;
            OccurrencesListBox.ItemsSource = null;
            IsPrimaryButtonEnabled = false;
        }
    }
}
