using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Serein.Gui.Utils;
using TriggerBase = Serein.Core.Models.Automations.Triggers.TriggerBase;

namespace Serein.Gui.Windows;

public partial class TriggersEditor : Window
{
    public new ObservableCollection<TriggerBase> Triggers { get; }

    public TriggersEditor(ObservableCollection<TriggerBase> triggers)
    {
        InitializeComponent();
        Triggers = triggers;
        DataContext = this;
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not MenuItem menuItem)
        {
            return;
        }

        switch (menuItem.Tag as string)
        {
            case "Add":
                break;

            case "Delete":
                DialogFactory
                    .ShowDeleteConfirmation("确定要删除所选项吗？")
                    .ContinueWith(
                        (task) =>
                        {
                            if (!task.Result)
                            {
                                return;
                            }

                            foreach (var trigger in ListView.SelectedItems.OfType<TriggerBase>())
                            {
                                Triggers.Remove(trigger);
                            }
                        }
                    );
                break;

            case "Edit":
                break;
        }
    }
}
