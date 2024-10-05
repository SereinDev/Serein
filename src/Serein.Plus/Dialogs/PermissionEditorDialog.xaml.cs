using System.Linq;
using System.Windows.Controls;

using iNKORE.UI.WPF.Modern.Controls;

using Serein.Core.Services.Permissions;

namespace Serein.Plus.Dialogs;

public partial class PermissionEditorDialog : ContentDialog
{
    private readonly PermissionManager _permissionManager;

    public string PermissionKey { get; set; }
    public bool? Value { get; set; }

    public PermissionEditorDialog(PermissionManager permissionManager, string? key = null, bool? value = null)
    {
        _permissionManager = permissionManager;
        PermissionKey = key ?? string.Empty;
        Value = value;
        DataContext = this;
        InitializeComponent();

        AutoSuggestBox.Text = PermissionKey;
        ValueComboBox.SelectedIndex = Value switch
        {
            true => 1,
            false => 2,
            _ => 0
        };
        Update();
    }

    private void Update()
    {
        if (_permissionManager.Permissions.TryGetValue(AutoSuggestBox.Text, out var description))
        {
            DescriptionTextBlock.Text = description;
            WarningInfoBar.IsOpen = false;
        }
        else
        {
            DescriptionTextBlock.Text = string.Empty;
            WarningInfoBar.IsOpen = true;
        }

        PermissionKey = AutoSuggestBox.Text; // fail to bind
    }

    private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        Update();

        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            AutoSuggestBox.ItemsSource = _permissionManager.Permissions.Keys.Select((key) => key.Contains(AutoSuggestBox.Text));
    }

    private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.ChosenSuggestion is not null)
            AutoSuggestBox.Text = args.ChosenSuggestion.ToString() ?? string.Empty;
    }

    private void ValueComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Value = ValueComboBox.SelectedIndex switch
        {
            1 => true,
            2 => false,
            _ => null
        };
    }
}
