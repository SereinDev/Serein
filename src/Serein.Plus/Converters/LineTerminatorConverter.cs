using System;
using System.Globalization;
using System.Windows.Data;

namespace Serein.Plus.Converters;

public class LineTerminatorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is string str
            ? (object)str.Replace("\r", "\\r").Replace("\n", "\\n")
            : throw new NotSupportedException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is string str
            ? (object)str.Replace("\\r", "\r").Replace("\\n", "\n")
            : throw new NotSupportedException();
    }
}
