using System;
using System.Globalization;
using System.Windows.Data;

namespace Serein.Plus.Converters;

public class EnumConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return System.Convert.ToInt32(value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return !targetType.IsEnum
            ? throw new InvalidOperationException()
            : Enum.Parse(targetType, value?.ToString() ?? throw new InvalidOperationException());
    }
}
