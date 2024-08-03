using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

using Serein.Core.Utils;

namespace Serein.Core.Services.Plugins.Storages;

public class LocalStorage : StorageBase
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true,
        AllowTrailingCommas = true,
    };

    public LocalStorage()
    {
        Directory.CreateDirectory(PathConstants.PluginsDirectory);

        if (File.Exists(Path))
        {
            var obj = JsonSerializer.Deserialize<JsonObject>(File.ReadAllText(Path));

            if (obj is not null)
                foreach ((string key, JsonNode? value) in obj)
                    _data.TryAdd(key, value?.ToString() ?? "null");
        }
        else
            File.WriteAllText(Path, "{}");
    }

    protected override void OnUpdated()
    {
        lock (_data)
            File.WriteAllText(Path, JsonSerializer.Serialize(_data, Options));
    }
}