using System.Windows;
using System.Windows.Controls;
using Serein.Core.Models.Commands;

namespace Serein.Plus.Windows;

public partial class MatchEditor : Window
{
    public MatchEditor(Match match)
    {
        InitializeComponent();
        DataContext = match;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var index = (sender as ComboBox)?.SelectedIndex;
        if ((MatchFieldType?)index is MatchFieldType.GroupMsg or MatchFieldType.PrivateMsg)
        {
            RequireAdminCheckBox.IsEnabled = true;
        }
        else
        {
            RequireAdminCheckBox.IsChecked = RequireAdminCheckBox.IsEnabled = false;
        }
    }
}
