using System;
using System.Windows;
using System.Windows.Controls;
using Serein.Core.Services.Data;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;
using Serein.Gui.Dialogs;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Gui.Pages;

public partial class AutomationPage : Page
{
    private readonly MainWindow _mainWindow;
    private readonly AutomationTaskProvider _automationTaskProvider;

    public AutomationPage(MainWindow mainWindow, AutomationTaskProvider automationTaskProvider)
    {
        _mainWindow = mainWindow;
        _automationTaskProvider = automationTaskProvider;
        InitializeComponent();

        automationTasksListView.ItemsSource = _automationTaskProvider.Value;
        _automationTaskProvider.Value.CollectionChanged += UpdateDetails;

        UpdateDetails(this, EventArgs.Empty);
    }

    private void UpdateDetails(object? sender, EventArgs e)
    {
        Details.Text =
            automationTasksListView.SelectedItems.Count > 1
                ? $"共{_automationTaskProvider.Value.Count}项，已选择{automationTasksListView.SelectedItems.Count}项"
            : automationTasksListView.SelectedIndex >= 0
                ? $"共{_automationTaskProvider.Value.Count}项，已选择第{automationTasksListView.SelectedIndex + 1}项"
            : $"共{_automationTaskProvider.Value.Count}项";
    }

    private void AutomationesDataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
        //RemoveMenuItem.IsEnabled = AutomationesDataGrid.SelectedItems.Count > 0;
        //EditMenuItem.IsEnabled =
        //    AutomationesDataGrid.SelectedItems.Count == 1
        //    && AutomationesDataGrid.SelectedItem is AutomationTask;
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        var tag = (sender as MenuItem)?.Tag as string;

        switch (tag)
        {
            case "Add":
                // var m1 = new AutomationTask();
                // var dialog1 = new AutomationEditor(m1) { Owner = _mainWindow };

                // if (dialog1.ShowDialog() == true)
                // {
                //     _automationTaskProvider.Value.Add(m1);
                //     _automationTaskProvider.SaveAsyncWithDebounce();
                // }
                break;

            case "Remove":
                DialogHelper
                    .ShowDeleteConfirmation("确定要删除所选项吗？")
                    .ContinueWith(
                        (task) =>
                        {
                            if (!task.Result)
                            {
                                return;
                            }
                            Dispatcher.Invoke(() =>
                            {
                                //foreach (
                                //    var item in AutomationesDataGrid
                                //        .SelectedItems.OfType<AutomationTask>()
                                //        .ToArray()
                                //)
                                //{
                                //    _automationTaskProvider.Value.Remove(item);
                                //}

                                _automationTaskProvider.SaveAsyncWithDebounce();
                            });
                        }
                    );
                break;

            case "Edit":
                //if (AutomationesDataGrid.SelectedItem is not AutomationTask task3)
                //{
                //    return;
                //}

                // var m4 = task3.ShallowClone();

                // var dialog2 = new AutomationEditor(m4) { Owner = _mainWindow };

                // if (dialog2.ShowDialog() == true)
                // {
                //     task3.Command = m4.Command;
                //     task3.RegExp = m4.RegExp;
                //     task3.Description = m4.Description;
                //     task3.FieldType = m4.FieldType;
                //     task3.RequireAdmin = m4.RequireAdmin;
                //     task3.Exclusions = m4.Exclusions;
                //     _automationTaskProvider.SaveAsyncWithDebounce();
                // }
                break;

            case "OpenDoc":
                // UrlConstants.DocsAutomation.OpenInBrowser();
                break;

            case "OpenVariablesDoc":
                UrlConstants.DocsVariables.OpenInBrowser();
                break;

            case "Refresh":
                _automationTaskProvider.Read();
                break;

            default:
                throw new NotSupportedException();
        }
    }
}
