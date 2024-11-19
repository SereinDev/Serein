using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Octokit;

using Serein.Core.Services.Data;

namespace Serein.Core.Services.Network;

public sealed class UpdateChecker
{
    private readonly ILogger _logger;
    private readonly System.Timers.Timer _timer;
    private readonly SettingProvider _settingProvider;
    private readonly GitHubClient _gitHubClient;
    private static readonly Version Version =
        typeof(UpdateChecker).Assembly.GetName().Version ?? new();

    private Release? _last;
    public Release? Newest { get; private set; }
    public event EventHandler? Updated;

    public UpdateChecker(ILogger<UpdateChecker> logger, SettingProvider settingProvider)
    {
        _logger = logger;
        _settingProvider = settingProvider;
        _gitHubClient = new(new ProductHeaderValue($"Serein.{SereinApp.Type}", SereinApp.Version));
        _timer = new(1000 * 60 * 10);
        _timer.Elapsed += async (_, _) =>
        {
            if (_settingProvider.Value.Application.CheckUpdate)
            {
                await CheckAsync();
            }
        };
    }

    public async Task<Release?> CheckAsync()
    {
        _logger.LogDebug("开始获取更新");
        try
        {
            var release = await _gitHubClient.Repository.Release.GetLatest("SereinDev", "Serein");
            var version = new Version(release.TagName.TrimStart('v'));

            if (release.TagName == _last?.TagName)
            {
                return release;
            }

            _last = release;

            _logger.LogDebug("获取到最新版本：{}", release.TagName);
            _logger.LogDebug("Body='{}'", release.Body);

            if (version > Version)
            {
                Newest = release;
                Updated?.Invoke(this, EventArgs.Empty);
            }

            return release;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "获取更新失败");
        }
        finally
        {
            _logger.LogDebug("获取更新结束");
        }

        return null;
    }

    public async Task StartAsync()
    {
        _timer.Start();

        if (_settingProvider.Value.Application.CheckUpdate)
        {
            await CheckAsync();
        }
    }
}
