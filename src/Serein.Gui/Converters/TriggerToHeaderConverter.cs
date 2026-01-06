using System;
using System.Globalization;
using System.Windows.Data;
using Serein.Core.Models.Automations.Triggers;

namespace Serein.Gui.Converters;

public class TriggerToHeaderConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            MatchTrigger => "匹配",
            PluginTrigger => "插件",
            ScheduleTrigger => "定时",
            EventTrigger => "事件",
            _ => "未知",
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
