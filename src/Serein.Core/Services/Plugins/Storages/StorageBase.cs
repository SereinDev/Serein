using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Logging;

namespace Serein.Core.Services.Plugins.Storages;

public abstract class StorageBase(ILogger logger)
{
    protected readonly Dictionary<string, string> _data = [];
    protected readonly ILogger _logger = logger;

    public int Length => _data.Count;

    public void Clear()
    {
        lock (_data)
        {
            _data.Clear();
        }

        OnUpdated();
    }

    public string? GetItem(string key)
    {
        return _data.TryGetValue(key, out var value) ? value : null;
    }

    public void RemoveItem(string key)
    {
        lock (_data)
        {
            _data.Remove(key);
        }

        OnUpdated();
    }

    public void SetItem(string key, string value)
    {
        lock (_data)
        {
            _data[key] = value ?? "null";
        }
        
        _logger.LogDebug("更新：'{}'='{}'", key, value);
        OnUpdated();
    }

    public string? Key(int index)
    {
        return _data.Keys.ElementAtOrDefault(index);
    }

    public string? this[string key]
    {
        get => GetItem(key);
        set => SetItem(key, value!);
    }

    protected virtual void OnUpdated() { }
}
