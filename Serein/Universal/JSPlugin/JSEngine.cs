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
using System.Threading;
using SystemInfoLibrary.OperatingSystem;

namespace Serein.JSPlugin
{
    internal static class JSEngine
    {
        /// <summary>
        /// JS引擎
        /// </summary>
        public static Engine MainEngine = new();

        /// <summary>
        /// 转换专用JS引擎
        /// </summary>
        public static Engine Converter = new();

        /// <summary>
        /// 初始化JS引擎
        /// </summary>
        /// <param name="executeByCommand">被命令执行</param>
        /// <param name="namespace">命名空间</param>
        /// <param name="cancellationTokenSource">取消Token</param>
        /// <returns>JS引擎</returns>
        public static Engine Init(bool executeByCommand = false, string @namespace = null, CancellationTokenSource cancellationTokenSource = null)
        {
            Engine engine = new(
                new Action<Options>((cfg) =>
                {
                    cfg.AllowClr(typeof(Process).Assembly);
                    cfg.CatchClrExceptions();
                    if (executeByCommand)
                    {
                        cfg.TimeoutInterval(TimeSpan.FromMinutes(1));
                    }
                    else if (cancellationTokenSource != null)
                    {
                        cfg.CancellationToken(cancellationTokenSource.Token);
                    }
                }
                ));
            engine.SetValue("Serein_SystemInfo",
                new Func<object>(() => SystemInfo.Info ?? OperatingSystemInfo.GetOperatingSystemInfo()));
#if !UNIX
            engine.SetValue("Serein_CPUUsage",
                new Func<float>(() => SystemInfo.CPUUsage));
#else
            engine.SetValue("Serein_CPUUsage", JsValue.Undefined);
#endif
            engine.SetValue("Serein_NetSpeed",
                new Func<Array>(() => new[] { SystemInfo.UploadSpeed, SystemInfo.DownloadSpeed }));
            engine.SetValue("Serein_Global_Path",
                Global.Path);
            engine.SetValue("Serein_Global_Version",
                Global.VERSION);
            engine.SetValue("Serein_Current_Namespace",
                @namespace);
            engine.SetValue("Serein_Log",
                new Action<object>((Content) => Logger.Out(LogType.Plugin_Info, $"[{@namespace}]", Content)));
            engine.SetValue("Serein_Command_Run",
                new Action<string>((command) => Command.Run(5, command)));
            engine.SetValue("Serein_Global_Debug",
                new Action<object>((Content) => Logger.Out(LogType.Debug, Content)));
            engine.SetValue("Serein_Global_Settings",
                new Func<string>(() => JsonConvert.SerializeObject(Global.Settings)));
            engine.SetValue("Serein_Plugin_JSFunc_Register",
                new Func<string, string, string, string, string>((Name, Version, Author, Description) => JSFunc.Register(@namespace, Name, Version, Author, Description)));
            engine.SetValue("Serein_Plugin_JSFunc_SetListener",
                new Func<string, Delegate, bool>((EventName, Function) => JSFunc.SetListener(@namespace, EventName, Function)));
            engine.SetValue("Serein_Motdpe",
                new Func<string, string>((addr) => new Motdpe(addr).Origin));
            engine.SetValue("Serein_Motdje",
                new Func<string, string>((addr) => new Motdje(addr).Origin));
            engine.SetValue("Serein_ServerManager_Start",
                new Func<bool>(() => ServerManager.Start(true)));
            engine.SetValue("Serein_ServerManager_Stop",
                new Action(() => ServerManager.Stop(true)));
            engine.SetValue("Serein_ServerManager_Kill",
                new Func<bool>(() => ServerManager.Kill(true)));
            engine.SetValue("Serein_ServerManager_Status",
                new Func<bool>(() => ServerManager.Status));
            engine.SetValue("Serein_ServerManager_Send",
                new Action<string, bool>((Commnad, Unicode) => ServerManager.InputCommand(Commnad, usingUnicode: Unicode)));
            engine.SetValue("Serein_ServerManager_GetTime",
                new Func<string>(() => ServerManager.GetTime()));
            engine.SetValue("Serein_ServerManager_GetCPUUsage",
                new Func<double>(() => ServerManager.CPUUsage));
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
                new Func<Delegate, JsValue, JsValue>((Function, Interval) => JSFunc.SetTimer(@namespace, Function, Interval, false)));
            engine.SetValue("setInterval",
                new Func<Delegate, JsValue, JsValue>((Function, Interval) => JSFunc.SetTimer(@namespace, Function, Interval, true)));
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
                @"var serein = {
                    log: Serein_Log,
                    path: Serein_Global_Path,
                    namespace: Serein_Current_Namespace,
                    version: Serein_Global_Version,
                    getSettings: Serein_Global_Settings,
                    debugLog: Serein_Global_Debug,
                    runCommand: Serein_Command_Run,
                    registerPlugin: Serein_Plugin_JSFunc_Register,
                    setListener: Serein_Plugin_JSFunc_SetListener,
                    getCPUUsage: Serein_CPUUsage,
                    getNetSpeed: Serein_NetSpeed,
                    getSysInfo: Serein_SystemInfo,
                    getMotdpe: Serein_Motdpe,
                    getMotdje: Serein_Motdje,
                    startServer: Serein_ServerManager_Start,
                    stopServer: Serein_ServerManager_Stop,
                    sendCmd: Serein_ServerManager_Send,
                    killServer: Serein_ServerManager_Kill,
                    getServerStatus: Serein_ServerManager_Status,
                    getServerTime: Serein_ServerManager_GetTime,
                    getServerCPUUsage: Serein_ServerManager_GetCPUUsage,
                    getServerFile: Serein_ServerManager_GetFilename,
                    sendGroup: Serein_Websocket_SendGroup,
                    sendPrivate: Serein_Websocket_SendPrivate,
                    sendPacket: Serein_Websocket_SendPacket,
                    getWsStatus: Serein_Websocket_Status,
                    bindMember: Serein_Member_Bind,
                    unbindMember: Serein_Member_UnBind,
                    getID: Serein_Member_GetID,
                    getGameID: Serein_Member_GetGameID
                    };"
            );
            return engine;
        }

        /// <summary>
        /// 运行代码
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns>错误信息</returns>
        public static Engine Run(string code, Engine engine, out string exceptionMessage)
        {
            try
            {
                (engine ?? MainEngine).Execute(code);
                exceptionMessage = string.Empty;
            }
            catch (JavaScriptException e)
            {
                Logger.Out(LogType.Debug, e);
                exceptionMessage = $"{e.Message}\n{e.JavaScriptStackTrace}";
            }
            catch (Exception e)
            {
                Logger.Out(LogType.Debug, e);
                exceptionMessage = e.Message;
            }
            return engine ?? MainEngine;
        }
    }
}
