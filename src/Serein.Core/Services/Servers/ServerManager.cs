using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Core.Models.Server;
using Serein.Core.Services.Data;
using Serein.Core.Services.Plugins;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Servers;

public partial class ServerManager(
    ISereinLogger logger,
    Matcher matcher,
    SettingProvider settingProvider,
    EventDispatcher eventDispatcher,
    ReactionManager reactionManager
)
{
    [GeneratedRegex(@"^\w{3,}$")]
    private static partial Regex GetServerIdRegex();

    private static readonly Regex ServerId = GetServerIdRegex();

    internal static void CheckId(string? id)
    {
        if (string.IsNullOrEmpty(id) || !ServerId.IsMatch(id))
            throw new InvalidOperationException("服务器Id格式不正确");
    }

    public IReadOnlyDictionary<string, Server> Servers => _server;

    private readonly Dictionary<string, Server> _server = new();
    private readonly Matcher _matcher = matcher;
    private readonly ISereinLogger _logger = logger;
    private readonly SettingProvider _settingProvider = settingProvider;
    private readonly EventDispatcher _eventDispatcher = eventDispatcher;
    private readonly ReactionManager _reactionManager = reactionManager;

    public bool AnyRunning =>
        _server.Any(static (kv) => kv.Value.Status == ServerStatus.Running);

    public Server Add(string id, Configuration configuration)
    {
        CheckId(id);

        var server = new Server(
                id,
                _logger,
                configuration,
                _settingProvider,
                _matcher,
                _eventDispatcher,
                _reactionManager
            );
        _server.Add(
            id,
           server
        );

        Save(id, configuration);
        return server;
    }

    public bool Remove(string id)
    {
        CheckId(id);
        if (!_server.Remove(id))
            return false;

        var path = string.Format(PathConstants.ServerConfigFileName, id);
        if (File.Exists(path))
            File.Delete(path);

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
                _logger.LogDebug(e, "保存文件时出现异常");
            }
        }
    }

    private static void Save(string id, Configuration configuration)
    {
        CheckId(id);

        var path = string.Format(PathConstants.ServerConfigFileName, id);
        var dir = Path.GetDirectoryName(path);

        if (string.IsNullOrEmpty(dir))
            throw new InvalidOperationException();

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        File.WriteAllText(
            path,
            JsonSerializer.Serialize(
                configuration,
                options: new(JsonSerializerOptionsFactory.CamelCase) { WriteIndented = true, }
                )
        );
    }
}
