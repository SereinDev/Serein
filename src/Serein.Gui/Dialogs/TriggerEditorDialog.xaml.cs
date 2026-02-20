using iNKORE.UI.WPF.Modern.Controls;
using Serein.Core.Models.Commands;

namespace Serein.Gui.Dialogs;

public partial class TriggerEditorDialog : ContentDialog
{
    public TriggerEditorDialog(Command command)
    {
        DataContext = command;
        InitializeComponent();
    }
}
