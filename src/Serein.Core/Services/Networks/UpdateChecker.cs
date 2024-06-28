using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Timers;

using Microsoft.Extensions.Logging;

using Octokit;

using Serein.Core.Models.Network;
using Serein.Core.Services.Data;

namespace Serein.Core.Services.Networks;

public class UpdateChecker : INotifyPropertyChanged
{
    private readonly ILogger _logger;
    private readonly Timer _timer;
    private readonly SettingProvider _settingProvider;
    private readonly GitHubClient _gitHubClient;
    private static readonly Version Version =
        typeof(UpdateChecker).Assembly.GetName().Version ?? new Version();

    public Release? Latest { get; private set; }

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

    public void Start()
    {
        _timer.Start();

        if (_settingProvider.Value.Application.CheckUpdate)
            CheckAsync();
    }

    public event EventHandler? Updated;

    public async Task CheckAsync()
    {
        var release = await _gitHubClient.Repository.Release.GetLatest("SereinDev", "Serein");
        var version = new Version(release.TagName.TrimStart('v'));

        if (release.TagName == Latest?.TagName)
            return;

        Latest = release;

        if (version <= Version)
            Notify();
    }

    private void Notify()
    {
        _logger.LogDebug("获取到新版本：{}", Latest?.TagName);
        Updated?.Invoke(this, EventArgs.Empty);
    }

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
