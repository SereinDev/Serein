using System;
using System.Collections.Generic;

namespace Serein.Plugin
{
    internal class Event : IDisposable
    {
        public void Dispose()
        {
            onServerStart.Clear();
            onServerStop.Clear();
            onServerSendCommand.Clear();
            onServerOutput.Clear();
            onServerOriginalOutput.Clear();
            onGroupIncrease.Clear();
            onGroupDecrease.Clear();
            onGroupPoke.Clear();
            onReceiveGroupMessage.Clear();
            onReceivePrivateMessage.Clear();
            onReceivePacket.Clear();
            onSereinStart.Clear();
            onSereinClose.Clear();
            onPluginsReload.Clear();
        }

        public List<Delegate> onServerStart { get; set; } = new List<Delegate>();
        public List<Delegate> onServerStop { get; set; } = new List<Delegate>();
        public List<Delegate> onServerSendCommand { get; set; } = new List<Delegate>();
        public List<Delegate> onServerOutput { get; set; } = new List<Delegate>();
        public List<Delegate> onServerOriginalOutput { get; set; } = new List<Delegate>();
        public List<Delegate> onGroupIncrease { get; set; } = new List<Delegate>();
        public List<Delegate> onGroupDecrease { get; set; } = new List<Delegate>();
        public List<Delegate> onGroupPoke { get; set; } = new List<Delegate>();
        public List<Delegate> onReceiveGroupMessage { get; set; } = new List<Delegate>();
        public List<Delegate> onReceivePrivateMessage { get; set; } = new List<Delegate>();
        public List<Delegate> onReceivePacket { get; set; } = new List<Delegate>();
        public List<Delegate> onSereinStart { get; set; } = new List<Delegate>();
        public List<Delegate> onSereinClose { get; set; } = new List<Delegate>();
        public List<Delegate> onPluginsReload { get; set; } = new List<Delegate>();
    }
}
