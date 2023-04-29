using Jint.Native;
using Serein.Base;
using Serein.Utils;
using System;

namespace Serein.Core.JSPlugin
{
    internal class JSLogger
    {
        public JSLogger(string name)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        private readonly string Name;

#pragma warning disable IDE1006
        public void info()
            => info(null);

        public void info(JsValue jsValue)
            => Logger.Output(LogType.Plugin_Info, $"[{Name}]", jsValue);

        public void warn()
            => warn(null);

        public void warn(JsValue jsValue)
            => Logger.Output(LogType.Plugin_Warn, $"[{Name}]", jsValue);

        public void error()
            => error(null);

        public void error(JsValue jsValue)
            => Logger.Output(LogType.Plugin_Error, $"[{Name}]", jsValue);

        public void debug()
            => debug(null);

        public void debug(JsValue jsValue)
            => Logger.Output(LogType.Debug, $"[{Name}]", jsValue);
#pragma warning restore IDE1006
    }
}