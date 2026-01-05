using System;
using System.Text.Json;
using System.Threading.Tasks;
using Serein.Core.Models.Abstractions;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Data;

public abstract class FileDataProviderBase<T> : NotifyPropertyChangedModelBase
    where T : notnull
{
    protected static readonly JsonSerializerOptions Options = new(
        JsonSerializerOptionsFactory.Common
    )
    {
        WriteIndented = true,
    };

    private DateTime _last;

    public abstract T Read();

    public abstract void Save();

    public async Task SaveAsyncWithDebounce()
    {
        _last = DateTime.Now;
        await Task.Delay(1000);

        if ((DateTime.Now - _last).TotalMilliseconds > 900)
        {
            Save();
        }
    }

    public abstract T Value { get; }
}
