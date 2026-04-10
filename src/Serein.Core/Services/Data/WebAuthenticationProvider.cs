using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Network.Web.WebAuthentication;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Data;

internal sealed class WebAuthenticationProvider
    : FileDataProviderBase<ObservableCollection<AuthenticationBase>>
{
    public WebAuthenticationProvider()
    {
        Read();
    }

    public override ObservableCollection<AuthenticationBase> Value { get; } = [];

    public override ObservableCollection<AuthenticationBase> Read()
    {
        try
        {
            if (File.Exists(PathConstants.WebAuthenticationsFile))
            {
                var wrapper = JsonSerializer.Deserialize<
                    DataItemWrapper<ObservableCollection<AuthenticationBase>>
                >(
                    File.ReadAllText(PathConstants.WebAuthenticationsFile),
                    JsonSerializerOptionsFactory.Common
                );

                if (wrapper?.Type != typeof(ObservableCollection<AuthenticationBase>).ToString())
                {
                    return Value;
                }

                lock (Value)
                {
                    Value.Clear();

                    if (wrapper.Data is null)
                    {
                        return Value;
                    }

                    foreach (var task in wrapper.Data)
                    {
                        Value.Add(task);
                    }
                }
            }
            else
            {
                Save();
            }

            return Value;
        }
        catch (Exception e)
        {
            throw new InvalidOperationException(
                $"加载网页验证凭据文件（{PathConstants.WebAuthenticationsFile}）时出现异常",
                e
            );
        }
    }

    public override void Save()
    {
        try
        {
            Directory.CreateDirectory(PathConstants.Root);
            File.WriteAllText(
                PathConstants.WebAuthenticationsFile,
                JsonSerializer.Serialize(DataItemWrapper.Wrap(Value), Options)
            );
        }
        catch (Exception e)
        {
            throw new InvalidOperationException(
                $"保存网页验证凭据文件（{PathConstants.WebAuthenticationsFile}）时出现异常",
                e
            );
        }
    }
}
