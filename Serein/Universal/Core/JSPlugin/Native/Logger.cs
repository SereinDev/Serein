using Jint.Native;
using Serein.Base;
using System;

namespace Serein.Core.JSPlugin.Native
{
    internal class Logger
    {
        public Logger(string name)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
        }

        private readonly string _name;

#pragma warning disable IDE1006
        public void info(params JsValue[] jsValues)
            => Utils.Logger.Output(LogType.Plugin_Info, $"[{_name}]", string.Join<JsValue>("\x20", jsValues));

        public void warn(params JsValue[] jsValues)
            => Utils.Logger.Output(LogType.Plugin_Warn, $"[{_name}]", string.Join<JsValue>("\x20", jsValues));

        public void error(params JsValue[] jsValues)
            => Utils.Logger.Output(LogType.Plugin_Error, $"[{_name}]", string.Join<JsValue>("\x20", jsValues));

        public void debug(params JsValue[] jsValues)
            => Utils.Logger.Output(LogType.Debug, $"[{_name}]", string.Join<JsValue>("\x20", jsValues));
#pragma warning restore IDE1006
    }
}