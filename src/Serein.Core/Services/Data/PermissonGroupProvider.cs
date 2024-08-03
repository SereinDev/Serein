using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

using Serein.Core.Models;
using Serein.Core.Models.Permissions;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Data;

public class PermissionGroupProvider : DataProviderBase<Dictionary<string, Group>>
{
    public PermissionGroupProvider()
    {
        Value = new()
        {
            ["everyone"] = new()
            {
                Name = "默认",
                Description = "默认权限组",
                Priority = int.MinValue
            }
        };
        Read();
    }

    public override Dictionary<string, Group> Value { get; }

    public override Dictionary<string, Group> Read()
    {
        try
        {
            if (File.Exists(PathConstants.PermissionGroupsFile))
            {
                var wrapper = JsonSerializer.Deserialize<DataItemWrapper<Dictionary<string, Group>>>(
                    File.ReadAllText(PathConstants.PermissionGroupsFile),
                    JsonSerializerOptionsFactory.CamelCase
                );

                if (wrapper?.Type == nameof(Dictionary<string, Group>))
                    lock (Value)
                    {
                        Value.Clear();

                        if (wrapper.Data is not null)
                            foreach (var kv in wrapper.Data)
                            {
                                Value.Add(kv.Key, kv.Value);
                            }
                    }
            }
            else
                Save();

            return Value;
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"加载权限组文件（{PathConstants.PermissionGroupsFile}）时出现异常", e);
        }
    }

    public override void Save()
    {
        try
        {
            Directory.CreateDirectory(PathConstants.Root);
            File.WriteAllText(
                PathConstants.PermissionGroupsFile,
                JsonSerializer.Serialize(
                    DataItemWrapper.Wrap(nameof(Dictionary<string, Group>), Value),
                    options: new(JsonSerializerOptionsFactory.CamelCase) { WriteIndented = true }
                )
            );
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"保存权限组文件（{PathConstants.PermissionGroupsFile}）时出现异常", e);
        }
    }
}
