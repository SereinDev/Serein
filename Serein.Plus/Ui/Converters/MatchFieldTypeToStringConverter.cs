using System;
using System.Globalization;
using System.Windows.Data;

using Serein.Core.Models.Commands;

namespace Serein.Plus.Ui.Converters;

public class MatchFieldTypeToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is MatchFieldType matchFieldType
            ? matchFieldType switch
            {
                MatchFieldType.Disabled => "禁用",
                MatchFieldType.ServerOutput => "服务器输出",
                MatchFieldType.ServerInput => "服务器输入",
                MatchFieldType.GroupMsg => "群聊消息",
                MatchFieldType.PrivateMsg => "私聊消息",
                MatchFieldType.SelfMsg => "自身消息",
                _ => throw new NotSupportedException()
            }
            : "禁用";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
