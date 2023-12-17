namespace Serein.Core.Models;

public abstract class DataItemWrapper
{
    public string Type { get; set; } = string.Empty;

    public static DataItemWrapper<TItem> Wrap<TItem>(string type, TItem data)
        where TItem : notnull
    {
        return new(type, data);
    }
}

public class DataItemWrapper<T> : DataItemWrapper
    where T : notnull
{
    public T? Data { get; set; }

    public DataItemWrapper(string type, T data)
    {
        Type = type;
        Data = data;
    }
}
