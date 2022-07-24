using System.Collections.Generic;

namespace Serein.Plugin
{
    internal class Event
    {
        public List<string> onServerStart { get; set; } = new List<string>();
        public List<string> onServerStop { get; set; } = new List<string>();
        public List<string> onServerSendCommand { get; set; } = new List<string>();
        public List<string> onGroupIncrease { get; set; } = new List<string>();
        public List<string> onGroupDecrease { get; set; } = new List<string>();
        public List<string> onGroupPoke { get; set; } = new List<string>();
        public List<string> onReceiveGroupMessage { get; set; } = new List<string>();
        public List<string> onReceivePrivateMessage { get; set; } = new List<string>();
        public List<string> onSereinStart { get; set; } = new List<string>();
        public List<string> onSereinClose { get; set; } = new List<string>();
    }
}
