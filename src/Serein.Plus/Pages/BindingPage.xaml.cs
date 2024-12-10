using System.Windows;
using System.Windows.Controls;
using Serein.Core.Services.Bindings;

namespace Serein.Plus.Pages;

public partial class BindingPage : Page
{
    private readonly BindingManager _bindingManager;

    public BindingPage(BindingManager bindingManager)
    {
        _bindingManager = bindingManager;

        InitializeComponent();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        BindingListView.ItemsSource = _bindingManager.Records;
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        BindingListView.ItemsSource = _bindingManager.Records;
    }
}
