using System.Linq;
using System.Windows.Controls;
using iNKORE.UI.WPF.Modern.Controls;
using Serein.Core.Services.Permissions;

namespace Serein.Plus.Dialogs;

public partial class PermissionEditorDialog : ContentDialog
{
    private readonly PermissionManager _permissionManager;

    public string Node { get; set; }
    public bool? Value { get; set; }

    public PermissionEditorDialog(
        PermissionManager permissionManager,
        string? node = null,
        bool? value = null
    )
    {
        _permissionManager = permissionManager;
        Node = node ?? string.Empty;
        Value = value;
        DataContext = this;
        InitializeComponent();

        ValueComboBox.SelectedIndex = Value switch
        {
            true => 1,
            false => 2,
            _ => 0,
        };
        Update();
    }

    private void Update()
    {
        if (_permissionManager.Nodes.TryGetValue(Node, out var description))
        {
            DescriptionTextBlock.Text = description;
            WarningInfoBar.IsOpen = false;
        }
        else
        {
            DescriptionTextBlock.Text = string.Empty;
            WarningInfoBar.IsOpen = true;
        }
    }

    private void AutoSuggestBox_TextChanged(
        AutoSuggestBox sender,
        AutoSuggestBoxTextChangedEventArgs args
    )
    {
        Update();

        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            sender.ItemsSource = _permissionManager.Nodes.Keys.Select((key) => key.Contains(Node));
        }
    }

    private void AutoSuggestBox_QuerySubmitted(
        AutoSuggestBox sender,
        AutoSuggestBoxQuerySubmittedEventArgs args
    )
    {
        if (args.ChosenSuggestion is not null)
        {
            Node = args.ChosenSuggestion.ToString() ?? string.Empty;
        }
    }

    private void ValueComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Value = ValueComboBox.SelectedIndex switch
        {
            1 => true,
            2 => false,
            _ => null,
        };
    }
}
