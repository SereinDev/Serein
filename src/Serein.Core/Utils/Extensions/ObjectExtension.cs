using System;

namespace Serein.Core.Utils.Extensions;

public static class ObjectExtension
{
    /// <summary>
    /// 转换为指定类型
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public static T OfType<T>(this object? o)
        where T : notnull
    {
        return o is T t ? t : throw new InvalidOperationException();
    }
}
