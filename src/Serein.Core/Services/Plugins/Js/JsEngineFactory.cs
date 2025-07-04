using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Jint;
using Jint.Native;
using Jint.Runtime.Interop;
using Microsoft.Extensions.Logging;
using Serein.ConnectionProtocols.Models;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Commands;
using Serein.Core.Models.Network.Connection;
using Serein.Core.Services.Data;
using Serein.Core.Services.Plugins.Js.BuiltInModules;
using Serein.Core.Services.Plugins.Storages;

namespace Serein.Core.Services.Plugins.Js;

public sealed class JsEngineFactory(
    SettingProvider settingProvider,
    LocalStorage localStorage,
    SessionStorage sessionStorage,
    PluginLoggerBase pluginLogger,
    ILogger<JsEngineFactory> logger
)
{
    public FileSystem FileSystem { get; } = new();
    public Process Process { get; } = new();

    private readonly ILogger _logger = logger;

    private Options PrepareOptions(JsPlugin jsPlugin)
    {
        var assemblies = new List<Assembly>
        {
            typeof(Console).Assembly, // System
            typeof(SereinApp).Assembly, // Serein.Core
            typeof(Protocol).Assembly, // Serein.ConnectionProtocols
        };

        var entry = Assembly.GetEntryAssembly();

        if (entry is not null)
        {
            assemblies.Add(entry);
        }

        foreach (
            var assemblyName in jsPlugin.Config.NetAssemblies.Concat(
                settingProvider.Value.Application.JSGlobalAssemblies
            )
        )
        {
            try
            {
                assemblies.Add(Assembly.Load(assemblyName));
            }
            catch (Exception e)
            {
                pluginLogger.Log(
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
                SerializeToJson = (obj) => JsonSerializer.Serialize(obj),
            },
            Host = { StringCompilationAllowed = jsPlugin.Config.AllowStringCompilation },
            Strict = jsPlugin.Config.Strict,
            ExperimentalFeatures = ExperimentalFeature.All,
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

        engine.SetValue("fs", FileSystem);
        engine.SetValue("process", Process);
        engine.SetValue("localStorage", localStorage);
        engine.SetValue("sessionStorage", sessionStorage);

        engine.SetValue("window", JsValue.Undefined);
        engine.SetValue("exports", JsValue.Undefined);

        engine.SetValue("__filename", Path.GetFullPath(jsPlugin.FileName));
        engine.SetValue("__dirname", Path.GetDirectoryName(Path.GetFullPath(jsPlugin.FileName)));

        engine.SetValue("setTimeout", jsPlugin.TimerFactory.SetTimeout);
        engine.SetValue("setInterval", jsPlugin.TimerFactory.SetInterval);
        engine.SetValue("clearTimeout", jsPlugin.TimerFactory.ClearTimeout);
        engine.SetValue("clearInterval", jsPlugin.TimerFactory.ClearInterval);

        AddTypeReference<Command>();
        AddTypeReference<CommandOrigin>();
        AddTypeReference<AppType>();
        AddTypeReference<ReactionType>();
        AddTypeReference<TargetType>();

        return engine;

        void AddTypeReference<T>()
        {
            engine.SetValue(typeof(T).Name, TypeReference.CreateTypeReference<T>(engine));
        }
    }
}
