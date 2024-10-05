using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Octokit;

using Serein.Core.Models;
using Serein.Core.Services.Data;

namespace Serein.Core.Services.Network;

public class UpdateChecker : NotifyPropertyChangedModelBase
{
    private readonly ILogger _logger;
    private readonly System.Timers.Timer _timer;
    private readonly SettingProvider _settingProvider;
    private readonly GitHubClient _gitHubClient;
    private static readonly Version Version =
        typeof(UpdateChecker).Assembly.GetName().Version ?? new();

    public Release? Latest { get; private set; }
    public event EventHandler? Updated;

    public UpdateChecker(ILogger logger, SettingProvider settingProvider)
    {
        _logger = logger;
        _settingProvider = settingProvider;
        _gitHubClient = new(new ProductHeaderValue($"Serein.{SereinApp.Type}"));
        _timer = new(1000 * 60 * 2);
        _timer.Elapsed += async (_, _) =>
        {
            if (_settingProvider.Value.Application.CheckUpdate)
                await CheckAsync();
        };
    }

    public async Task CheckAsync()
    {
        try
        {
            var release = await _gitHubClient.Repository.Release.GetLatest("SereinDev", "Serein");
            var version = new Version(release.TagName.TrimStart('v'));

            if (release.TagName == Latest?.TagName)
                return;

            Latest = release;

            if (version > Version)
            {
                _logger.LogDebug("获取到新版本：{}", Latest?.TagName);
                Updated?.Invoke(this, EventArgs.Empty);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "获取更新失败");
        }
    }

    public async Task StartAsync()
    {
        _timer.Start();

        if (_settingProvider.Value.Application.CheckUpdate)
            await CheckAsync();
    }
}
