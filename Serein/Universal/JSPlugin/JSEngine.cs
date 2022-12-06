using Jint;
using Jint.Native;
using Jint.Runtime;
using Jint.Runtime.Interop;
using Newtonsoft.Json;
using Serein.Base;
using Serein.Items;
using Serein.Items.Motd;
using Serein.Server;
using System;
using System.Diagnostics;

namespace Serein.JSPlugin
{
    internal static class JSEngine
    {
        /// <summary>
        /// JS引擎
        /// </summary>
        public static Engine engine = new Engine();

        /// <summary>
        /// 转换专用JS引擎
        /// </summary>
        public static Engine Converter = new Engine();

        /// <summary>
        /// 初始化JS引擎
        /// </summary>
        /// <param name="ExecuteByCommand">被命令执行</param>
        /// <returns>JS引擎</returns>
        public static Engine Init(bool ExecuteByCommand = false,string Name = null)
        {
            Engine engine = new Engine(
                new Action<Options>((cfg) =>
                {
                    cfg.AllowClr(typeof(Process).Assembly);
                    cfg.CatchClrExceptions();
                    if (ExecuteByCommand)
                        cfg.TimeoutInterval(TimeSpan.FromMinutes(1));
                }
                ));
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
                        return SystemInfo.UsedRAM.ToString();
                    case "totalram":
                        return SystemInfo.TotalRAM.ToString();
                    case "rampercentage":
                        return SystemInfo.RAMPercentage;
                    default:
                        return string.Empty;
                }
            }));
            engine.SetValue("Serein_Global_Path",
                Global.Path);
            engine.SetValue("Serein_Global_Version",
                Global.VERSION);
            engine.SetValue("Serein_Log",
                new Action<object>((Content) => Logger.Out(LogType.Plugin_Info, Content)));
            engine.SetValue("Serein_Command_Run",
                new Action<string>((command) => Command.Run(5, command)));
            engine.SetValue("Serein_Global_Debug",
                new Action<object>((Content) => Logger.Out(LogType.Debug, Content)));
            engine.SetValue("Serein_Global_Settings",
                new Func<string>(() => JsonConvert.SerializeObject(Global.Settings)));
            engine.SetValue("Serein_Plugin_JSFunc_Register",
                new Func<string, string, string, string, bool>(JSFunc.Register));
            engine.SetValue("Serein_Plugin_JSFunc_SetListener",
                new Func<string, Delegate, bool>((EventName,Function)=> JSFunc.SetListener(Name,EventName, Function)));
            engine.SetValue("Serein_Motdpe",
                new Func<string, string>((IP) => new Motdpe(IP).Original));
            engine.SetValue("Serein_Motdje",
                new Func<string, string>((IP) => new Motdje(IP).Original));
            engine.SetValue("Serein_ServerManager_Start",
                new Func<bool>(() => ServerManager.Start(true)));
            engine.SetValue("Serein_ServerManager_Stop",
                new Action(() => ServerManager.Stop(true)));
            engine.SetValue("Serein_ServerManager_Kill",
                new Func<bool>(() => ServerManager.Kill(true)));
            engine.SetValue("Serein_ServerManager_Status",
                new Func<bool>(() => ServerManager.Status));
            engine.SetValue("Serein_ServerManager_Send",
                new Action<string, bool>((Commnad, Unicode) => ServerManager.InputCommand(Commnad, Unicode: Unicode)));
            engine.SetValue("Serein_ServerManager_GetTime",
                new Func<string>(() => ServerManager.GetTime()));
            engine.SetValue("Serein_ServerManager_GetCPUPersent",
                new Func<string>(() => ServerManager.CPUPersent.ToString("N2")));
            engine.SetValue("Serein_ServerManager_GetFilename",
                new Func<string>(() => ServerManager.StartFileName));
            engine.SetValue("Serein_Websocket_SendGroup",
                new Func<long, string, bool>((Target, Message) => Websocket.Send(false, Message, Target)));
            engine.SetValue("Serein_Websocket_SendPrivate",
                new Func<long, string, bool>((Target, Message) => Websocket.Send(true, Message, Target)));
            engine.SetValue("Serein_Websocket_SendPacket",
                new Func<string, bool>((Msg) => Websocket.Send(Msg)));
            engine.SetValue("Serein_Websocket_Status",
                new Func<bool>(() => Websocket.Status));
            engine.SetValue("Serein_Member_Bind",
                new Func<long, string, bool>(Binder.Bind));
            engine.SetValue("Serein_Member_UnBind",
                new Func<long, bool>(Binder.UnBind));
            engine.SetValue("Serein_Member_GetID",
                new Func<string, long>(Binder.GetID));
            engine.SetValue("Serein_Member_GetGameID",
                new Func<long, string>(Binder.GetGameID));
            engine.SetValue("setTimeout",
                new Func<Delegate, JsValue, JsValue>((Function, Interval) => JSFunc.SetTimer(Function, Interval, false)));
            engine.SetValue("setInterval",
                new Func<Delegate, JsValue, JsValue>((Function, Interval) => JSFunc.SetTimer(Function, Interval, true)));
            engine.SetValue("clearTimeout",
                new Func<JsValue, bool>(JSFunc.ClearTimer));
            engine.SetValue("clearInterval",
                new Func<JsValue, bool>(JSFunc.ClearTimer));
            engine.SetValue("getMD5",
                new Func<string, string>(JSFunc.GetMD5));
            engine.SetValue("WebSocket",
                TypeReference.CreateTypeReference(engine, typeof(JSWebSocket)));
            engine.SetValue("Logger",
                TypeReference.CreateTypeReference(engine, typeof(JSLogger)));
            engine.Execute(
                "var serein={" +
                "log:Serein_Log," +
                "path:Serein_Global_Path," +
                "version:Serein_Global_Version," +
                "getSettings:Serein_Global_Settings," +
                "debugLog:Serein_Global_Debug," +
                "runCommand:Serein_Command_Run," +
                "registerPlugin:Serein_Plugin_JSFunc_Register," +
                "setListener:Serein_Plugin_JSFunc_SetListener," +
                "getSysInfo:Serein_SystemInfo," +
                "getMotdpe:Serein_Motdpe," +
                "getMotdje:Serein_Motdje," +
                "startServer:Serein_ServerManager_Start," +
                "stopServer:Serein_ServerManager_Stop," +
                "sendCmd:Serein_ServerManager_Send," +
                "killServer:Serein_ServerManager_Kill," +
                "getServerStatus:Serein_ServerManager_Status," +
                "getServerTime:Serein_ServerManager_GetTime," +
                "getServerCPUPersent:Serein_ServerManager_GetCPUPersent," +
                "getServerFile:Serein_ServerManager_GetFilename," +
                "sendGroup:Serein_Websocket_SendGroup," +
                "sendPrivate:Serein_Websocket_SendPrivate," +
                "sendPacket:Serein_Websocket_SendPacket," +
                "getWsStatus:Serein_Websocket_Status," +
                "bindMember:Serein_Member_Bind," +
                "unbindMember:Serein_Member_UnBind," +
                "getID:Serein_Member_GetID," +
                "getGameID:Serein_Member_GetGameID" +
                "};");
            return engine;
        }

        /// <summary>
        /// 运行代码
        /// </summary>
        /// <param name="Code">代码</param>
        /// <returns>错误信息</returns>
        public static string Run(string Code, Engine Engine = null)
        {
            try
            {
                (Engine ?? engine).Execute(Code);
                return string.Empty;
            }
            catch (JavaScriptException e)
            {
                Logger.Out(LogType.Debug, e);
                return $"{e.Message} (at line {e.Location.Start.Line}:{e.Location.Start.Column})";
            }
            catch (Exception e)
            {
                Logger.Out(LogType.Debug, e);
                return e.Message;
            }
        }
    }
}
