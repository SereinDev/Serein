using Serein.Server;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Serein.Base
{
    internal static class AutoRun
    {
        /// <summary>
        /// 检查自动运行
        /// </summary>
        public static void Check()
        {
            if (Global.Settings.Serein.AutoRun.ConnectWS || ((IList<string>)Environment.GetCommandLineArgs()).Contains("auto_connect"))
                Task.Run(() => Websocket.Connect(true));
            if (Global.Settings.Serein.AutoRun.StartServer || ((IList<string>)Environment.GetCommandLineArgs()).Contains("auto_start"))
                Task.Run(() => ServerManager.Start(false));
        }
    }
}