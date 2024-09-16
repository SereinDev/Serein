using System;
using System.Reflection;

namespace Serein.Core.Utils.Extensions;

public static class ObjectExtension
{
    public static T OfType<T>(this object? o)
        where T : notnull
    {
        return o is T t ? t : throw new InvalidOperationException();
    }

    public static T WiseClone<T>(this T o) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(o, nameof(o));

        var type = typeof(T);
        if (type.IsValueType)
            return o;

        var result = Activator.CreateInstance<T>();
        WiseCloneTo(o, result);

        return result;
    }

    public static void WiseCloneTo<T>(this T from, T to) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(from, nameof(from));
        ArgumentNullException.ThrowIfNull(to, nameof(to));

        var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
        foreach (var field in fields)
            field.SetValue(to, field.GetValue(from));
    }
}
