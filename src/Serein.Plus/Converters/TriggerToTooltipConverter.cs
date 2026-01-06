using System;
using System.Globalization;
using System.Windows.Data;
using Serein.Core.Models.Automations.Triggers;

namespace Serein.Plus.Converters;

#pragma warning disable IDE0046

public class TriggerToTooltipConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not TriggerBase trigger)
        {
            return string.Empty;
        }

        return "状态："
            + (trigger.IsEnabled ? "已启用" : "已禁用")
            + "\r\n"
            + trigger switch
            {
                MatchTrigger t => $"{(t.IsRegex ? "正则表达式" : "匹配文本")}：{t.Pattern}",
                PluginTrigger t => $"标识符：{t.Key}",
                ScheduleTrigger t => $"Cron表达式：{t.CronExpression}",
                EventTrigger t => $"事件列表：{string.Join(", ", t.Events)}",
                _ => "未知触发器",
            };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
