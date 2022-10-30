using Serein.Items;
using System;

namespace Serein.Plugin
{
    internal class JSLogger
    {
        public JSLogger(string Name)
        {
            this.Name = Name ?? throw new ArgumentNullException(nameof(Name));
        }

        private string Name { get; set; }

#pragma warning disable IDE1006
        public void info(object Line)
            => Logger.Out(LogType.Plugin_Info, $"[{Name}]", (Line ?? string.Empty).ToString());

        public void warn(object Line)
            => Logger.Out(LogType.Plugin_Warn, $"[{Name}]", (Line ?? string.Empty).ToString());

        public void error(object Line)
            => Logger.Out(LogType.Plugin_Error, $"[{Name}]", (Line ?? string.Empty).ToString());

        public void debug(object Line)
            => Logger.Out(LogType.Debug, $"[{Name}]", (Line ?? string.Empty).ToString());
#pragma warning restore IDE1006
    }
}