using System;
using System.Threading.Tasks;
using System.Timers;

using Microsoft.Extensions.Logging;

using Octokit;

using Serein.Core.Models.Network;
using Serein.Core.Models.Output;
using Serein.Core.Services.Data;

namespace Serein.Core.Services.Networks;

public class UpdateChecker
{
    private readonly ISereinLogger _logger;
    private readonly Timer _timer;
    private readonly SettingProvider _settingProvider;
    private readonly GitHubClient _gitHubClient;
    private static readonly Version Version =
        typeof(UpdateChecker).Assembly.GetName().Version ?? new Version();

    public UpdateChecker(ISereinLogger logger, SettingProvider settingProvider)
    {
        _logger = logger;
        _settingProvider = settingProvider;
        _gitHubClient = new(new ProductHeaderValue($"Serein.{SereinApp.Type}"));
        _timer = new(120_000);
        _timer.Elapsed += async (_, _) =>
        {
            if (_settingProvider.Value.Application.CheckUpdate)
                await Check();
        };
    }

    public void Start()
    {
        _timer.Start();
    }

    public event EventHandler<UpdateEventArgs>? Updated;

    public async Task Check()
    {
        var release = await _gitHubClient.Repository.Release.GetLatest("SereinDev", "Serein");
        var version = new Version(release.TagName.TrimStart('v'));

        if (version <= Version)
            return;

        _logger.LogDebug("获取到新版本：{}", release.TagName);
        Updated?.Invoke(this, new(release));
    }
}
