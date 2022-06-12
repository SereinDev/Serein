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
        public int Port { get; set; } = 6700;
    }
}
