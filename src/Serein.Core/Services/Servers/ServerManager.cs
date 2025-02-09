using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serein.Core.Models;
using Serein.Core.Models.Server;
using Serein.Core.Services.Commands;
using Serein.Core.Services.Data;
using Serein.Core.Services.Plugins;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Servers;

public partial class ServerManager
{
    [GeneratedRegex(@"^\w{3,}$")]
    private static partial Regex GetServerIdRegex();

    private static readonly Regex ServerId = GetServerIdRegex();
    private static readonly string[] Blacklist =
    [
        "CON",
        "PRN",
        "NUL",
        "AUX",
        "COM0",
        "COM1",
        "COM2",
        "COM3",
        "COM4",
        "COM5",
        "COM6",
        "COM7",
        "COM8",
        "COM9",
        "LPT0",
        "LPT1",
        "LPT2",
        "LPT3",
        "LPT4",
        "LPT5",
        "LPT6",
        "LPT7",
        "LPT8",
        "LPT9",
    ];

    public static void ValidateId(string? id)
    {
        if (string.IsNullOrEmpty(id) || !ServerId.IsMatch(id))
        {
            throw new ArgumentException("服务器Id格式不正确", nameof(id));
        }

        if (Blacklist.Contains(id.ToUpperInvariant()))
        {
            throw new ArgumentException("不能使用Windows的保留关键字作为Id", nameof(id));
        }
    }

    public IReadOnlyDictionary<string, Server> Servers => _servers;
    public event EventHandler<ServersUpdatedEventArgs>? ServersUpdated;

    private readonly ILogger<Server> _serverlogger;
    private readonly ILogger _logger;
    private readonly ILogger<LogWriter> _logWriterLogger;
    private readonly SereinApp _sereinApp;
    private readonly SettingProvider _settingProvider;
    private readonly EventDispatcher _eventDispatcher;
    private readonly ReactionTrigger _reactionManager;
    private readonly Matcher _matcher;
    private readonly Dictionary<string, Server> _servers = [];

    public ServerManager(
        ILogger<Server> serverlogger,
        ILogger<ServerManager> logger,
        ILogger<LogWriter> logWriterLogger,
        SereinApp sereinApp,
        Matcher matcher,
        SettingProvider settingProvider,
        EventDispatcher eventDispatcher,
        ReactionTrigger reactionManager
    )
    {
        _matcher = matcher;
        _serverlogger = serverlogger;
        _logger = logger;
        _logWriterLogger = logWriterLogger;
        _sereinApp = sereinApp;
        _settingProvider = settingProvider;
        _eventDispatcher = eventDispatcher;
        _reactionManager = reactionManager;

        LoadAll();
        AppDomain.CurrentDomain.UnhandledException += (_, _) => Task.Run(OnCrash);
    }

    public bool AnyRunning => _servers.Any(static (kv) => kv.Value.Status);

    public Server Add(string id, Configuration configuration)
    {
        ValidateId(id);

        var server = new Server(
            id,
            _serverlogger,
            _logWriterLogger,
            _sereinApp,
            _matcher,
            configuration,
            _settingProvider,
            _eventDispatcher,
            _reactionManager
        );
        _servers.Add(id, server);
        ServersUpdated?.Invoke(this, new(ServersUpdatedType.Added, id, server));

        Save(id, configuration);

        _logger.LogDebug("添加服务器：{}", id);
        return server;
    }

    public bool Remove(string id)
    {
        ValidateId(id);

        if (_servers.TryGetValue(id, out var server) && server.Status)
        {
            throw new InvalidOperationException("服务器仍在运行中");
        }

        if (!_servers.Remove(id) || server is null)
        {
            return false;
        }

        var path = string.Format(PathConstants.ServerConfigFile, id);
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        _logger.LogDebug("删除服务器：{}", id);
        ServersUpdated?.Invoke(this, new(ServersUpdatedType.Removed, id, server));

        return true;
    }

    public void SaveAll()
    {
        foreach (var (id, server) in Servers)
        {
            try
            {
                Save(id, server.Configuration);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "保存文件时出现异常");
            }
        }
    }

    private static void Save(string id, Configuration configuration)
    {
        ValidateId(id);

        var path = string.Format(PathConstants.ServerConfigFile, id);

        Directory.CreateDirectory(PathConstants.ServerConfigDirectory);

        File.WriteAllText(
            path,
            JsonSerializer.Serialize(
                DataItemWrapper.Wrap(configuration),
                options: new(JsonSerializerOptionsFactory.Common) { WriteIndented = true }
            )
        );
    }

    private void LoadAll()
    {
        if (!Directory.Exists(PathConstants.ServerConfigDirectory))
        {
            Directory.CreateDirectory(PathConstants.ServerConfigDirectory);
            Add("myserver", new());
            return;
        }

        var files = Directory.EnumerateFiles(PathConstants.ServerConfigDirectory, "*.json");

        foreach (var file in files)
        {
            try
            {
                var config = LoadFrom(file);

                Add(Path.GetFileNameWithoutExtension(file), config);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "尝试读取“{}”时异常", file);
            }
        }
    }

    private void OnCrash()
    {
        foreach (var (_, server) in Servers)
        {
            try
            {
                if (server.Configuration.AutoStopWhenCrashing && server.Status)
                {
                    server.Stop();
                }
            }
            catch { }
        }
    }

    public static Configuration LoadFrom(string path)
    {
        var data = JsonSerializer.Deserialize<DataItemWrapper<Configuration>>(
            File.ReadAllText(path),
            JsonSerializerOptionsFactory.Common
        );

        return data?.Type != typeof(Configuration).ToString()
            ? throw new InvalidOperationException("类型不正确")
            : data.Data ?? throw new InvalidOperationException("数据为空");
    }
}
