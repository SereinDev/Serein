using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jint;
using Jint.Native;
using Jint.Runtime;
using Jint.Runtime.Interop;
using Newtonsoft.Json;
using Serein.Base;
using Serein.Base.Motd;
using Serein.Core.Generic;
using Serein.Core.JSPlugin.Native;
using Serein.Core.JSPlugin.Permission;
using Serein.Core.Server;
using Serein.Extensions;
using Serein.Utils;
using System;
using System.Reflection;
using System.Threading;
using SystemInfoLibrary.OperatingSystem;

namespace Serein.Core.JSPlugin
{
    internal static class JSEngine
    {
        /// <summary>
        /// 初始化JS引擎
        /// </summary>
        /// <returns>JS引擎</returns>
        public static Engine Init() => Create(true, null, null, null);

        /// <summary>
        /// 初始化JS引擎
        /// </summary>
        /// <param name="isExecuteByCommand">被命令执行</param>
        /// <param name="namespace">命名空间</param>
        /// <param name="cancellationTokenSource">取消Token</param>
        /// <returns>JS引擎</returns>
        public static Engine Create(bool isExecuteByCommand, string @namespace, CancellationTokenSource cancellationTokenSource, PreLoadConfig preLoadConfig)
        {
            Engine engine = new(
                new Action<Options>((cfg) =>
                {
                    cfg.CatchClrExceptions();
                    cfg.Interop.MemberAccessor = MemberAccessor;
                    if (!isExecuteByCommand)
                    {
                        List<Assembly> assemblies = new();
                        foreach (string assemblyString in preLoadConfig.Assemblies ?? Array.Empty<string>())
                        {
                            try
                            {
                                assemblies.Add(Assembly.Load(assemblyString));
                            }
                            catch (Exception e)
                            {
                                Utils.Logger.Output(LogType.Plugin_Warn, $"加载程序集“{assemblyString}”时出现异常：", e.Message);
                            }
                        }

                        cfg.AllowClr(assemblies.ToArray());
                        cfg.CancellationToken(cancellationTokenSource.Token);
                        cfg.EnableModules(Path.Combine(Global.PATH, "plugins"));

                        cfg.Modules.RegisterRequire = true;
                        cfg.Interop.AllowGetType = preLoadConfig.AllowGetType;
                        cfg.Interop.AllowOperatorOverloading = preLoadConfig.AllowOperatorOverloading;
                        cfg.Interop.AllowSystemReflection = preLoadConfig.AllowSystemReflection;
                        cfg.Interop.AllowWrite = preLoadConfig.AllowWrite;
                        // cfg.StringCompilationAllowed = preLoadConfig.StringCompilationAllowed;
                        cfg.Strict = preLoadConfig.Strict;
                        cfg.Interop.ExceptionHandler = (_) => true;
                    }
                    else
                    {
                        cfg.TimeoutInterval(TimeSpan.FromMinutes(1));
                    }
                }
                ));
            engine.SetValue("serein_path",
                Global.PATH);
            engine.SetValue("serein_version",
                Global.VERSION);
            engine.SetValue("serein_namespace",
                string.IsNullOrEmpty(@namespace) ? JsValue.Null : @namespace);
            engine.SetValue("serein_debugLog",
                new Action<JsValue>((content) => Utils.Logger.Output(LogType.Debug, $"[{@namespace ?? "unknown"}]", content)));
            if (!string.IsNullOrEmpty(@namespace))
            {
                engine.SetValue("serein_log",
                    new Action<JsValue>((content) => Utils.Logger.Output(LogType.Plugin_Info, $"[{@namespace}]", content)));
                engine.SetValue("serein_registerPlugin",
                    new Func<string, string, string, string, string>((name, version, author, description) => JSFunc.Register(@namespace, name, version, author, description)));
                engine.SetValue("serein_setListener",
                    new Func<string, JsValue, bool>((eventName, callback) => JSFunc.SetListener(@namespace, eventName, callback)));
                engine.SetValue("serein_setVariable",
                    new Func<string, JsValue, bool>(JSFunc.SetVariable));
                engine.SetValue("serein_export",
                    new Action<string, JsValue>(JSFunc.Export));
                engine.SetValue("serein_setPreLoadConfig",
                    new Action<JsValue, JsValue, JsValue, JsValue, JsValue, JsValue, JsValue>((assemblies, allowGetType, allowOperatorOverloading, allowSystemReflection, allowWrite, strict, stringCompilationAllowed) => JSFunc.SetPreLoadConfig(@namespace, assemblies, allowGetType, allowOperatorOverloading, allowSystemReflection, allowWrite, strict, stringCompilationAllowed)));
                engine.SetValue("setTimeout",
                    new Func<JsValue, JsValue, JsValue>((callback, interval) => JSFunc.SetTimer(@namespace, callback, interval, false)));
                engine.SetValue("setInterval",
                    new Func<JsValue, JsValue, JsValue>((callback, interval) => JSFunc.SetTimer(@namespace, callback, interval, true)));
                engine.SetValue("clearTimeout",
                    new Func<JsValue, bool>(JSFunc.ClearTimer));
                engine.SetValue("clearInterval",
                    new Func<JsValue, bool>(JSFunc.ClearTimer));
                engine.SetValue("WSClient",
                    TypeReference.CreateTypeReference(engine, typeof(WSClient)));
                engine.SetValue("Logger",
                    TypeReference.CreateTypeReference(engine, typeof(Native.Logger)));
            }
            else
            {
                engine.SetValue("serein_log", JsValue.Undefined);
                engine.SetValue("serein_registerPlugin", JsValue.Undefined);
                engine.SetValue("serein_setListener", JsValue.Undefined);
                engine.SetValue("serein_setVariable", JsValue.Undefined);
                engine.SetValue("serein_export", JsValue.Undefined);
                engine.SetValue("serein_setPreLoadConfig", JsValue.Undefined);
                engine.SetValue("setTimeout", JsValue.Undefined);
                engine.SetValue("setInterval", JsValue.Undefined);
                engine.SetValue("clearTimeout", JsValue.Undefined);
                engine.SetValue("clearInterval", JsValue.Undefined);
                engine.SetValue("WSClient", JsValue.Undefined);
                engine.SetValue("Logger", JsValue.Undefined);
                engine.SetValue("require", JsValue.Undefined);
            }
            engine.SetValue("serein_getSysinfo",
                new Func<object>(() => SystemInfo.Info ?? OperatingSystemInfo.GetOperatingSystemInfo()));
            engine.SetValue("serein_getCPUUsage",
                new Func<float>(() => SystemInfo.CPUUsage));
#if CONSOLE
            engine.SetValue("serein_type", 0);
#elif WINFORM
            engine.SetValue("serein_type", 1);
#elif WPF
            engine.SetValue("serein_type", 2);
#else
            engine.SetValue("serein_type", -1);
#endif
            engine.SetValue("serein_typeName", Global.TYPE);
            engine.SetValue("serein_startTime", Global.StartTime);
            engine.SetValue("serein_getNetSpeed",
                new Func<Array>(() => new[] { SystemInfo.UploadSpeed, SystemInfo.DownloadSpeed }));
            engine.SetValue("serein_runCommand",
                new Action<string>((command) => Command.Run(CommandOrigin.Javascript, command)));
            engine.SetValue("serein_getSettings",
                new Func<string>(() => JsonConvert.SerializeObject(Global.Settings)));
            engine.SetValue("serein_getSettingsObject",
                new Func<JsValue>(() => JsValue.FromObject(engine, Global.Settings)));
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
            engine.SetValue("serein_getServerMotd",
                new Func<JsValue>(() => JsValue.FromObject(engine, ServerManager.Motd)));
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
            engine.SetValue("serein_sendTemp",
                new Func<long, long, string, bool>((groupID, userID, message) => Websocket.Send(groupID, userID, message)));
            engine.SetValue("serein_sendPacket",
                new Func<string, bool>((message) => Websocket.Send(message)));
            engine.SetValue("serein_getWsStatus",
                new Func<bool>(() => Websocket.Status));
            engine.SetValue("serein_bindMember",
                new Func<long, string, bool>(Generic.Binder.Bind));
            engine.SetValue("serein_unbindMember",
                new Func<long, bool>(Generic.Binder.UnBind));
            engine.SetValue("serein_getID",
                new Func<string, long>(Generic.Binder.GetID));
            engine.SetValue("serein_getGameID",
                new Func<long, string>(Generic.Binder.GetGameID));
            engine.SetValue("serein_getGroupCache",
                new Func<JsValue>(() => JsValue.FromObject(engine, JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Member>>>(Global.GroupCache.ToJson()))));
            engine.SetValue("serein_getUserInfo",
                new Func<long, long, JsValue>((groupID, userID) => Global.GroupCache.TryGetValue(groupID, out Dictionary<long, Member> groupinfo) && groupinfo.TryGetValue(userID, out Member member) ? JsValue.FromObject(engine, member) : JsValue.Null));
            engine.SetValue("serein_getPluginList",
                new Func<JsValue>(() => JsValue.FromObject(engine, JSPluginManager.PluginDict.Values.Select(plugin => new PluginStruct(plugin)).ToArray())));
            engine.SetValue("serein_getRegexes",
                new Func<JsValue>(() => JsValue.FromObject(engine, Global.RegexList.ToArray())));
            engine.SetValue("serein_addRegex",
                new Func<string, int, bool, string, string, long[], bool>(JSFunc.AddRegex));
            engine.SetValue("serein_editRegex",
                new Func<int, string, int, bool, string, string, long[], bool>(JSFunc.EditRegex));
            engine.SetValue("serein_removeRegex",
                new Func<int, bool>(JSFunc.RemoveRegex));
            engine.SetValue("serein_reloadFiles",
                new Func<string, bool>((type) => JSFunc.ReloadFiles(@namespace, type)));
            engine.SetValue("serein_import",
                new Func<string, JsValue>((key) => JSPluginManager.VariablesExportedDict.TryGetValue(key, out JsValue jsValue) ? jsValue : JsValue.Undefined));
            engine.SetValue("serein_getPermissionGroups",
                new Func<JsValue>(() => JsValue.FromObject(engine, Global.PermissionGroups)));
            engine.SetValue("serein_addPermissionGroup",
                new Func<string, PermissionGroup, bool, bool>(PermissionManager.Add));
            engine.SetValue("serein_removePermissionGroup",
                new Func<string, bool>(PermissionManager.Remove));
            engine.SetValue("serein_calculatePermission",
                new Func<string, long, long?, JsValue>((type, userId, groupId) => JsValue.FromObject(engine, PermissionManager.Calculate(type, userId, groupId ?? -1))));
            engine.SetValue("serein_existPermissionGroup",
                new Func<string, bool>(Global.PermissionGroups.ContainsKey));
            engine.SetValue("serein_setPermission",
                new Func<string, string, JsValue, bool>(PermissionManager.SetPermission));
            engine.SetValue("serein_safeCall",
                new Func<JsValue, JsValue[], JsValue>(JSFunc.SafeCall));
            engine.SetValue("Motdpe",
                TypeReference.CreateTypeReference(engine, typeof(Motdpe)));
            engine.SetValue("Motdje",
                TypeReference.CreateTypeReference(engine, typeof(Motdje)));
            engine.Execute(
                @"const serein = {
                    path:               serein_path,
                    type:               serein_type,
                    typeName:           serein_typeName,
                    version:            serein_version,
                    startTime:          serein_startTime,
                    namespace:          serein_namespace,

                    getSettings:        serein_getSettings,
                    getSettingsObject:  serein_getSettingsObject,
                    log:                serein_log,
                    debugLog:           serein_debugLog,
                    runCommand:         serein_runCommand,
                    registerPlugin:     serein_registerPlugin,
                    setListener:        serein_setListener,
                    getPluginList:      serein_getPluginList,
                    setVariable:        serein_setVariable,
                    setPreLoadConfig:   serein_setPreLoadConfig,
                    reloadFiles:        serein_reloadFiles,
                    safeCall:           serein_safeCall,

                    getRegexes:         serein_getRegexes,
                    addRegex:           serein_addRegex,
                    editRegex:          serein_editRegex,
                    removeRegex:        serein_removeRegex,

                    import:             serein_import,
                    imports:            serein_import,
                    export:             serein_export,
                    exports:            serein_export,

                    getCPUUsage:        serein_getCPUUsage,
                    getNetSpeed:        serein_getNetSpeed,
                    getSysInfo:         serein_getSysinfo,

                    startServer:        serein_startServer,
                    stopServer:         serein_stopServer,
                    sendCmd:            serein_sendCmd,
                    killServer:         serein_killServer,
                    getServerStatus:    serein_getServerStatus,
                    getServerTime:      serein_getServerTime,
                    getServerCPUUsage:  serein_getServerCPUUsage,
                    getServerFile:      serein_getServerFile,
                    getServerMotd:      serein_getServerMotd,
                    getMotdpe:          serein_getMotdpe,
                    getMotdje:          serein_getMotdje,

                    sendGroup:          serein_sendGroup,
                    sendPrivate:        serein_sendPrivate,
                    sendTemp:           serein_sendTemp,
                    sendPacket:         serein_sendPacket,
                    getWsStatus:        serein_getWsStatus,
                    getGroupCache:      serein_getGroupCache,
                    getUserInfo:        serein_getUserInfo,

                    bindMember:         serein_bindMember,
                    unbindMember:       serein_unbindMember,
                    getID:              serein_getID,
                    getGameID:          serein_getGameID,

                    getPermissionGroups:    serein_getPermissionGroups,
                    addPermissionGroup:     serein_addPermissionGroup,
                    removePermissionGroup:  serein_removePermissionGroup,
                    calculatePermission:    serein_calculatePermission,
                    existPermissionGroup:   serein_existPermissionGroup,
                    setPermission:          serein_setPermission,
                };"
            );
            return engine;
        }

