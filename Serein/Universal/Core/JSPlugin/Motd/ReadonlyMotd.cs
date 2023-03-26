using Jint.Native;

namespace Serein.Core.JSPlugin.Motd
{
    internal struct ReadonlyMotd
    {
        private Base.Motd.Motd Motd;

#pragma warning disable IDE1006

        public string ip => Motd.IP.ToString();
        public int port => Motd.Port;
        public int onlinePlayer => Motd.OnlinePlayer;
        public int maxPlayer => Motd.MaxPlayer;
        public string description => Motd.Description;
        public string protocol => Motd.Protocol;
        public string version => Motd.Version;
        public double delay => Motd.Delay;
        public string exception => Motd.Exception;
        public bool isSuccessful => Motd.IsSuccessful;
        public string favicon => Motd.Favicon;
        public string levelName => Motd.LevelName;
        public string gameMode => Motd.GameMode;
        public string origin => Motd.Origin;

#pragma warning restore IDE1006

        public ReadonlyMotd(Base.Motd.Motd motd)
            => Motd = motd;
    }
}