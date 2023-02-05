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

        private string Name { get; set; }

#pragma warning disable IDE1006
        public void info(object line)
            => Logger.Output(LogType.Plugin_Info, $"[{Name}]", (line ?? string.Empty).ToString());

        public void warn(object line)
            => Logger.Output(LogType.Plugin_Warn, $"[{Name}]", (line ?? string.Empty).ToString());

        public void error(object line)
            => Logger.Output(LogType.Plugin_Error, $"[{Name}]", (line ?? string.Empty).ToString());

        public void debug(object line)
            => Logger.Output(LogType.Debug, $"[{Name}]", (line ?? string.Empty).ToString());
#pragma warning restore IDE1006
    }
}