using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using iNKORE.UI.WPF.Modern.Controls;

using Serein.Core.Models.Commands;
using Serein.Core.Services.Data;
using Serein.Plus.Dialogs;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Pages;

public partial class MatchPage : Page
{
    private readonly MatchesProvider _matchesProvider;

    public MatchPage(MatchesProvider matchesProvider)
    {
        _matchesProvider = matchesProvider;
        InitializeComponent();
        MatchesDataGrid.ItemsSource = _matchesProvider.Value;
        _matchesProvider.Value.CollectionChanged += UpdateDetails;
    }

    private void MatchesDataGrid_LayoutUpdated(object sender, EventArgs e)
    {
        _matchesProvider.SaveAsyncWithDebounce();
        UpdateDetails(sender, e);
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
        Remove.IsEnabled =
            MatchesDataGrid.SelectedItems.Count > 1
            || MatchesDataGrid.SelectedItems.Count == 1 && MatchesDataGrid.SelectedItem is Match;
        Edit.IsEnabled =
            MatchesDataGrid.SelectedItems.Count == 1 && MatchesDataGrid.SelectedItem is Match;
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        var tag = (sender as MenuItem)?.Tag?.ToString();

        switch (tag)
        {
            case "Add":
                var m1 = new Match();
                new MatchEditorDialog(m1)
                    .ShowAsync()
                    .ContinueWith(
                        (r) =>
                        {
                            if (r.Result == ContentDialogResult.Primary)
                                Dispatcher.Invoke(() => _matchesProvider.Value.Add(m1));
                        }
                    );
                break;

            case "Remove":
                foreach (var item in MatchesDataGrid.SelectedItems.OfType<Match>())
                    _matchesProvider.Value.Remove(item);

                break;

            case "Edit":
                if (MatchesDataGrid.SelectedItem is not Match m3)
                    return;

                var m4 = (m3.Clone() as Match)!;

                new MatchEditorDialog(m4)
                    .ShowAsync()
                    .ContinueWith(
                        (r) =>
                        {
                            if (r.Result == ContentDialogResult.Primary)
                            {
                                m3.Command = m4.Command;
                                m3.RegExp = m4.RegExp;
                                m3.Description = m4.Description;
                                m3.FieldType = m4.FieldType;
                                m3.RequireAdmin = m4.RequireAdmin;
                                m3.Exclusions = m4.Exclusions;
                            };
                        }
                    );
                break;

            case "OpenDoc":

                break;

            case "Refresh":
                _matchesProvider.Read();
                break;

            default:
                throw new NotSupportedException();
        }
    }
}
