using Serein.Core.Models;
using Serein.Core.Models.Settings;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

using System.IO;
using System.Text.Json;

namespace Serein.Core.Services.Data;

public class SettingProvider : IDataProvider<AppSetting>
{
    private AppSetting? _appSetting;

    public AppSetting Value => _appSetting ?? Read();

    public AppSetting Read()
    {
        if (File.Exists(PathConstants.Matches))
        {
            var wrapper = JsonSerializer.Deserialize<DataItemWrapper<AppSetting>>(
                File.ReadAllText(PathConstants.Matches),
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
            PathConstants.Setting,
            JsonSerializer.Serialize(
                DataItemWrapper.Wrap(nameof(AppSetting), _appSetting),
                options: new(JsonSerializerOptionsFactory.CamelCase) { WriteIndented = true }
            )
        );
    }
}