        /// <summary>
        /// 运行代码
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns>错误信息</returns>
        public static void Run(this Engine engine, string code, out string exceptionMessage)
        {
            try
            {
                engine.Execute(code);
                exceptionMessage = string.Empty;
            }
            catch (JavaScriptException e)
            {
                Utils.Logger.Output(LogType.Debug, e);
                exceptionMessage = $"{e.Message}\n{e.JavaScriptStackTrace}";
            }
            catch (Exception e)
            {
                Utils.Logger.Output(LogType.Debug, e);
                exceptionMessage = e.Message;
            }
        }

        public static JsValue MemberAccessor(Engine engine, object target, string member)
        {
            if (target is null || string.IsNullOrEmpty(member) || member.Length == 0)
            {
                return null;
            }
            string pascalCasingName = GetPascalCasingName(member);
            Type type = target.GetType();
            MemberInfo[] memberInfos = type.GetMembers(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
            foreach (MemberInfo memberInfo in memberInfos)
            {
                if (pascalCasingName != memberInfo.Name && member != memberInfo.Name)
                {
                    continue;
                }
                switch (memberInfo.MemberType)
                {
                    case MemberTypes.Property:
                        return JsValue.FromObject(engine, (memberInfo as PropertyInfo).GetValue(target));
                    case MemberTypes.Field:
                        return JsValue.FromObject(engine, (memberInfo as FieldInfo).GetValue(target));
                    default:
                        return null; // 交给Jint（计算最佳匹配方法）
                }
            }
            return null;
        }

        /// <summary>
        /// 获取帕斯卡命名
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>转换结果</returns>
        private static string GetPascalCasingName(string name)
        {
            if (char.IsUpper(name, 0))
            {
                return name;
            }
            char[] chars = name.ToCharArray();
            chars[0] = char.ToUpperInvariant(chars[0]);
            return new string(chars);
        }
    }
}
