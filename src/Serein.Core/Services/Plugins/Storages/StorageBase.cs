using System.Collections.Generic;
using System.Linq;

using Serein.Core.Utils;

namespace Serein.Core.Services.Plugins.Storages;

public abstract class StorageBase
{
    public const string Path = $"{PathConstants.PluginsDirectory}/{PathConstants.LocalStorageFileName}";

    protected StorageBase()
    {
        _data = [];
    }

    protected readonly Dictionary<string, string> _data;

    public int Length => _data.Count;

    public void Clear()
    {
        lock (_data)
            _data.Clear();

        OnUpdated();
    }

    public string? GetItem(string key)
    {
        return _data.TryGetValue(key, out var value) ? value : null;
    }

    public void RemoveItem(string key)
    {
        lock (_data)
            _data.Remove(key);

        OnUpdated();
    }

    public void SetItem(string key, string value)
    {
        lock (_data)
            _data.Add(key, value ?? "null");

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