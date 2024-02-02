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

public class JsEngineFactory
{
    private readonly SettingProvider _settingProvider;
    private readonly IOutputHandler _logger;

    public JsEngineFactory(SettingProvider settingProvider, IOutputHandler logger)
    {
        _settingProvider = settingProvider;
        _logger = logger;
    }

    public Options CreateOptions(JsPlugin jsPlugin)
    {
        var assemblies = new List<Assembly>();
        foreach (
            var assemblyName in jsPlugin.Config.CSharpAssemblies.Concat(
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
                _logger.LogPlugin(
                    LogLevel.Warning,
                    jsPlugin.Name,
                    $"加载程序集“{assemblyName}”时出现异常：" + e.Message
                );
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

        AddTypeReference<Command>(engine);
        AddTypeReference<CommandOrigin>(engine);
        AddTypeReference<MatchFieldType>(engine);
        AddTypeReference<Match>(engine);
        AddTypeReference<Schedule>(engine);

        AddTypeReference<MineStat>(engine);
        AddTypeReference<ConnStatus>(engine);
        AddTypeReference<SlpProtocol>(engine);

        return engine;
    }

    private static void AddTypeReference<T>(Engine engine)
    {
        engine.SetValue(typeof(T).ToString(), TypeReference.CreateTypeReference<T>(engine));
    }
}
