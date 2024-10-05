using System;
using System.Threading.Tasks;

using Serein.Core.Models;

namespace Serein.Core.Services.Data;

public abstract class DataProviderBase<T> : NotifyPropertyChangedModelBase
    where T : notnull
{
    private DateTime _last;

    public abstract T Read();

    public abstract void Save();

    public async Task SaveAsyncWithDebounce()
    {
        _last = DateTime.Now;
        await Task.Delay(1000);

        if ((DateTime.Now - _last).TotalMilliseconds > 900)
            Save();
    }

    public abstract T Value { get; }
}
