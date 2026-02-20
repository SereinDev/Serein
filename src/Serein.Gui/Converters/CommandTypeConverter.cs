using System;
using System.Globalization;
using System.Windows.Data;
using Serein.Core.Models.Commands;

namespace Serein.Gui.Converters;

public class CommandTypeConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is not CommandType commandType
            ? "未知"
            : commandType switch
            {
                CommandType.ExecuteShellCommand => "终端",
                CommandType.InputServer => "服务器",
                CommandType.SendGroupMsg => "群聊",
                CommandType.SendPrivateMsg => "私聊",
                CommandType.SendChannelMsg => "频道",
                CommandType.SendGuildMsg => "公会",
                CommandType.SendReply => "回复",
                CommandType.SendData => "数据",
                CommandType.Bind => "绑定",
                CommandType.Unbind => "解绑",
                CommandType.ExecuteJavascriptCodes => "Js",
                CommandType.Debug => "调试",
                CommandType.Invalid => "无效",
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
            };
    }

    public object ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        throw new NotSupportedException();
    }
}
