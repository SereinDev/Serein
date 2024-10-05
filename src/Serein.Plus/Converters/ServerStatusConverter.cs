using System;
using System.Globalization;
using System.Windows.Data;

using Serein.Core.Models.Server;

namespace Serein.Plus.Converters;

public class ServerStatusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ServerStatus status)
            return status switch
            {
                ServerStatus.Running => "运行中",
                ServerStatus.Stopped => "已停止",
                ServerStatus.Unknown => "未启动",
                _ => throw new NotSupportedException()
            };

        throw new NotSupportedException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
