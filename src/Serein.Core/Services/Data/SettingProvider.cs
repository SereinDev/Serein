using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

using Serein.Core.Models;
using Serein.Core.Models.Settings;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Data;

public class SettingProvider : IItemProvider<Setting>, INotifyPropertyChanged
{
    private DateTime _last;

    public SettingProvider()
    {
        Value = Read();
    }

    public Setting Value { get; }

    public Setting Read()
    {
        if (File.Exists(PathConstants.SettingFile))
        {
            var wrapper = JsonSerializer.Deserialize<DataItemWrapper<Setting>>(
                File.ReadAllText(PathConstants.SettingFile),
                JsonSerializerOptionsFactory.CamelCase
            );

            if (wrapper?.Type == nameof(Setting))
                return wrapper.Data;
        }
        else
            Save();

        return new();
    }

    public void Save()
    {
        File.WriteAllText(
            PathConstants.SettingFile,
            JsonSerializer.Serialize(
                DataItemWrapper.Wrap(nameof(Setting), Value),
                options: new(JsonSerializerOptionsFactory.CamelCase) { WriteIndented = true }
            )
        );
    }

    public async Task SaveAsyncWithDebounce()
    {
        _last = DateTime.Now;
        await Task.Delay(500);

        if ((DateTime.Now - _last).TotalMilliseconds > 400)
            Save();
    }

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
