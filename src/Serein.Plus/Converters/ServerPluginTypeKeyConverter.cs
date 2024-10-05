using System;
using System.Globalization;
using System.Windows.Data;

using Serein.Core.Models.Server;

namespace Serein.Plus.Converters;

public class ServerPluginTypeKeyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is PluginType type)
            if (type == PluginType.Library)
                return "动态链接库";
            else
                return type.ToString();

        throw new NotSupportedException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
