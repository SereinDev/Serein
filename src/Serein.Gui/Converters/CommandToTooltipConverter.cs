using System;
using System.Globalization;
using System.Windows.Data;
using Serein.Core.Models.Commands;

namespace Serein.Gui.Converters;

public class CommandToTooltipConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Command command)
        {
            return "未知命令";
        }

        var tooltip = $"类型：{command.Type}";
        if (!string.IsNullOrEmpty(command.Body))
        {
            tooltip += $"\r\n主体：{command.Body}";
        }

        if (!string.IsNullOrEmpty(command.Arguments.Target))
        {
            tooltip += $"\r\n目标：{command.Arguments.Target}";
        }
        return tooltip;
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
