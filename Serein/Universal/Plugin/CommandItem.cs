using System;

namespace Serein.Plugin
{
    internal class CommandItem
    {
        public string Command { get; set; } = string.Empty;
        public Delegate Function { get; set; }
    }
}
