using System.IO;
using System.Collections.Generic;
using Jint;
using Jint.Native;
using Jint.Runtime;
using Jint.Runtime.Interop;
using Newtonsoft.Json;
using Serein.Base;
using Serein.Extensions;
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
                        cfg.Modules.RegisterRequire = true;
                        cfg.EnableModules(Path.Combine(Global.PATH, "plugins"));
                    }
                }
                ));
            engine.SetValue("serein_path",
                Global.PATH);
            engine.SetValue("serein_version",
                Global.VERSION);
            engine.SetValue("serein_namespace",
                string.IsNullOrEmpty(@namespace) ? JsValue.Null : @namespace);
            if (!string.IsNullOrEmpty(@namespace))
            {
                engine.SetValue("serein_log",
                    new Action<JsValue>((content) => Logger.Output(LogType.Plugin_Info, $"[{@namespace}]", content)));
                engine.SetValue("serein_debugLog",
                    new Action<JsValue>((content) => Logger.Output(LogType.Debug, $"[{@namespace}]", content)));
                engine.SetValue("serein_registerPlugin",
                    new Func<string, string, string, string, string>((name, version, author, description) => JSFunc.Register(@namespace, name, version, author, description)));
                engine.SetValue("serein_setListener",
                    new Func<string, Delegate, bool>((eventName, @delegate) => JSFunc.SetListener(@namespace, eventName, @delegate)));
                engine.SetValue("setTimeout",
                    new Func<Delegate, JsValue, JsValue>((Function, Interval) => JSFunc.SetTimer(@namespace, Function, Interval, false)));
                engine.SetValue("setInterval",
                    new Func<Delegate, JsValue, JsValue>((Function, Interval) => JSFunc.SetTimer(@namespace, Function, Interval, true)));
                engine.SetValue("clearTimeout",
                    new Func<JsValue, bool>(JSFunc.ClearTimer));
                engine.SetValue("clearInterval",
                    new Func<JsValue, bool>(JSFunc.ClearTimer));
                engine.SetValue("WSClient",
                    TypeReference.CreateTypeReference(engine, typeof(JSWebSocket)));
                engine.SetValue("Logger",
                    TypeReference.CreateTypeReference(engine, typeof(JSLogger)));
            }
            else
            {
                engine.SetValue("serein_log", JsValue.Undefined);
                engine.SetValue("serein_debugLog", JsValue.Undefined);
                engine.SetValue("serein_registerPlugin", JsValue.Undefined);
                engine.SetValue("serein_setListener", JsValue.Undefined);
                engine.SetValue("setTimeout", JsValue.Undefined);
                engine.SetValue("setInterval", JsValue.Undefined);
                engine.SetValue("clearTimeout", JsValue.Undefined);
                engine.SetValue("clearInterval", JsValue.Undefined);
                engine.SetValue("WSClient", JsValue.Undefined);
                engine.SetValue("Logger", JsValue.Undefined);

            }
            engine.SetValue("serein_getSysinfo",
                new Func<object>(() => SystemInfo.Info ?? OperatingSystemInfo.GetOperatingSystemInfo()));
#if !UNIX
            engine.SetValue("serein_getCPUUsage",
                new Func<float>(() => SystemInfo.CPUUsage));
#else
            engine.SetValue("serein_getCPUUsage", JsValue.Undefined);
#endif
#if CONSOLE
            engine.SetValue("serein_type", 0);
#elif WINFORM
            engine.SetValue("serein_type", 1);
#elif WPF
            engine.SetValue("serein_type", 2);
#else
            engine.SetValue("serein_type", -1);
