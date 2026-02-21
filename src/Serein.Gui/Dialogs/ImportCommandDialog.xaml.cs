using iNKORE.UI.WPF.Modern.Controls;

namespace Serein.Gui.Dialogs;

public partial class ImportCommandDialog : ContentDialog
{
    public ImportCommandDialog()
    {
        InitializeComponent();
    }

    public string Command { get; set; } = string.Empty;
}
