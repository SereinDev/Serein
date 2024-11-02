using System;
using System.Globalization;
using System.Windows.Data;

namespace Serein.Plus.Converters;

public class ReverseBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool b ? !b
            : value is null ? true
            : throw new NotSupportedException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool b ? !b
            : value is null ? true
            : throw new NotSupportedException();
    }
}
