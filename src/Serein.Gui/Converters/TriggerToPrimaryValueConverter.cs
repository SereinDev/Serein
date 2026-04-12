using System;
using System.Globalization;
using System.Windows.Data;
using Serein.Core.Models.Automations;
using Serein.Core.Models.Automations.Triggers;

namespace Serein.Gui.Converters;

public class TriggerToPrimaryValueConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            MatchTrigger trigger => string.IsNullOrWhiteSpace(trigger.Pattern)
                ? "(空)"
                : trigger.Pattern,
            ScheduleTrigger trigger => string.IsNullOrWhiteSpace(trigger.CronExpression)
                ? "(空)"
                : trigger.CronExpression,
            PluginTrigger trigger => string.IsNullOrWhiteSpace(trigger.Key) ? "(空)" : trigger.Key,
            EventTrigger trigger => GetEventName(trigger.Event),
            _ => "(空)",
        };
    }

    public object ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        throw new NotImplementedException();
    }

    private static string GetEventName(Events type)
    {
        return type switch
        {
            Events.None => "无",
            Events.ServerStart => "服务器启动",
            Events.ServerExitedNormally => "服务器关闭：正常退出",
            Events.ServerExitedUnexpectedly => "服务器关闭：不正常退出",
            Events.GroupIncreased => "群人数增加",
            Events.GroupDecreased => "群人数减少",
            Events.GroupPoke => "群戳一戳",
            Events.BindingSucceeded => "绑定成功",
            Events.UnbindingSucceeded => "解绑成功",
            Events.PermissionDeniedFromPrivateMsg => "权限不足：私聊",
            Events.PermissionDeniedFromGroupMsg => "权限不足：群聊",
            Events.SereinCrash => "Serein崩溃",
            _ => "未知",
        };
    }
}
