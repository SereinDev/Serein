using System;
using System.Text.Json.Serialization;

namespace Serein.Core.Models;

public abstract class DataItemWrapper
{
    public static DataItemWrapper<TItem> Wrap<TItem>(string type, TItem data)
        where TItem : notnull => new(type, data);

    [JsonRequired]
    [JsonPropertyOrder(-114514)]
    public string Type { get; set; } = string.Empty;

#pragma warning disable CA1822
    [JsonPropertyName("_time")]
    [JsonPropertyOrder(-1919810)]
    public DateTime Time => DateTime.Now;
#pragma warning restore CA1822
}

public class DataItemWrapper<T> : DataItemWrapper
    where T : notnull
{
    [JsonRequired]
    public T? Data { get; set; }

    public DataItemWrapper(string type, T data)
    {
        Type = type;
        Data = data;
    }
}
