namespace Serein.Core.Services.Data;

public interface IItemProvider<T>
    where T : notnull
{
    T Read();

    void Save();

    T Value { get; }
}
