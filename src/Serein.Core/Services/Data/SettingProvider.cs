using System;
using System.IO;
using System.Text.Json;
using Serein.Core.Models;
using Serein.Core.Models.Settings;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Data;

public sealed class SettingProvider : DataProviderBase<Setting>
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
                    JsonSerializerOptionsFactory.Common
                );

                if (wrapper?.Type == typeof(Setting).ToString() && wrapper.Data is not null)
                {
                    return CompleteReactions(wrapper.Data);
                }
            }

            return new();
        }
        catch (Exception e)
        {
            throw new InvalidOperationException(
                $"加载设置文件（{PathConstants.SettingFile}）时出现异常",
                e
            );
        }
    }

    private static Setting CompleteReactions(Setting setting)
    {
        if (setting.Reactions.Count != Setting.DefaultReactions.Count)
        {
            foreach (var kv in Setting.DefaultReactions)
            {
                if (!setting.Reactions.ContainsKey(kv.Key))
                {
                    setting.Reactions.Add(kv.Key, kv.Value);
                }
            }
        }
        return setting;
    }

    public override void Save()
    {
        try
        {
            File.WriteAllText(
                PathConstants.SettingFile,
                JsonSerializer.Serialize(
                    DataItemWrapper.Wrap(Value),
                    options: new(JsonSerializerOptionsFactory.Common) { WriteIndented = true }
                )
            );
        }
        catch (Exception e)
        {
            throw new InvalidOperationException(
                $"保存设置文件（{PathConstants.SettingFile}）时出现异常",
                e
            );
        }
    }
}
