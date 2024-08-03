using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;

using Serein.Core.Models;
using Serein.Core.Models.Settings;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Data;

public class SettingProvider : DataProviderBase<Setting>, INotifyPropertyChanged
{
    public SettingProvider()
    {
        Value = Read();
        Save();
    }

    public override Setting Value { get; }

    public override Setting Read()
    {
        try
        {
            if (File.Exists(PathConstants.SettingFile))
            {
                var wrapper = JsonSerializer.Deserialize<DataItemWrapper<Setting>>(
                    File.ReadAllText(PathConstants.SettingFile),
                    JsonSerializerOptionsFactory.CamelCase
                );

                if (wrapper?.Type == nameof(Setting))
                    return wrapper.Data ?? new();
            }

            return new();
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"加载设置文件（{PathConstants.SettingFile}）时出现异常", e);
        }
    }

    public override void Save()
    {
        try
        {
            File.WriteAllText(
            PathConstants.SettingFile,
            JsonSerializer.Serialize(
               DataItemWrapper.Wrap(nameof(Setting), Value),
               options: new(JsonSerializerOptionsFactory.CamelCase) { WriteIndented = true }
                )
            );
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"保存设置文件（{PathConstants.SettingFile}）时出现异常", e);
        }
    }

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
