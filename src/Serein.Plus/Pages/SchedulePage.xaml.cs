using System;

using Serein.Core.Services.Data;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Pages;

public partial class SchedulePage : Page
{
    private readonly ScheduleProvider _scheduleProvider;

    public SchedulePage(ScheduleProvider scheduleProvider)
    {
        _scheduleProvider = scheduleProvider;
        InitializeComponent();
        ScheduleDataGrid.ItemsSource = _scheduleProvider.Value;
        _scheduleProvider.Value.CollectionChanged += UpdateDetails;
    }

    private void UpdateDetails(object? sender, EventArgs e)
    {
        Details.Text =
            ScheduleDataGrid.SelectedItems.Count > 1
                ? $"共{_scheduleProvider.Value.Count}项，已选择{ScheduleDataGrid.SelectedItems.Count}项"
                : ScheduleDataGrid.SelectedIndex >= 0
                    ? $"共{_scheduleProvider.Value.Count}项，已选择第{ScheduleDataGrid.SelectedIndex + 1}项"
                    : $"共{_scheduleProvider.Value.Count}项";
    }
}
