using System.Collections.Generic;
using System;

namespace Serein.Plugin
{
    internal class Event
    {
        public List<Delegate> onServerStart { get; set; } = new List<Delegate>();
        public List<Delegate> onServerStop { get; set; } = new List<Delegate>();
        public List<Delegate> onServerSendCommand { get; set; } = new List<Delegate>();
        public List<Delegate> onServerOutput { get; set; } = new List<Delegate>();
        public List<Delegate> onGroupIncrease { get; set; } = new List<Delegate>();
        public List<Delegate> onGroupDecrease { get; set; } = new List<Delegate>();
        public List<Delegate> onGroupPoke { get; set; } = new List<Delegate>();
        public List<Delegate> onReceiveGroupMessage { get; set; } = new List<Delegate>();
        public List<Delegate> onReceivePrivateMessage { get; set; } = new List<Delegate>();
        public List<Delegate> onReceivePacket { get; set; } = new List<Delegate>();
        public List<Delegate> onSereinStart { get; set; } = new List<Delegate>();
        public List<Delegate> onSereinClose { get; set; } = new List<Delegate>();
    }
}
