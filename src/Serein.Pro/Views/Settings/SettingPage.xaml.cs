using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

using Serein.Core;
using Serein.Pro.ViewModels;

namespace Serein.Pro.Views.Settings;

public sealed partial class SettingPage : Page
{
    private readonly IServiceProvider _services;
    private SettingViewModel ViewModel { get; }

    public SettingPage()
    {
        _services = SereinApp.Current!.Services;
        ViewModel = _services.GetRequiredService<SettingViewModel>();

        InitializeComponent();
    }

    private void SelectorBar_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args)
    {
        if (SelectorBar.SelectedItem.Tag is not Type type)
            type = typeof(BlankPage);
        ContentFrame.Navigate(type,null ,new DrillInNavigationTransitionInfo());
    }
}
