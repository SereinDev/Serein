using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Sentry;
using Sentry.Profiling;

using Serein.Core.Services.Data;

namespace Serein.Core.Services;

internal class SentryReporter(ILogger<SentryReporter> logger, SettingProvider settingProvider)
{
    private readonly ILogger _logger = logger;
    private readonly SettingProvider _settingProvider = settingProvider;

    public void Initialize()
    {
        if (!_settingProvider.Value.Application.EnableSentry || SereinApp.Type == AppType.Unknown)
            return;

        SentrySdk.Init(options =>
        {
            options.Dsn =
                "https://69fd834cacda6b9bacefb7e4456faa21@o4507968791379968.ingest.us.sentry.io/4507968824082432";
            options.AutoSessionTracking = true;
            options.TracesSampleRate = 1;
            options.ProfilesSampleRate = 0.5;
            options.AddIntegration(new ProfilingIntegration(TimeSpan.FromMilliseconds(1000)));

#if DEBUG
            options.Debug = true;
            options.Environment = "development";
#else
            options.Environment = "production";
#endif
        });

        _logger.LogDebug("已启用Sentry");

        TaskScheduler.UnobservedTaskException += (_, e) => SentrySdk.CaptureException(e.Exception);
    }
}
