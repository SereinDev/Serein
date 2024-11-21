using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

using Microsoft.Extensions.Logging;

using Serein.Core.Models;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Plugins.Storages;

public sealed class LocalStorage : StorageBase
{
    public static readonly string Path =
        $"{PathConstants.PluginsDirectory}/{PathConstants.LocalStorageFileName}";

    public LocalStorage(ILogger<LocalStorage> logger)
        : base(logger)
    {
        if (File.Exists(Path))
        {
            var data = JsonSerializer.Deserialize<DataItemWrapper<JsonObject>>(
                File.ReadAllText(Path),
                options: new(JsonSerializerOptionsFactory.Common) { WriteIndented = true }
            );

            if (data?.Type == typeof(JsonObject).ToString() && data.Data is not null)
            {
                foreach ((string key, JsonNode? value) in data.Data)
                {
                    _data.TryAdd(key, value?.ToString() ?? "null");
                }
            }
        }
        else
        {
            OnUpdated();
        }
    }

    protected override void OnUpdated()
    {
        Directory.CreateDirectory(PathConstants.PluginsDirectory);
        lock (_data)
        {
            File.WriteAllText(
             Path,
             JsonSerializer.Serialize(
                 DataItemWrapper.Wrap(_data),
                 options: new(JsonSerializerOptionsFactory.Common) { WriteIndented = true }
             )
         );
        }
    }
}
