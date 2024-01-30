using System.Threading.Tasks;

namespace Serein.Core.Services.Data;

public interface IItemProvider<T>
    where T : notnull
{
    T Read();

    void Save();

    Task SaveAsyncWithDebounce();

    T Value { get; }
}
