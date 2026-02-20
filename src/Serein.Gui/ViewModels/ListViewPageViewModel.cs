using System.Collections;
using Serein.Core.Models.Abstractions;

namespace Serein.Gui.ViewModels;

public class ListViewPageViewModel : NotifyPropertyChangedModelBase
{
    public bool CanRemove { get; set; }

    public bool CanEdit { get; set; }

    public void UpdateSelection(IList selectedItems)
    {
        CanRemove = selectedItems.Count > 0;
        CanEdit = selectedItems.Count == 1;
    }
}
