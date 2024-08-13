using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;

using Serein.Core.Models;
using Serein.Core.Models.Network.Ssh;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Data;

public class SshServerKeysProvider : DataProviderBase<SshServerKeys>
{
    public SshServerKeysProvider()
    {
        Value = Read();
        Save();
    }

    public override SshServerKeys Value { get; }

    public override SshServerKeys Read()
    {
        try
        {
            if (File.Exists(PathConstants.SshKeysFile))
            {
                var wrapper = JsonSerializer.Deserialize<DataItemWrapper<SshServerKeys>>(
                    File.ReadAllText(PathConstants.SshKeysFile),
                    JsonSerializerOptionsFactory.CamelCase
                );

                if (wrapper?.Type == typeof(SshServerKeys).ToString())
                    return wrapper.Data ?? new();
            }

            return new()
            {
                Rsa = Convert.ToBase64String(new RSACryptoServiceProvider(4096).ExportCspBlob(true)),
                Dss = Convert.ToBase64String(new DSACryptoServiceProvider(1024).ExportCspBlob(true)),
            };
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"加载SSH服务器Key文件（{PathConstants.SshKeysFile}）时出现异常", e);
        }
    }

    public override void Save()
    {
        try
        {
            Directory.CreateDirectory(PathConstants.Root);
            File.WriteAllText(
                PathConstants.SshKeysFile,
                JsonSerializer.Serialize(
                    DataItemWrapper.Wrap(Value),
                    options: new(JsonSerializerOptionsFactory.CamelCase) { WriteIndented = true }
                )
            );
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"保存SSH服务器Key文件（{PathConstants.SshKeysFile}）时出现异常", e);
        }
    }
}