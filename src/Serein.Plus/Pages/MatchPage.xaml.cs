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
using Serein.Plus.Windows;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Pages;

public partial class MatchPage : Page
{
    private readonly MainWindow _mainWindow;
    private readonly MatchesProvider _matchesProvider;

    public MatchPage(MainWindow mainWindow, MatchesProvider matchesProvider)
    {
        _mainWindow = mainWindow;
        _matchesProvider = matchesProvider;
        InitializeComponent();
        MatchesDataGrid.ItemsSource = _matchesProvider.Value;
        _matchesProvider.Value.CollectionChanged += UpdateDetails;
    }

    private void UpdateDetails(object? sender, EventArgs e)
    {
        Details.Text =
            MatchesDataGrid.SelectedItems.Count > 1
                ? $"共{_matchesProvider.Value.Count}项，已选择{MatchesDataGrid.SelectedItems.Count}项"
                : MatchesDataGrid.SelectedIndex >= 0
                    ? $"共{_matchesProvider.Value.Count}项，已选择第{MatchesDataGrid.SelectedIndex + 1}项"
                    : $"共{_matchesProvider.Value.Count}项";
    }

    private void MatchesDataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
        RemoveMenuItem.IsEnabled = MatchesDataGrid.SelectedItems.Count > 0;
        EditMenuItem.IsEnabled =
            MatchesDataGrid.SelectedItems.Count == 1 && MatchesDataGrid.SelectedItem is Match;
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        var tag = (sender as MenuItem)?.Tag as string;

        switch (tag)
        {
            case "Add":
                var m1 = new Match();
                var dialog1 = new MatchEditor(m1) { Owner = _mainWindow };

                if (dialog1.ShowDialog() == true)
                {
                    _matchesProvider.Value.Add(m1);
                    _matchesProvider.SaveAsyncWithDebounce();
                }
                break;

            case "Remove":
                DialogHelper.ShowDeleteConfirmation("确定要删除所选项吗？").ContinueWith((task) =>
                {
                    if (!task.Result)
                        return;
                    Dispatcher.Invoke(() =>
                {

                    foreach (var item in MatchesDataGrid.SelectedItems.OfType<Match>().ToArray())
                        _matchesProvider.Value.Remove(item);

                    _matchesProvider.SaveAsyncWithDebounce();
                });
                });
                break;

            case "Edit":
                if (MatchesDataGrid.SelectedItem is not Match m3)
                    return;

                var m4 = m3.ShallowClone();

                var dialog2 = new MatchEditor(m4) { Owner = _mainWindow };

                if (dialog2.ShowDialog() == true)
                {
                    m3.Command = m4.Command;
                    m3.RegExp = m4.RegExp;
                    m3.Description = m4.Description;
                    m3.FieldType = m4.FieldType;
                    m3.RequireAdmin = m4.RequireAdmin;
                    m3.Exclusions = m4.Exclusions;
                    _matchesProvider.SaveAsyncWithDebounce();
                }
                break;

            case "OpenDoc":
                UrlConstants.DocsMatch.OpenInBrowser();
                break;

            case "Refresh":
                _matchesProvider.Read();
                break;

            default:
                throw new NotSupportedException();
        }
    }
}
