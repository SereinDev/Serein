using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Jint;
using Jint.Native;
using Jint.Runtime.Interop;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Commands;
using Serein.Core.Models.Network.Connection.OneBot.Messages;
using Serein.Core.Models.Output;
using Serein.Core.Models.Plugins.Js;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;
using Serein.Core.Services.Plugins.Js.BuiltInModules;
using Serein.Core.Services.Plugins.Storages;
using Serein.Core.Utils;

namespace Serein.Core.Services.Plugins.Js;

public sealed class JsEngineFactory(
    SettingProvider settingProvider,
    LocalStorage localStorage,
    SessionStorage sessionStorage,
    IPluginLogger pluginLogger,
    ILogger<JsEngineFactory> logger
)
{
    private readonly SettingProvider _settingProvider = settingProvider;
    private readonly LocalStorage _localStorage = localStorage;
    private readonly SessionStorage _sessionStorage = sessionStorage;
    private readonly IPluginLogger _pluginLogger = pluginLogger;
    private readonly ILogger _logger = logger;

    private Options PrepareOptions(JsPlugin jsPlugin)
    {
        var assemblies = new List<Assembly> { typeof(Console).Assembly };

        foreach (
            var assemblyName in jsPlugin.Config.NetAssemblies.Concat(
                _settingProvider.Value.Application.JSGlobalAssemblies
            )
        )
        {
            try
            {
                assemblies.Add(Assembly.Load(assemblyName));
            }
            catch (Exception e)
            {
                _pluginLogger.Log(
                    LogLevel.Warning,
                    jsPlugin.Info.Name,
                    $"加载所需程序集“{assemblyName}”时出现异常：\n" + e.Message
                );
                _logger.LogDebug(
                    e,
                    "[{}] 加载所需程序集“{}”时出现异常",
                    jsPlugin.Info.Name,
                    assemblyName
                );
            }
        }

        var cfg = new Options
        {
            Modules = { RegisterRequire = true },
            Interop =
            {
                Enabled = true,
                AllowGetType = jsPlugin.Config.AllowGetType,
                AllowOperatorOverloading = jsPlugin.Config.AllowOperatorOverloading,
                AllowSystemReflection = jsPlugin.Config.AllowSystemReflection,
                AllowWrite = jsPlugin.Config.AllowWrite,
                AllowedAssemblies = assemblies,
                ExceptionHandler = (_) => true,
            },
            StringCompilationAllowed = jsPlugin.Config.AllowStringCompilation,
            Strict = jsPlugin.Config.Strict,
        };

        cfg.CancellationToken(jsPlugin.CancellationToken);
        cfg.EnableModules(Path.GetDirectoryName(Path.GetFullPath(jsPlugin.FileName))!, false);

        return cfg;
    }

    internal Engine Create(JsPlugin jsPlugin)
    {
        var engine = new Engine(PrepareOptions(jsPlugin));

        engine.SetValue("serein", jsPlugin.ScriptInstance);
        engine.SetValue("console", jsPlugin.Console);
        engine.SetValue("localStorage", _localStorage);
        engine.SetValue("sessionStorage", _sessionStorage);

        engine.SetValue("window", JsValue.Undefined);
        engine.SetValue("exports", JsValue.Undefined);

        engine.SetValue("__filename", Path.GetFullPath(jsPlugin.FileName));
        engine.SetValue("__dirname", Path.GetDirectoryName(Path.GetFullPath(jsPlugin.FileName)));

        engine.SetValue("setTimeout", jsPlugin.TimerFactory.SetTimeout);
        engine.SetValue("setInterval", jsPlugin.TimerFactory.SetInterval);
        engine.SetValue("clearTimeout", jsPlugin.TimerFactory.ClearTimeout);
        engine.SetValue("clearInterval", jsPlugin.TimerFactory.ClearInterval);

        engine.SetValue("fs", TypeReference.CreateTypeReference(engine, typeof(FileSystem)));
        engine.SetValue("process", TypeReference.CreateTypeReference(engine, typeof(Process)));

        AddTypeReference<EncodingMap.EncodingType>();
        AddTypeReference<Command>();
        AddTypeReference<CommandOrigin>();
        AddTypeReference<AppType>();
        AddTypeReference<ReactionType>();
        AddTypeReference<Role>();
        AddTypeReference<SubType>();
        AddTypeReference<MessageFormat>();
        AddTypeReference<MessageType>();

        return engine;

        void AddTypeReference<T>()
        {
            engine.SetValue(typeof(T).Name, TypeReference.CreateTypeReference<T>(engine));
        }
    }
}
