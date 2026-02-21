using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using iNKORE.UI.WPF.Modern.Controls.Helpers;
using Serein.Gui.ViewModels;

namespace Serein.Gui.Controls;

public partial class ServerTabControl : TabControl
{
    public ServerTabControl()
    {
        InitializeComponent();
    }

    protected override bool IsItemItsOwnContainerOverride(object item)
    {
        return item is PanelTabItem || base.IsItemItsOwnContainerOverride(item);
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
        return new PanelTabItem();
    }

    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
        base.PrepareContainerForItemOverride(element, item);

        if (element is not PanelTabItem tabItem)
        {
            return;
        }

        if (item is PanelViewModel viewModel)
        {
            tabItem.BindViewModel(viewModel);
        }

        TabItemHelper.SetIsClosable(tabItem, false);
    }

    protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
        base.OnItemsChanged(e);

        if (Items.Count == 0)
        {
            return;
        }

        if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems?.Count > 0)
        {
            SelectedItem = e.NewItems[0];
            return;
        }

        SelectedIndex = 0;
    }
}
