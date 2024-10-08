using System;

namespace Serein.Core.Utils.Extensions;

public static class ObjectExtension
{
    public static T OfType<T>(this object? o)
        where T : notnull
    {
        return o is T t ? t : throw new InvalidOperationException();
    }
}
