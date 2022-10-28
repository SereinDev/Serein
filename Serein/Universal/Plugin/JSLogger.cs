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

        public void Info(object Line)
            => Logger.Out(LogType.Info, $"[{Name}]", (Line ?? string.Empty).ToString());

        public void Warn(object Line)
            => Logger.Out(LogType.Warn, $"[{Name}]", (Line ?? string.Empty).ToString());

        public void Error(object Line)
            => Logger.Out(LogType.Error, $"[{Name}]", (Line ?? string.Empty).ToString());

        public void Debug(object Line)
            => Logger.Out(LogType.Debug, $"[{Name}]", (Line ?? string.Empty).ToString());
    }
}