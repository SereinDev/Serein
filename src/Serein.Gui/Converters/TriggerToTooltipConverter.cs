using System;
using System.Globalization;
using System.Windows.Data;
using Serein.Core.Models.Automations;
using Serein.Core.Models.Automations.Triggers;

namespace Serein.Gui.Converters;

#pragma warning disable IDE0046

public class TriggerToTooltipConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
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
                MatchTrigger t =>
                    $"匹配域：{GetMatchFieldName(t.FieldType)}\r\n"
                    + (IsMessageFieldType(t.FieldType)
                        ? $"需要管理员权限：{(t.RequireAdminPermission ? "是" : "否")}\r\n"
                        : string.Empty)
                    + $"{(t.IsRegex ? "正则表达式" : "匹配文本")}：{t.Pattern}",
                PluginTrigger t => $"标识符：{t.Key}",
                ScheduleTrigger t => $"Cron表达式：{t.CronExpression}",
                EventTrigger t => $"事件：{GetEventName(t.Event)}",
                _ => "未知触发器",
            };
    }

    private static bool IsMessageFieldType(MatchFieldType type)
    {
        return type
            is MatchFieldType.GroupMsg
                or MatchFieldType.PrivateMsg
                or MatchFieldType.SelfMsg
                or MatchFieldType.ChannelMsg
                or MatchFieldType.GuildMsg;
    }

    private static string GetMatchFieldName(MatchFieldType type)
    {
        return type switch
        {
            MatchFieldType.ServerOutput => "服务器输出",
            MatchFieldType.ServerInput => "服务器输入",
            MatchFieldType.GroupMsg => "群聊消息",
            MatchFieldType.PrivateMsg => "私聊消息",
            MatchFieldType.SelfMsg => "自身消息",
            MatchFieldType.ChannelMsg => "频道消息",
            MatchFieldType.GuildMsg => "群组消息",
            _ => "未知",
        };
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

    public object ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        throw new NotImplementedException();
    }
}
