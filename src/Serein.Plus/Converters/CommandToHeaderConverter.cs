using System;
using System.Globalization;
using System.Windows.Data;
using Serein.Core.Models.Commands;

namespace Serein.Plus.Converters;

public class CommandToHeaderConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Command command)
        {
            return "未知";
        }

        return command.Type switch
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
            CommandType.ExecuteJavascriptCodes => "JS",
            CommandType.Debug => "调试",
            CommandType.Invalid => "无效",
            _ => command.Type.ToString(),
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
