using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Octokit;
using Serein.Core.Models;
using Serein.Core.Services.Data;
using Serein.Core.Utils;

namespace Serein.Core.Services.Network;

public sealed class UpdateChecker : NotifyPropertyChangedModelBase
{
    private bool _listened;
    private readonly SereinApp _sereinApp;
    private readonly ILogger _logger;
    private readonly System.Timers.Timer _timer;
    private readonly SettingProvider _settingProvider;
    private readonly GitHubClient _gitHubClient;
    private readonly HttpClient _httpClient;
    private static readonly Version Version =
        typeof(UpdateChecker).Assembly.GetName().Version ?? new();

    public Release? LastResult { get; private set; }
    public Release? Latest { get; private set; }
    public bool IsReadyToReplace { get; private set; }
    public event EventHandler? Updated;
    public event EventHandler? Prepared;

    public UpdateChecker(
        SereinApp sereinApp,
        ILogger<UpdateChecker> logger,
        SettingProvider settingProvider,
        CancellationTokenProvider cancellationTokenProvider
    )
    {
        _sereinApp = sereinApp;
        _logger = logger;
        _settingProvider = settingProvider;
        _gitHubClient = new(
            new ProductHeaderValue($"Serein.{sereinApp.Type}", sereinApp.Version.ToString())
        );
        _timer = new(1000 * 60 * 10);
        _timer.Elapsed += async (_, _) =>
        {
            if (_settingProvider.Value.Application.CheckUpdate)
            {
                await CheckAsync();
            }
        };

        _httpClient = new();

        cancellationTokenProvider.Token.Register(Stop);
    }

    public async Task CheckAsync()
    {
        _logger.LogDebug("开始获取更新");
        try
        {
            var release = await _gitHubClient.Repository.Release.GetLatest("SereinDev", "Serein");
            var version = new Version(release.TagName.TrimStart('v'));

            if (release.TagName == LastResult?.TagName)
            {
                return;
            }

            LastResult = release;

            _logger.LogDebug("获取到最新版本：{}", release.TagName);
            _logger.LogDebug("Body='{}'", release.Body);

            if (version > Version)
            {
                Latest = release;
                Updated?.Invoke(this, EventArgs.Empty);

                if (_settingProvider.Value.Application.AutoUpdate)
                {
                    UpdateAsync(release);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "获取更新失败");
        }
        finally
        {
            _logger.LogDebug("获取更新结束");
        }
    }

    public async Task StartAsync()
    {
        _timer.Start();

        if (_settingProvider.Value.Application.CheckUpdate)
        {
            await CheckAsync();
        }
    }

    private async Task DownloadAsync(string path, Release release)
    {
        var asset = release.Assets.FirstOrDefault(
            (asset) =>
                asset.Name.EndsWith(".zip")
                && asset.Name.Contains("win", StringComparison.InvariantCultureIgnoreCase)
                && asset.Name.Contains(
                    _sereinApp.Version.ToString(),
                    StringComparison.InvariantCultureIgnoreCase
                )
        );

        if (asset is null)
        {
            _logger.LogWarning("未找到适用于当前应用的发布包");
            return;
        }

        var response = await _httpClient.GetAsync(
            asset.BrowserDownloadUrl,
            HttpCompletionOption.ResponseHeadersRead
        );
        response.EnsureSuccessStatusCode();

        ZipFile.ExtractToDirectory(await response.Content.ReadAsStreamAsync(), path, true);
    }

    private async Task UpdateAsync(Release release)
    {
        var path = Path.Join(PathConstants.UpdateCache, release.TagName);
        Directory.CreateDirectory(path);
        IsReadyToReplace = false;

        if (Directory.GetFiles(path).Length == 0)
        {
            try
            {
                await DownloadAsync(path, release);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "下载更新失败");
                return;
            }
        }

        IsReadyToReplace = true;

        if (Environment.OSVersion.Platform != PlatformID.Win32NT)
        {
            _logger.LogWarning("仅有在Windows系统下才支持自动替换更新");
            _logger.LogWarning("你可以在Serein退出后手动复制更新文件（{}）到当前目录", path);
        }
        else if (!_listened)
        {
            AppDomain.CurrentDomain.ProcessExit += (_, _) =>
                Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = $"/c xcopy {path.Replace('/', '\\')} /Y & pause",
                    }
                );
            _listened = true;

            _logger.LogInformation("更新已下载完毕。退出Serein即可自动更新");
            Prepared?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Stop()
    {
        _timer.Stop();
        _timer.Dispose();
        _httpClient.Dispose();
    }
}
