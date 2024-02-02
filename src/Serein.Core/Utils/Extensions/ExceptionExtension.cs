using System;

using Jint.Runtime;

namespace Serein.Core.Utils.Extensions;

public static class ExceptionExtension
{
    public static string GetDetailString(this Exception e)
    {
        return e is JavaScriptException ex
            ? $"{ex.Message}\n{ex.JavaScriptStackTrace}"
            : $"{e.GetType().FullName}: {e.Message}";
    }
}
