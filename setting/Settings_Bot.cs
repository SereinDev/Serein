using System;

namespace Serein
{
    internal class Settings_Bot
    {
        public bool EnableLog { get; set; } = false;
        public bool GivePermissionToAllAdmin { get; set; } = false;
        public bool EnbaleOutputData { get; set; } = false;
        public long[] GroupList { get; set; } = Array.Empty<long>();
        public long[] PermissionList { get; set; } = Array.Empty<long>();
        public int Port { get; set; } = 6700;
    }
}
