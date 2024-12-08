using System;
using Jint.Runtime;

namespace Serein.Core.Utils.Extensions;

public static class ExceptionExtension
{
    /// <summary>
    /// 获取异常详细信息
    /// </summary>
    public static string GetDetailString(this Exception e)
    {
        return e is JavaScriptException ex
            ? $"{ex.Message}\n{ex.JavaScriptStackTrace}"
            : $"{e.GetType().FullName}: {e.Message}";
    }
}
