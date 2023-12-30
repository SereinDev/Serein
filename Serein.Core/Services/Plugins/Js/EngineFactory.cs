using Jint;

using Serein.Core.Models.Plugins.Js;

namespace Serein.Core.Services.Plugins.Js;

public static class EngineFactory
{
    public static Engine Create(
        string @namespace,
        ScriptInstance scriptInstance,
        PreLoadConfig preLoadConfig
    )
    {
        var engine = new Engine();
        engine.SetValue("serein", scriptInstance);

        return engine;
    }
}
