using System;
using System.Globalization;
using System.Windows.Data;

namespace Serein.Plus.Ui.Converters;

public class EscapingConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string str)
            return str.Replace("\r", "\\r").Replace("\n", "\\n");

        throw new NotSupportedException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string str)
            return str.Replace("\\r", "\r").Replace("\\n", "\n");

        throw new NotSupportedException();
    }
}
