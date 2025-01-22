using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Serein.Core.Services.Bindings;

namespace Serein.Plus.Pages;

public partial class BindingPage : Page
{
    private readonly Lazy<BindingManager> _bindingManager;

    public BindingPage(IServiceProvider serviceProvider)
    {
        _bindingManager = new(serviceProvider.GetRequiredService<BindingManager>());

        InitializeComponent();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        Task.Run(() => _bindingManager.Value.Records)
            .ContinueWith((task) => Dispatcher.Invoke(() =>
            {
                Ring.Visibility = Visibility.Collapsed;
                BindingListView.IsEnabled = true;
                BindingListView.ItemsSource = task.Result;
            }));
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        BindingListView.ItemsSource = _bindingManager.Value.Records;
    }
}
