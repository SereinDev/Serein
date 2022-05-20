namespace Serein
{
    class Settings_Bot
    {
        public bool EnableLog { get; set; } = false;
        public bool GivePermissionToAllAdmin { get; set; } = false;
        public long[] GroupList { get; set; } = { };
        public long[] PermissionList { get; set; } = { };
        public int Port { get; set; } = 6700;
    }
}
