using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Serein.Core.Models.Commands;

namespace Serein.Plus.Windows;

public partial class ScheduleEditor : Window
{
    private readonly Schedule _schedule;

    public ScheduleEditor(Schedule schedule)
    {
        _schedule = schedule;
        InitializeComponent();
        DataContext = _schedule;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        PreviewTextBlock.ToolTip = _schedule.Cron is not null
            ? string.Join(
                "\r\n",
                _schedule
                    .Cron.GetNextOccurrences(DateTime.Now, DateTime.Now.AddYears(1))
                    .Take(10)
                    .Select((datetime) => datetime.ToString("f"))
            )
            : string.Empty;
    }
}
