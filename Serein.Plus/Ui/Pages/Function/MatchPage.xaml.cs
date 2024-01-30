using System;
using System.Collections.ObjectModel;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Commands;
using Serein.Core.Services.Data;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Ui.Pages.Function;

public partial class MatchPage : Page
{
    private readonly IHost _host;
    private IServiceProvider Services => _host.Services;
    private MatchesProvider MatchesProvider => Services.GetRequiredService<MatchesProvider>();
    public ObservableCollection<Match> Matches => MatchesProvider.Value;

    public MatchPage(IHost host)
    {
        _host = host;
        InitializeComponent();
        MatchesDataGrid.ItemsSource = Matches;
        Matches.CollectionChanged += UpdateDetails;
    }

    private void OnLayoutUpdated(object sender, EventArgs e)
    {
        MatchesProvider.SaveAsyncWithDebounce();
        UpdateDetails(sender, e);
    }

    private void UpdateDetails(object? sender, EventArgs e)
    {
        Details.Text =
            MatchesDataGrid.SelectedItems.Count > 1
                ? $"共{Matches.Count}项，已选择{MatchesDataGrid.SelectedItems.Count}项"
                : MatchesDataGrid.SelectedIndex >= 0
                    ? $"共{Matches.Count}项，已选择第{MatchesDataGrid.SelectedIndex + 1}项"
                    : $"共{Matches.Count}项";
    }
}
