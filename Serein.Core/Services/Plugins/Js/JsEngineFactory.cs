using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Jint;
using Jint.Runtime.Interop;

using MineStatLib;

using Serein.Core.Models;
using Serein.Core.Models.Commands;
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
            var assemblyName in jsPlugin.PreloadConfig.NetAssemblies.Concat(
                _settingProvider.Value.Function.JSGlobalAssemblies
            )
        )
        {
            try
            {
                assemblies.Add(Assembly.Load(assemblyName));
            }
            catch (Exception e)
            {
                _logger.LogPluginWarn(
                    jsPlugin.FileName,
                    $"加载程序集“{assemblyName}”时出现异常：" + e.Message
                );
            }
        }

        var cfg = new Options
        {
            Modules = { RegisterRequire = true },
            Interop =
            {
                AllowGetType = jsPlugin.PreloadConfig.AllowGetType,
                AllowOperatorOverloading = jsPlugin.PreloadConfig.AllowOperatorOverloading,
                AllowSystemReflection = jsPlugin.PreloadConfig.AllowSystemReflection,
                AllowWrite = jsPlugin.PreloadConfig.AllowWrite,
                AllowedAssemblies = assemblies,
                ExceptionHandler = (_) => true
            },
            StringCompilationAllowed = jsPlugin.PreloadConfig.StringCompilationAllowed,
            Strict = jsPlugin.PreloadConfig.Strict
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
