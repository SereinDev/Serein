namespace Serein.Core.Services.Data;

public interface IDataProvider<T>
    where T : notnull
{
    T Read();

    void Save();

    T Value { get; }
}
