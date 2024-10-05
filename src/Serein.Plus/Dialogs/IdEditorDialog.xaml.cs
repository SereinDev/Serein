using iNKORE.UI.WPF.Modern.Controls;

namespace Serein.Plus.Dialogs;

public partial class IdEditorDialog : ContentDialog
{
    public IdEditorDialog(long id)
    {
        Id = id;
        DataContext = this;

        InitializeComponent();
    }

    public long Id { get; set; }
}
