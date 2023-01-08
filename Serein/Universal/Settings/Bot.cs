using System.Collections.Generic;

namespace Serein.Settings
{
    internal class Bot
    {
        public string Authorization = string.Empty;
        public bool AutoEscape;
        public bool AutoReconnect;
        public bool EnableLog;
        public bool EnbaleOutputData;
        public bool EnbaleParseAt = true;
        public bool GivePermissionToAllAdmin;
        public List<long> GroupList = new();
        public List<long> PermissionList = new();
        public string Uri = "127.0.0.1:6700";
    }
}
