using System.IO;
using System.Text.Json;

using Serein.Core.Models;
using Serein.Core.Models.Settings;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Data;

public class SettingProvider : IItemProvider<AppSetting>
{
    private AppSetting? _appSetting;

    public AppSetting Value => _appSetting ?? Read();

    public AppSetting Read()
    {
        if (File.Exists(PathConstants.MatchesFile))
        {
            var wrapper = JsonSerializer.Deserialize<DataItemWrapper<AppSetting>>(
                File.ReadAllText(PathConstants.MatchesFile),
                JsonSerializerOptionsFactory.CamelCase
            );

            if (wrapper?.Type == nameof(AppSetting))
                _appSetting = wrapper.Data;
        }
        else
            Save();

        _appSetting ??= new();
        return _appSetting!;
    }

    public void Save()
    {
        _appSetting ??= new();
        File.WriteAllText(
            PathConstants.SettingFile,
            JsonSerializer.Serialize(
                DataItemWrapper.Wrap(nameof(AppSetting), _appSetting),
                options: new(JsonSerializerOptionsFactory.CamelCase) { WriteIndented = true }
            )
        );
    }
}