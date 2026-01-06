using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Serein.Plus.Helpers
{
    public static class GridViewColumnHelper
    {
        public static readonly DependencyProperty MinWidthProperty =
            DependencyProperty.RegisterAttached(
                "MinWidth",
                typeof(double),
                typeof(GridViewColumnHelper),
                new PropertyMetadata(0.0, OnMinWidthChanged)
            );

        public static double GetMinWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(MinWidthProperty);
        }

        public static void SetMinWidth(DependencyObject obj, double value)
        {
            obj.SetValue(MinWidthProperty, value);
        }

        private static void OnMinWidthChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e
        )
        {
            if (d is GridViewColumn column)
            {
                ((INotifyPropertyChanged)column).PropertyChanged -= Column_PropertyChanged;
                ((INotifyPropertyChanged)column).PropertyChanged += Column_PropertyChanged;

                // Initial check
                if (column.Width < (double)e.NewValue)
                {
                    column.Width = (double)e.NewValue;
                }
            }
        }

        private static void Column_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Width" && sender is GridViewColumn column)
            {
                var minWidth = GetMinWidth(column);
                if (column.Width < minWidth)
                {
                    column.Width = minWidth;
                }
            }
        }
    }
}
