using System.Collections.Generic;

namespace Serein.Settings
{
    internal class Bot
    {
        public string Authorization = string.Empty;
        public bool AutoEscape = false;
        public bool AutoReconnect = false;
        public bool EnableLog = false;
        public bool EnbaleOutputData = false;
        public bool EnbaleParseAt = true;
        public bool GivePermissionToAllAdmin = false;
        public List<long> GroupList = new List<long>();
        public List<long> PermissionList = new List<long>();
        public string Uri = "127.0.0.1:6700";
    }
}
