using System;
using System.Globalization;
using System.Windows.Data;

namespace Serein.Gui.Converters;

public class ServerTabToolTipConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length != 5)
        {
            throw new NotSupportedException();
        }

        var serverId = values[0] as string;
        var status = values[1] as bool?;
        var pid = values[2] as int?;
        var fileName = values[3] as string;
        var argument = values[4] as string;

        if (string.IsNullOrEmpty(serverId) || status is null)
        {
            throw new NotSupportedException();
        }

        if (!status.Value)
        {
            return string.IsNullOrEmpty(fileName)
                ? $"{serverId} · 未启动"
                : $"""
                    {serverId} · 未启动
                    {fileName} {argument}
                    """;
        }

        return $"""
            {serverId} · 运行中 (PID: {pid})
            {fileName} {argument}
            """;
        ;
    }

    public object[] ConvertBack(
        object value,
        Type[] targetTypes,
        object parameter,
        CultureInfo culture
    )
    {
        throw new NotSupportedException();
    }
}
