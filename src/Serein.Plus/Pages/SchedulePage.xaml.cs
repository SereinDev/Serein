using System;
using System.Collections.ObjectModel;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Commands;
using Serein.Core.Services.Data;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Pages;

public partial class SchedulePage : Page
{
    private readonly IHost _host;
    private IServiceProvider Services => _host.Services;
    private ScheduleProvider ScheduleProvider => Services.GetRequiredService<ScheduleProvider>();
    public ObservableCollection<Schedule> Schedules => ScheduleProvider.Value;

    public SchedulePage(IHost host)
    {
        _host = host;
        InitializeComponent();
        ScheduleDataGrid.ItemsSource = Schedules;
        Schedules.CollectionChanged += UpdateDetails;
    }

    private void OnLayoutUpdated(object sender, EventArgs e)
    {
        ScheduleProvider.SaveAsyncWithDebounce();
        UpdateDetails(sender, e);
    }

    private void UpdateDetails(object? sender, EventArgs e)
    {
        Details.Text =
            ScheduleDataGrid.SelectedItems.Count > 1
                ? $"共{Schedules.Count}项，已选择{ScheduleDataGrid.SelectedItems.Count}项"
                : ScheduleDataGrid.SelectedIndex >= 0
                    ? $"共{Schedules.Count}项，已选择第{ScheduleDataGrid.SelectedIndex + 1}项"
                    : $"共{Schedules.Count}项";
        ;
    }
}
