using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Force.DeepCloner;

using Serein.Core.Models.Commands;
using Serein.Core.Services.Data;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;
using Serein.Plus.Dialogs;
using Serein.Plus.ViewModels;
using Serein.Plus.Windows;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Pages;

public partial class SchedulePage : Page
{
    private readonly MainWindow _mainWindow;
    private readonly ScheduleProvider _scheduleProvider;
    private readonly ListViewPageViewModel _viewModel;

    public SchedulePage(MainWindow mainWindow, ScheduleProvider scheduleProvider)
    {
        _mainWindow = mainWindow;
        _viewModel = new();
        _scheduleProvider = scheduleProvider;
        DataContext = _viewModel;

        InitializeComponent();
        ScheduleDataGrid.ItemsSource = _scheduleProvider.Value;
        _scheduleProvider.Value.CollectionChanged += (o, e) => Dispatcher.Invoke(UpdateDetails, o, e);
    }

    private void UpdateDetails(object? sender, EventArgs e)
    {
        _viewModel.IsSelected = ScheduleDataGrid.SelectedItems.Count >= 1;
        Details.Text =
            ScheduleDataGrid.SelectedItems.Count > 1
                ? $"共{_scheduleProvider.Value.Count}项，已选择{ScheduleDataGrid.SelectedItems.Count}项"
                : ScheduleDataGrid.SelectedIndex >= 0
                    ? $"共{_scheduleProvider.Value.Count}项，已选择第{ScheduleDataGrid.SelectedIndex + 1}项"
                    : $"共{_scheduleProvider.Value.Count}项";
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        var tag = (sender as MenuItem)?.Tag as string;

        switch (tag)
        {
            case "Add":
                var s1 = new Schedule();
                var dialog1 = new ScheduleEditor(s1) { Owner = _mainWindow };

                if (dialog1.ShowDialog() == true)
                {
                    _scheduleProvider.Value.Add(s1);
                    _scheduleProvider.SaveAsyncWithDebounce();
                }
                break;

            case "Remove":
                DialogHelper.ShowDeleteConfirmation("确定要删除所选项吗？").ContinueWith((task) =>
                {
                    if (!task.Result)
                        return;

                    Dispatcher.Invoke(() =>
                    {
                        foreach (var item in ScheduleDataGrid.SelectedItems.OfType<Schedule>().ToArray())
                            _scheduleProvider.Value.Remove(item);

                        _scheduleProvider.SaveAsyncWithDebounce();
                    });
                });
                break;

            case "Edit":
                if (ScheduleDataGrid.SelectedItem is not Schedule s3)
                    return;

                var s4 = s3.ShallowClone();

                var dialog2 = new ScheduleEditor(s4) { Owner = _mainWindow };

                if (dialog2.ShowDialog() == true)
                {
                    s3.Command = s4.Command;
                    s3.Expression = s4.Expression;
                    s3.Description = s4.Description;
                    s3.IsEnabled = s4.IsEnabled;
                    _scheduleProvider.SaveAsyncWithDebounce();
                }
                break;

            case "Toggle":
                foreach (var item in ScheduleDataGrid.SelectedItems.OfType<Schedule>().ToArray())
                    item.IsEnabled = !item.IsEnabled;
                break;

            case "OpenDoc":
                UrlConstants.DocsSchedule.OpenInBrowser();
                break;

            case "Refresh":
                _scheduleProvider.Read();
                break;

            default:
                throw new NotSupportedException();
        }
    }

    private void ScheduleDataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
        EditMenuItem.IsEnabled = ScheduleDataGrid.SelectedItems.Count == 1 && ScheduleDataGrid.SelectedItem is Schedule;
    }
}
