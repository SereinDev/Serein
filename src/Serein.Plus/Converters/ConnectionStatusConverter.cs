using System;
using System.Globalization;
using System.Windows.Data;

namespace Serein.Plus.Converters;

public class ConnectionStatusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool b ? b ? "已连接" : "未连接" : throw new NotSupportedException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
