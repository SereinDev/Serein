using System;
using System.Globalization;
using System.Windows.Data;
using EmbedIO;

namespace Serein.Gui.Converters;

public class WebServerStateToTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is WebServerState state)
        {
            return state switch
            {
                WebServerState.Listening => "运行中",
                WebServerState.Loading => "加载中",
                WebServerState.Created => "已创建",
                WebServerState.Stopped => "未启动",
                _ => throw new ArgumentOutOfRangeException(nameof(value)),
            };
        }

        throw new ArgumentOutOfRangeException(nameof(value));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
