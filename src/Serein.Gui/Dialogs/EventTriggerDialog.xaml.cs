using iNKORE.UI.WPF.Modern.Controls;
using Serein.Core.Models.Automations.Triggers;

namespace Serein.Gui.Dialogs;

public partial class EventTriggerDialog : ContentDialog
{
    public EventTriggerDialog(EventTrigger trigger, bool isEdit)
    {
        Title = isEdit ? "编辑事件触发器" : "新建事件触发器";
        DataContext = trigger;
        InitializeComponent();
    }
}
