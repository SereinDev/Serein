using System;
using System.Text.Json.Serialization;

namespace Serein.Core.Models.Abstractions;

public abstract class DataItemWrapper
{
    public static DataItemWrapper<TItem> Wrap<TItem>(TItem data)
        where TItem : notnull => new() { Type = typeof(TItem).ToString(), Data = data };

    [JsonRequired]
    [JsonPropertyOrder(-114514)]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("_time")]
    [JsonPropertyOrder(-1919810)]
    public DateTime Time { get; } = DateTime.Now;
}

public class DataItemWrapper<T> : DataItemWrapper
    where T : notnull
{
    [JsonRequired]
    public T? Data { get; set; }
}