#endif
            engine.SetValue("serein_getNetSpeed",
                new Func<Array>(() => new[] { SystemInfo.UploadSpeed, SystemInfo.DownloadSpeed }));
            engine.SetValue("serein_runCommand",
                new Action<string>((command) => Command.Run(5, command)));
            engine.SetValue("serein_getSettings",
                new Func<string>(() => JsonConvert.SerializeObject(Global.Settings)));
            engine.SetValue("serein_getSettingsObject",
                new Func<object>(() => Global.Settings));
            engine.SetValue("serein_getMotdpe",
                new Func<string, string>((addr) => new Motdpe(addr).Origin));
            engine.SetValue("serein_getMotdje",
                new Func<string, string>((addr) => new Motdje(addr).Origin));
            engine.SetValue("serein_startServer",
                new Func<bool>(() => ServerManager.Start(true)));
            engine.SetValue("serein_stopServer",
                new Action(() => ServerManager.Stop(true)));
            engine.SetValue("serein_killServer",
                new Func<bool>(() => ServerManager.Kill(true)));
            engine.SetValue("serein_getServerStatus",
                new Func<bool>(() => ServerManager.Status));
            engine.SetValue("serein_sendCmd",
                new Action<string, bool>((commnad, usingUnicode) => ServerManager.InputCommand(commnad, usingUnicode, false)));
            engine.SetValue("serein_getServerTime",
                new Func<string>(() => ServerManager.Time));
            engine.SetValue("serein_getServerCPUUsage",
                new Func<double>(() => ServerManager.CPUUsage));
            engine.SetValue("serein_getServerFile",
                new Func<string>(() => ServerManager.StartFileName));
            engine.SetValue("serein_sendGroup",
                new Func<long, string, bool>((target, message) => Websocket.Send(false, message, target)));
            engine.SetValue("serein_sendPrivate",
                new Func<long, string, bool>((target, message) => Websocket.Send(true, message, target)));
            engine.SetValue("serein_sendPacket",
                new Func<string, bool>((message) => Websocket.Send(message)));
            engine.SetValue("serein_getWsStatus",
                new Func<bool>(() => Websocket.Status));
            engine.SetValue("serein_bindMember",
                new Func<long, string, bool>(Binder.Bind));
            engine.SetValue("serein_unbindMember",
                new Func<long, bool>(Binder.UnBind));
            engine.SetValue("serein_getID",
                new Func<string, long>(Binder.GetID));
            engine.SetValue("serein_getGameID",
                new Func<long, string>(Binder.GetGameID));
            engine.SetValue("serein_getGroupCache",
                new Func<Dictionary<string, Dictionary<string, string>>>(() => JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(Global.GroupCache.ToJson())));
            engine.SetValue("serein_getUserName",
                new Func<long, long, string>((groupID, userID) => Global.GroupCache.TryGetValue(groupID, out Dictionary<long, Member> groupinfo) && groupinfo.TryGetValue(userID, out Member member) ? member.ShownName : string.Empty));
            engine.SetValue("serein_getPluginList",
                new Func<List<dynamic>>(() => JsonConvert.DeserializeObject<List<dynamic>>(JSPluginManager.PluginDict.Values.ToJson())));
            engine.SetValue("serein_getRegexs",
                new Func<List<Regex>>(() => Global.RegexList));
            engine.SetValue("serein_addRegex",
                new Func<string, int, bool, string, string, bool>(JSFunc.AddRegex));
            engine.SetValue("serein_editRegex",
                new Func<int, string, int, bool, string, string, bool>(JSFunc.EditRegex));
            engine.SetValue("serein_removeRegex",
                new Func<int, bool>(JSFunc.RemoveRegex));
            engine.SetValue("getMD5",
                new Func<string, string>(JSFunc.GetMD5));
            engine.SetValue("Motdpe",
                TypeReference.CreateTypeReference(engine, typeof(Motdpe)));
            engine.SetValue("Motdje",
                TypeReference.CreateTypeReference(engine, typeof(Motdje)));
            engine.Execute(
                @"var serein = {
                    log: serein_log,
                    type: serein_type,
                    path: serein_path,
                    namespace: serein_namespace,
                    version: serein_version,
                    getSettings: serein_getSettings,
                    getSettingsObject: serein_getSettingsObject,
                    debugLog: serein_debugLog,
                    runCommand: serein_runCommand,
                    registerPlugin: serein_registerPlugin,
                    setListener: serein_setListener,
                    getCPUUsage: serein_getCPUUsage,
                    getNetSpeed: serein_getNetSpeed,
                    getSysInfo: serein_getSysinfo,
                    getMotdpe: serein_getMotdpe,
                    getMotdje: serein_getMotdje,
                    startServer: serein_startServer,
                    stopServer: serein_stopServer,
                    sendCmd: serein_sendCmd,
                    killServer: serein_killServer,
                    getServerStatus: serein_getServerStatus,
                    getServerTime: serein_getServerTime,
                    getServerCPUUsage: serein_getServerCPUUsage,
                    getServerFile: serein_getServerFile,
                    sendGroup: serein_sendGroup,
                    sendPrivate: serein_sendPrivate,
                    sendPacket: serein_sendPacket,
                    getWsStatus: serein_getWsStatus,
                    bindMember: serein_bindMember,
                    unbindMember: serein_unbindMember,
                    getID: serein_getID,
                    getGameID: serein_getGameID,
                    getGroupCache: serein_getGroupCache,
                    getPluginList: serein_getPluginList,
                    getUserName: serein_getUserName,
                    loadFrom: require,
                    getRegexs: serein_getRegexs,
                    addRegex: serein_addRegex,
                    editRegex: serein_editRegex,
                    removeRegex: serein_removeRegex,
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
                engine.Execute(code);
                exceptionMessage = string.Empty;
            }
            catch (JavaScriptException e)
            {
                Logger.Output(LogType.Debug, e);
                exceptionMessage = $"{e.Message}\n{e.JavaScriptStackTrace}";
            }
            catch (Exception e)
            {
                Logger.Output(LogType.Debug, e);
                exceptionMessage = e.Message;
            }
            return engine;
        }
    }
}
