using iNKORE.UI.WPF.Modern.Controls;
using Serein.Core.Models.Automations.Triggers;

namespace Serein.Gui.Dialogs;

public partial class PluginTriggerDialog : ContentDialog
{
    public PluginTriggerDialog(PluginTrigger trigger, bool isEdit)
    {
        Title = isEdit ? "编辑插件触发器" : "新建插件触发器";
        DataContext = trigger;
        InitializeComponent();
    }
}
