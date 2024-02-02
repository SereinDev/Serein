using System;
using System.Text.Json.Serialization;

namespace Serein.Core.Models;

public abstract class DataItemWrapper
{
    [JsonRequired]
    [JsonPropertyOrder(-114514)]
    public string Type { get; set; } = string.Empty;

    [JsonRequired]
    [JsonPropertyOrder(-1919810)]
    public static DateTime Time => DateTime.Now;

    public static DataItemWrapper<TItem> Wrap<TItem>(string type, TItem data)
        where TItem : notnull
    {
        return new(type, data);
    }
}

public class DataItemWrapper<T> : DataItemWrapper
    where T : notnull
{
    [JsonRequired]
    public T Data { get; set; }

    public DataItemWrapper(string type, T data)
    {
        Type = type;
        Data = data;
    }
}
