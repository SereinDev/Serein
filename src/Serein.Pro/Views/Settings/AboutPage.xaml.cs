using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

using Serein.Core;
using Serein.Pro.ViewModels;

namespace Serein.Pro.Views.Settings;

public sealed partial class AboutPage : Page
{
    private readonly IServiceProvider _services;
    public AboutViewModel ViewModel { get; }

    public AboutPage()
    {
        _services = SereinApp.Current!.Services;
        ViewModel = _services.GetRequiredService<AboutViewModel>();

        InitializeComponent();
    }
}
