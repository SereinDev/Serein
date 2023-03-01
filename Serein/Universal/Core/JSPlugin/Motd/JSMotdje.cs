using Serein.Base.Motd;
using System;

namespace Serein.Core.JSPlugin.Motd
{
    internal class JSMotdje : Motdje
    {
        public JSMotdje(int port)
        {
            Port = port;
        }

        public JSMotdje(string addr)
        {
            if (!base.TryParse(addr))
            {
                throw new ArgumentException("地址无效", nameof(addr));
            }
            if (Port == -1)
            {
                Port = 25565;
            }
        }

#pragma warning disable IDE1006

        public void get() => base.Get();
        public void tryGet() => base.TryGet();
        public string ip => IP.ToString();
        public int port => Port;
        public int onlinePlayer => OnlinePlayer;
        public int maxPlayer => MaxPlayer;
        public string description => Description;
        public string protocol => Protocol;
        public string version => Version;
        public double delay => Delay;
        public string exception => Exception;
        public bool isSuccessful => IsSuccessful;
        public string favicon => Favicon;

#pragma warning restore IDE1006

    }
}