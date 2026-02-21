using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using iNKORE.UI.WPF.Modern.Controls.Helpers;
using Serein.Gui.ViewModels;

namespace Serein.Gui.Controls;

public partial class ServerTabControl : TabControl
{
    private const double HeaderActiveHeight = 32d;
    private Point _dragStartPoint;
    private PanelTabItem? _draggedTab;
    private InsertionAdorner? _insertionAdorner;

    public ServerTabControl()
    {
        InitializeComponent();

        AddHandler(
            PreviewMouseLeftButtonDownEvent,
            new MouseButtonEventHandler(OnTabItemPreviewMouseLeftButtonDown),
            true
        );
        AddHandler(PreviewMouseMoveEvent, new MouseEventHandler(OnTabItemPreviewMouseMove), true);
        AddHandler(DragOverEvent, new DragEventHandler(OnTabItemDragOver), true);
        AddHandler(DropEvent, new DragEventHandler(OnTabItemDrop), true);
        AddHandler(DragLeaveEvent, new DragEventHandler(OnTabItemDragLeave), true);
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

        if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            if (SelectedIndex < 0 && Items.Count > 0)
            {
                SelectedIndex = 0;
            }
            return;
        }
    }

    #region 拖拽相关

    private void OnTabItemPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _dragStartPoint = e.GetPosition(this);
        _draggedTab = FindParentTabItem(e.OriginalSource as DependencyObject);
    }

    private void OnTabItemPreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed || _draggedTab == null)
        {
            return;
        }

        var currentPosition = e.GetPosition(this);
        var horizontalMove = Math.Abs(currentPosition.X - _dragStartPoint.X);
        var verticalMove = Math.Abs(currentPosition.Y - _dragStartPoint.Y);

        if (
            horizontalMove < SystemParameters.MinimumHorizontalDragDistance
            && verticalMove < SystemParameters.MinimumVerticalDragDistance
        )
        {
            return;
        }

        DragDrop.DoDragDrop(_draggedTab, _draggedTab, DragDropEffects.Move);
        _draggedTab = null;
    }

    private void OnTabItemDragOver(object sender, DragEventArgs e)
    {
        if (!IsValidTabDrag(e.Data))
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
            return;
        }

        var targetTab = FindParentTabItem(e.OriginalSource as DependencyObject);
        if (targetTab == null || IsOutsideHeader(e))
        {
            e.Effects = DragDropEffects.None;
            ClearAdorner();
            e.Handled = true;
            return;
        }

        var insertBefore = IsInsertBefore(e, targetTab);
        ShowAdorner(targetTab, insertBefore);
        e.Effects = DragDropEffects.Move;
        e.Handled = true;
    }

    private void OnTabItemDrop(object sender, DragEventArgs e)
    {
        if (!IsValidTabDrag(e.Data))
        {
            return;
        }

        if (e.Data.GetData(typeof(PanelTabItem)) is not TabItem sourceTab)
        {
            return;
        }

        var targetTab = FindParentTabItem(e.OriginalSource as DependencyObject);
        if (targetTab == null || ReferenceEquals(sourceTab, targetTab) || IsOutsideHeader(e))
        {
            ClearAdorner();
            return;
        }

        var insertBefore = IsInsertBefore(e, targetTab);
        MoveTabItem(sourceTab, targetTab, insertBefore);
        ClearAdorner();
        _draggedTab = null;
    }

    private void OnTabItemDragLeave(object sender, DragEventArgs e)
    {
        if (!IsValidTabDrag(e.Data))
        {
            return;
        }

        ClearAdorner();
    }

    private void MoveTabItem(TabItem sourceTab, TabItem targetTab, bool insertBefore)
    {
        var sourceItem = ItemContainerGenerator.ItemFromContainer(sourceTab);
        var targetItem = ItemContainerGenerator.ItemFromContainer(targetTab);

        if (
            sourceItem == DependencyProperty.UnsetValue
            || targetItem == DependencyProperty.UnsetValue
        )
        {
            return;
        }

        var sourceIndex = Items.IndexOf(sourceItem);
        var targetIndex = Items.IndexOf(targetItem);

        if (sourceIndex < 0 || targetIndex < 0)
        {
            return;
        }

        if (ItemsSource is ObservableCollection<PanelViewModel> observablePanels)
        {
            if (!insertBefore)
            {
                targetIndex++;
            }

            if (sourceIndex < targetIndex)
            {
                targetIndex--;
            }

            if (targetIndex == sourceIndex)
            {
                return;
            }

            observablePanels.Move(sourceIndex, targetIndex);
            SelectedItem = sourceItem;
        }
        else
        {
            throw new NotSupportedException();
        }
    }

    private static PanelTabItem? FindParentTabItem(DependencyObject? source)
    {
        while (source != null && source is not PanelTabItem)
        {
            source = VisualTreeHelper.GetParent(source);
        }

        return source as PanelTabItem;
    }

    private bool IsValidTabDrag(IDataObject data)
    {
        return _draggedTab != null && data.GetData(typeof(PanelTabItem)) is PanelTabItem;
    }

    private bool IsOutsideHeader(DragEventArgs e)
    {
        var position = e.GetPosition(this);
        return position.Y > HeaderActiveHeight && position.X > 10;
    }

    private static bool IsInsertBefore(DragEventArgs e, TabItem targetTab)
    {
        var position = e.GetPosition(targetTab);
        return position.X < targetTab.ActualWidth / 2;
    }

    private void ShowAdorner(TabItem targetTab, bool insertBefore)
    {
        var layer = AdornerLayer.GetAdornerLayer(targetTab);
        if (layer == null)
        {
            return;
        }

        if (
            _insertionAdorner != null
            && _insertionAdorner.AdornedElement == targetTab
            && _insertionAdorner.InsertBefore == insertBefore
        )
        {
            return;
        }

        ClearAdorner();

        _insertionAdorner = new InsertionAdorner(targetTab, insertBefore);
        layer.Add(_insertionAdorner);
    }

    private void ClearAdorner()
    {
        if (_insertionAdorner == null)
        {
            return;
        }

        var layer = AdornerLayer.GetAdornerLayer(_insertionAdorner.AdornedElement);
        layer?.Remove(_insertionAdorner);
        _insertionAdorner = null;
    }

    private sealed class InsertionAdorner : Adorner
    {
        private readonly Pen _linePen;
        private const double LineLength = 16d;

        public InsertionAdorner(UIElement adornedElement, bool insertBefore)
            : base(adornedElement)
        {
            InsertBefore = insertBefore;
            IsHitTestVisible = false;

            var brush = TryFindResource("AccentFillColorDefaultBrush") is Brush resourceBrush
                ? resourceBrush.CloneCurrentValue()
                : new SolidColorBrush(Color.FromArgb(200, 0, 120, 215));

            brush.Opacity = 0.78;
            brush.Freeze();

            _linePen = new Pen(brush, 2);
            _linePen.Freeze();
        }

        public bool InsertBefore { get; }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (AdornedElement is not FrameworkElement element)
            {
                return;
            }

            var x = InsertBefore ? 0 : element.ActualWidth;
            var midY = element.ActualHeight / 2;
            var y1 = midY - LineLength / 2;
            var y2 = midY + LineLength / 2;

            drawingContext.DrawLine(_linePen, new Point(x, y1), new Point(x, y2));
        }
    }
    #endregion
}
