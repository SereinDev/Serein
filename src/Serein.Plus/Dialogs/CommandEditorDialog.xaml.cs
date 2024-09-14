using iNKORE.UI.WPF.Modern.Controls;

namespace Serein.Plus.Dialogs;

public partial class CommandEditorDialog : ContentDialog
{
    public CommandEditorDialog(string? command = null)
    {
        Command = command ?? string.Empty;
        DataContext = this;

        InitializeComponent();
    }

    public string Command { get; set; }
}
