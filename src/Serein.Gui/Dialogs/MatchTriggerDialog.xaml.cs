using iNKORE.UI.WPF.Modern.Controls;
using Serein.Core.Models.Automations.Triggers;

namespace Serein.Gui.Dialogs;

public partial class MatchTriggerDialog : ContentDialog
{
    public MatchTriggerDialog(MatchTrigger trigger, bool isEdit)
    {
        Title = isEdit ? "编辑匹配触发器" : "新建匹配触发器";
        DataContext = trigger;
        InitializeComponent();
    }
}
