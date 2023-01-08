using Serein.Base;
using Serein.Items;
using System;

namespace Serein.JSPlugin
{
    internal class JSLogger
    {
        public JSLogger(string name)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        private string Name { get; set; }

#pragma warning disable IDE1006
        public void info(object line)
            => Logger.Out(LogType.Plugin_Info, $"[{Name}]", (line ?? string.Empty).ToString());

        public void warn(object line)
            => Logger.Out(LogType.Plugin_Warn, $"[{Name}]", (line ?? string.Empty).ToString());

        public void error(object line)
            => Logger.Out(LogType.Plugin_Error, $"[{Name}]", (line ?? string.Empty).ToString());

        public void debug(object line)
            => Logger.Out(LogType.Debug, $"[{Name}]", (line ?? string.Empty).ToString());
#pragma warning restore IDE1006
    }
}