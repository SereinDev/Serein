using System.IO;
using System.Text.Json;

using Serein.Core.Models;
using Serein.Core.Models.Settings;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Data;

public class SettingProvider : IItemProvider<Setting>
{
    private Setting? _setting;

    public Setting Value => _setting ?? Read();

    public Setting Read()
    {
        if (File.Exists(PathConstants.SettingFile))
        {
            var wrapper = JsonSerializer.Deserialize<DataItemWrapper<Setting>>(
                File.ReadAllText(PathConstants.SettingFile),
                JsonSerializerOptionsFactory.CamelCase
            );

            if (wrapper?.Type == nameof(Setting))
                _setting = wrapper.Data;
        }
        else
            Save();

        _setting ??= new();
        return _setting!;
    }

    public void Save()
    {
        _setting ??= new();
        File.WriteAllText(
            PathConstants.SettingFile,
            JsonSerializer.Serialize(
                DataItemWrapper.Wrap(nameof(Setting), _setting),
                options: new(JsonSerializerOptionsFactory.CamelCase) { WriteIndented = true }
            )
        );
    }
}
