using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Serein.Plus.Converters;

public class CollectionAnyToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool hasItems = false;
        if (value is ICollection collection)
        {
            hasItems = collection.Count > 0;
        }
        else if (value is IEnumerable enumerable)
        {
            var enumerator = enumerable.GetEnumerator();
            hasItems = enumerator.MoveNext();
        }

        var invert =
            parameter is string s && s.Equals("Invert", StringComparison.OrdinalIgnoreCase);

        return invert
            ? hasItems
                ? Visibility.Collapsed
                : Visibility.Visible
            : hasItems
                ? Visibility.Visible
                : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
