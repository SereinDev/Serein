using Jint;
using Serein.Base;
using Serein.Items.Motd;
using Serein.Server;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Serein.Plugin
{
    internal class JSEngine
    {
        private static Engine engine = new Engine();
        public static void Init()
        {
            engine = new Engine(cfg => cfg.AllowClr(
                typeof(File).Assembly,
                typeof(Path).Assembly,
                typeof(Directory).Assembly,
                typeof(DirectoryInfo).Assembly,
                typeof(StreamReader).Assembly,
                typeof(StreamWriter).Assembly,
                typeof(Encoding).Assembly,
                typeof(Process).Assembly,
                typeof(ProcessStartInfo).Assembly
                )
            .TimeoutInterval(new TimeSpan(0, 1, 0))
            .CatchClrExceptions()
            );
            engine.SetValue("Serein_SystemInfo", new Func<string, string>((Type) =>
            {
                switch (Type.ToLower())
                {
                    case "os":
                        return SystemInfo.OS;
                    case "net":
                        return SystemInfo.NET;
                    case "cpuname":
                        return SystemInfo.CPUName;
                    case "cpupercentage":
                        return SystemInfo.CPUPercentage;
                    case "usedram":
                        return SystemInfo.UsedRAM;
                    case "totalram":
                        return SystemInfo.TotalRAM;
                    case "rampercentage":
                        return SystemInfo.RAMPercentage;
                    default:
                        return string.Empty;
                }
            }));
            engine.SetValue("Serein_Command_Run", new Action<string>((command) => Command.Run(5, command)));
            engine.SetValue("Serein_Global_Path", Global.Path);
            engine.SetValue("Serein_Global_Version", Global.VERSION);
            engine.SetValue("Serein_Global_Debug", new Action<object>(Global.Debug));
            engine.SetValue("Serein_Plugin_JSFunc_Register", new Func<string, string, string, string, bool>(JSFunc.Register));
            engine.SetValue("Serein_Plugin_JSFunc_SetListener", new Func<string, string, bool>(JSFunc.SetListener));
            engine.SetValue("Serein_Motdpe", new Func<string, string>((IP) => { return new Motdpe(IP).Original; }));
            engine.SetValue("Serein_Motdje", new Func<string, string>((IP) => { return new Motdje(IP).Original; }));
            engine.SetValue("Serein_ServerManager_Start", new Func<bool>(() => ServerManager.Start(true)));
            engine.SetValue("Serein_ServerManager_Stop", new Action(ServerManager.Stop));
            engine.SetValue("Serein_ServerManager_Kill", new Func<bool>(() => ServerManager.Kill(true)));
            engine.SetValue("Serein_ServerManager_Status", new Func<bool>(() => { return ServerManager.Status; }));
            engine.SetValue("Serein_ServerManager_Stop", new Action<string, bool>((Commnad, Unicode) => ServerManager.InputCommand(Commnad, Unicode: Unicode)));
            engine.SetValue("Serein_ServerManager_GetTime", new Func<string>(() => ServerManager.GetTime()));
            engine.SetValue("Serein_ServerManager_GetCPUPersent", new Func<string>(() => { return ServerManager.CPUPersent.ToString("N2"); }));
            engine.SetValue("Serein_Websocket_SendGroup", new Func<long, string, bool>((Target, Message) => { return (Websocket.Send(false, Message, Target)); }));
            engine.SetValue("Serein_Websocket_SendPrivate", new Func<long, string, bool>((Target, Message) => { return (Websocket.Send(true, Message, Target)); }));
            engine.SetValue("Serein_Websocket_Status", new Func<bool>(() => { return Websocket.Status; }));
            engine.SetValue("Serein_Member_Add", new Func<long, bool>(Members.Add));
            engine.SetValue("Serein_Member_Remove", new Func<long, bool>(Members.Remove));
            engine.SetValue("Serein_Member_GetID", new Func<string, long>(Members.GetID));
            engine.SetValue("Serein_Member_GetGameID", new Func<long, string>(Members.GetGameID));
            engine.Execute("var serein={" +
                "path:Serein_Global_Path," +
                "versions:Serein_Global_Version," +
                "debugLog:Serein_Global_Debug," +
                "runCommand:Serein_Command_Run," +
                "registerPlugin:Serein_Plugin_JSFunc_Register," +
                "setListener:Serein_Plugin_JSFunc_SetListener," +
                "getSysInfo:Serein_SystemInfo," +
                "getMotdpe:Serein_Motdpe," +
                "getMotdje:Serein_Motdje," +
                "startServer:Serein_ServerManager_Start," +
                "stopServer:Serein_ServerManager_Stop," +
                "killServer:Serein_ServerManager_Kill," +
                "getServerStatus:Serein_ServerManager_Status," +
                "getServerTime:Serein_ServerManager_GetTime," +
                "getServerCPUPersent:Serein_ServerManager_GetCPUPersent," +
                "sendGroup:Serein_Websocket_SendGroup," +
                "sendPrivate:Serein_Websocket_SendPrivate," +
                "getWsStatus:Serein_Websocket_Status," +
                "addMember:Serein_Member_Add," +
                "removeMember:Serein_Member_Remove," +
                "getID:Serein_Member_GetID," +
                "getGameID:Serein_Member_GetGameID" +
                "};");
        }
        public static bool Run(string Code)
        {
            try
            {
                engine.Execute(Code);
                return true;
            }
            catch (Exception e)
            {
                Global.Debug("[JSEngine] " + e.Message);
                return false;
            }
        }
    }
}
