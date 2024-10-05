using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Sentry;
using Sentry.Profiling;

using Serein.Core.Utils;

namespace Serein.Core;

public partial class SereinApp
{
    public static readonly string Version =
     typeof(SereinApp).Assembly.GetName().Version?.ToString() ?? "¿¿¿";
    public static readonly string? FullVersion = typeof(SereinApp)
        .Assembly
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
        ?.InformationalVersion;
    public static readonly AppType Type;
    public static readonly bool StartForTheFirstTime = !File.Exists(PathConstants.SettingFile);
    public static SereinApp? Current { get; private set; }

    static SereinApp()
    {
        Type = Assembly.GetEntryAssembly()?.GetName().Name switch
        {
            "Serein.Cli" => AppType.Cli,
            "Serein.Lite" => AppType.Lite,
            "Serein.Plus" => AppType.Plus,
            _ => AppType.Unknown,
        };

        if (Type != AppType.Unknown)
            InitSentry();
    }

    private static void InitSentry()
    {
        SentrySdk.Init(options =>
        {
            options.Dsn = "https://69fd834cacda6b9bacefb7e4456faa21@o4507968791379968.ingest.us.sentry.io/4507968824082432";
            options.AutoSessionTracking = true;
            options.TracesSampleRate = 1;
            options.ProfilesSampleRate = 0.3;
            options.AddIntegration(new ProfilingIntegration(
                TimeSpan.FromMilliseconds(1000)
            ));

#if DEBUG
            options.Debug = true;
            options.Environment = "development";
#else
            options.Environment = "production";
#endif
        });

        AppDomain.CurrentDomain.UnhandledException += (_, e) => SentrySdk.CaptureException((Exception)e.ExceptionObject);
        TaskScheduler.UnobservedTaskException += (_, e) => SentrySdk.CaptureException(e.Exception);
    }
}
