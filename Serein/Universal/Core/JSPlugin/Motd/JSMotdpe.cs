using Serein.Base.Motd;
using System;

namespace Serein.Core.JSPlugin.Motd
{
    internal class JSMotdpe : Motdpe
    {
        public JSMotdpe(int port)
        {
            Port = port;
        }

        public JSMotdpe(string addr)
        {
            if (!base.TryParse(addr))
            {
                throw new ArgumentException("地址无效", nameof(addr));
            }
            if (Port == -1)
            {
                Port = 19132;
            }
        }

#pragma warning disable IDE1006

        public void get() => base.Get();
        public void tryGet() => base.TryGet();
        public string ip => IP.ToString();
        public int port => Port;
        public int onlinePlayer => onlinePlayer;
        public int maxPlayer => maxPlayer;
        public string description => Description;
        public string protocol => Protocol;
        public string version => Version;
        public string levelName => LevelName;
        public string gameMode => GameMode;
        public double delay => Delay;
        public string exception => Exception;
        public bool isSuccessful => IsSuccessful;

#pragma warning restore IDE1006

    }
}