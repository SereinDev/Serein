using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Serein.Plus.Converters;

public class SplitterConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is IEnumerable e && parameter is string p && !string.IsNullOrEmpty(p)
            ? string.Join(p, e.Cast<object>())
            : throw new NotSupportedException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string v && parameter is string p && !string.IsNullOrEmpty(p)&& targetType == typeof(string[]))
        {
            return v.Split(p, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }
        throw new NotSupportedException();
    }
}
