using System;
using System.Globalization;
using System.Windows.Data;

namespace Serein.Gui.Converters;

public class CommandToTooltipConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 3)
        {
            return "未知命令";
        }

        var type = values[0]?.ToString() ?? "未知";
        var body = values[1]?.ToString();
        var target = values[2]?.ToString();

        var tooltip = $"类型：{type}";
        if (!string.IsNullOrEmpty(body))
        {
            tooltip += $"\r\n主体：{body}";
        }

        if (!string.IsNullOrEmpty(target))
        {
            tooltip += $"\r\n目标：{target}";
        }

        return tooltip;
    }

    public object[] ConvertBack(
        object value,
        Type[] targetTypes,
        object parameter,
        CultureInfo culture
    )
    {
        throw new NotImplementedException();
    }
}
