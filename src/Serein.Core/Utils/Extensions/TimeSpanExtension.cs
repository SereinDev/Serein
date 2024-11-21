using System;
using System.Diagnostics.CodeAnalysis;

namespace Serein.Core.Utils.Extensions;

public static class TimeSpanExtension
{
    /// <summary>
    /// 转换为常规字符串
    /// </summary>
    [return: NotNullIfNotNull(nameof(timeSpan))]
    public static string? ToCommonString(this TimeSpan? timeSpan)
    {
        return timeSpan is null
            ? null
            : $"{(int)timeSpan.Value.TotalHours}:{timeSpan.Value.Minutes.ToString().PadLeft(2, '0')}:{timeSpan.Value.Seconds.ToString().PadLeft(2, '0')}";
    }
}
