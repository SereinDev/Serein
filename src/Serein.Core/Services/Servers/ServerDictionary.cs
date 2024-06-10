using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

using Serein.Core.Models.Output;
using Serein.Core.Services.Data;
using Serein.Core.Services.Plugins;

namespace Serein.Core.Services.Servers;

public partial class ServerDictionary(
    ISereinLogger logger,
    Matcher matcher,
    SettingProvider settingProvider,
    EventDispatcher eventDispatcher,
    ReactionManager reactionManager
) : IReadOnlyDictionary<string, Server>
{
    [GeneratedRegex(@"^\w{3,}$")]
    private static partial Regex GetServerIdRegex();

    private static readonly Regex ServerId = GetServerIdRegex();
    private readonly Dictionary<string, Server> _servers = new();
    private readonly ISereinLogger _logger = logger;
    private readonly SettingProvider _settingProvider = settingProvider;
    private readonly Matcher _matcher = matcher;
    private readonly EventDispatcher _eventDispatcher = eventDispatcher;
    private readonly ReactionManager _reactionManager = reactionManager;

    private static void CheckId(string? id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

        if (!ServerId.IsMatch(id))
            throw new InvalidOperationException("服务器ID不正确");
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out Server value) =>
        _servers.TryGetValue(key, out value);

    public bool ContainsKey(string key) => _servers.ContainsKey(key);

    public IEnumerator<KeyValuePair<string, Server>> GetEnumerator() => _servers.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _servers.GetEnumerator();

    public int Count => _servers.Count;
    public Server this[string key] => _servers[key];
    public ICollection<string> Keys => _servers.Keys;
    public ICollection<Server> Values => _servers.Values;
    IEnumerable<string> IReadOnlyDictionary<string, Server>.Keys => Keys;
    IEnumerable<Server> IReadOnlyDictionary<string, Server>.Values => Values;

    public void Add(string id, Configuration configuration)
    {
        CheckId(id);
        _servers.Add(
            id,
            new(
                id,
                _logger,
                configuration,
                _settingProvider,
                _matcher,
                _eventDispatcher,
                _reactionManager
            )
        );
    }

    public bool Remove(string id)
    {
        CheckId(id);
        return _servers.Remove(id);
    }

    public bool AnyRunning =>
        _servers.Any(static (kv) => kv.Value.Status == Models.Server.ServerStatus.Running);
}
