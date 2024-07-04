using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Jint;
using Jint.Runtime.Interop;

using Microsoft.Extensions.Logging;

using MineStatLib;

using Serein.Core.Models.Commands;
using Serein.Core.Models.Output;
using Serein.Core.Models.Plugins.Js;
using Serein.Core.Services.Data;
using Serein.Core.Utils;

namespace Serein.Core.Services.Plugins.Js;

public class JsEngineFactory(
    SettingProvider settingProvider,
    IPluginLogger pluginLogger,
    ILogger logger
)
{
    private readonly SettingProvider _settingProvider = settingProvider;
    private readonly IPluginLogger _pluginLogger = pluginLogger;
    private readonly ILogger _logger = logger;

    public Options CreateOptions(JsPlugin jsPlugin)
    {
        var assemblies = new List<Assembly>();
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
                    jsPlugin.PluginInfo.Name,
                    $"加载所需程序集“{assemblyName}”时出现异常：\n" + e.Message
                );
                _logger.LogDebug(e, "[{}] 加载所需程序集“{}”时出现异常", jsPlugin.PluginInfo.Name, assemblyName);
            }
        }

        var cfg = new Options
        {
            Modules = { RegisterRequire = true },
            Interop =
            {
                AllowGetType = jsPlugin.Config.AllowGetType,
                AllowOperatorOverloading = jsPlugin.Config.AllowOperatorOverloading,
                AllowSystemReflection = jsPlugin.Config.AllowSystemReflection,
                AllowWrite = jsPlugin.Config.AllowWrite,
                AllowedAssemblies = assemblies,
                ExceptionHandler = (_) => true
            },
            StringCompilationAllowed = jsPlugin.Config.StringCompilationAllowed,
            Strict = jsPlugin.Config.Strict
        };

        cfg.CatchClrExceptions();
        cfg.CancellationToken(jsPlugin.CancellationToken);
        cfg.EnableModules(PathConstants.PluginDirectory);

        return cfg;
    }

    public Engine Create(JsPlugin jsPlugin)
    {
        var engine = new Engine(CreateOptions(jsPlugin));

        engine.SetValue("serein", jsPlugin.ScriptInstance);
        engine.SetValue("console", jsPlugin.Console);

        AddTypeReference<Command>();
        AddTypeReference<CommandOrigin>();
        AddTypeReference<MatchFieldType>();
        AddTypeReference<Match>();
        AddTypeReference<Schedule>();

        AddTypeReference<MineStat>();
        AddTypeReference<ConnStatus>();
        AddTypeReference<SlpProtocol>();

        return engine;

        void AddTypeReference<T>()
        {
            engine.SetValue(typeof(T).ToString(), TypeReference.CreateTypeReference<T>(engine));
        }
    }
}
