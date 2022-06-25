using System.Collections.Generic;

namespace Serein
{
    internal class Settings_Bot
    {
        public bool EnableLog { get; set; } = false;
        public bool GivePermissionToAllAdmin { get; set; } = false;
        public bool EnbaleOutputData { get; set; } = false;
        public List<long> GroupList { get; set; } = new List<long>();
        public List<long> PermissionList { get; set; } = new List<long>();
        public string Uri { get; set; } = "127.0.0.1:6700";
        public string Authorization { get; set; } = string.Empty;
    }
}
