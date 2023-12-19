using System.Collections.Generic;
using System.IO;
using System.Text.Json;

using Serein.Core.Models;
using Serein.Core.Models.Commands;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Data;

public class MatchesProvider : IItemProvider<IReadOnlyList<Match>>
{
    private List<Match>? _matches;

    public IReadOnlyList<Match> Value => _matches ?? Read();

    public IReadOnlyList<Match> Read()
    {
        if (File.Exists(PathConstants.MatchesFile))
        {
            var wrapper = JsonSerializer.Deserialize<DataItemWrapper<List<Match>>>(
                File.ReadAllText(PathConstants.MatchesFile),
                JsonSerializerOptionsFactory.CamelCase
            );

            if (wrapper?.Type == nameof(Match))
                _matches = wrapper.Data;
        }
        else
            Save();

        _matches ??= new();
        return _matches;
    }

    public void Save()
    {
        _matches ??= new();
        Directory.CreateDirectory(PathConstants.DataDirectory);
        File.WriteAllText(
            PathConstants.MatchesFile,
            JsonSerializer.Serialize(
                DataItemWrapper.Wrap(nameof(Match), _matches),
                options: new(JsonSerializerOptionsFactory.CamelCase) { WriteIndented = true }
            )
        );
    }
}