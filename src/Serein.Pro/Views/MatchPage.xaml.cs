using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

using Serein.Core;
using Serein.Core.Services.Data;

namespace Serein.Pro.Views;

public sealed partial class MatchPage : Page
{
    private readonly IServiceProvider _services;

    private MatchesProvider MatchesProvider { get; }

    public MatchPage()
    {
        _services = SereinApp.Current!.Services;
        MatchesProvider = _services.GetRequiredService<MatchesProvider>();

        InitializeComponent();
    }
}
