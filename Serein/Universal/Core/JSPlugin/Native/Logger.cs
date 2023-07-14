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

        private static string _separator = "\x20";

        private readonly string _name;

        public void Info(params JsValue[] jsValues)
            => Utils.Output.Logger.Output(LogType.Plugin_Info, $"[{_name}]", string.Join<JsValue>(_separator, jsValues));

        public void Warn(params JsValue[] jsValues)
            => Utils.Output.Logger.Output(LogType.Plugin_Warn, $"[{_name}]", string.Join<JsValue>(_separator, jsValues));

        public void Error(params JsValue[] jsValues)
            => Utils.Output.Logger.Output(LogType.Plugin_Error, $"[{_name}]", string.Join<JsValue>(_separator, jsValues));

        public void Debug(params JsValue[] jsValues)
            => Utils.Output.Logger.Output(LogType.Debug, $"[{_name}]", string.Join<JsValue>(_separator, jsValues));
    }
}