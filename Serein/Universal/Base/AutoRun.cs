using Serein.Server;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Serein.Base
{
    internal static class AutoRun
    {
        /// <summary>
        /// 检查启动参数
        /// </summary>
        public static void Check()
        {
            IList<string> args = Environment.GetCommandLineArgs();
            if (Global.Settings.Serein.AutoRun.ConnectWS || args.Contains("auto_connect"))
            {
                Task.Run(() => Websocket.Connect(true));
            }
            if (Global.Settings.Serein.AutoRun.StartServer || args.Contains("auto_start"))
            {
                Task.Run(() => ServerManager.Start(false));
            }
        }
    }
}