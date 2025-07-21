using System;
using System.Globalization;
using System.Windows.Data;

namespace Serein.Plus.Converters;

public class EnumValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return System.Convert.ToInt32(value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return !targetType.IsEnum
            ? throw new NotSupportedException()
            : Enum.Parse(targetType, value?.ToString() ?? throw new NotSupportedException());
    }
}
