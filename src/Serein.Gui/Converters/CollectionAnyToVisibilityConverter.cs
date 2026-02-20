using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Serein.Gui.Converters;

public class CollectionAnyToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool hasItems = false;
        switch (value)
        {
            case ICollection collection:
                hasItems = collection.Count > 0;
                break;

            case IEnumerable enumerable:
            {
                var enumerator = enumerable.GetEnumerator();
                using var enumerator1 = enumerator as IDisposable;
                hasItems = enumerator.MoveNext();
                break;
            }
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

    public object ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        throw new NotImplementedException();
    }
}
