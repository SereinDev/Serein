using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

using Serein.Core.Models;
using Serein.Core.Models.Commands;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Data;

public class MatchesProvider : IItemProvider<ObservableCollection<Match>>
{
    private DateTime _last;

    public MatchesProvider()
    {
        Value = new();
        Read();
    }

    public ObservableCollection<Match> Value { get; }

    public ObservableCollection<Match> Read()
    {
        if (File.Exists(PathConstants.MatchesFile))
        {
            var wrapper = JsonSerializer.Deserialize<DataItemWrapper<List<Match>>>(
                File.ReadAllText(PathConstants.MatchesFile),
                JsonSerializerOptionsFactory.CamelCase
            );

            if (wrapper?.Type == nameof(Match))
                lock (Value)
                {
                    Value.Clear();

                    if (wrapper.Data is not null)
                        foreach (var match in wrapper.Data)
                        {
                            Value.Add(match);
                        }
                }
        }
        else
            Save();

        return Value;
    }

    public void Save()
    {
        Directory.CreateDirectory(PathConstants.DataDirectory);
        File.WriteAllText(
            PathConstants.MatchesFile,
            JsonSerializer.Serialize(
                DataItemWrapper.Wrap(nameof(Match), Value),
                options: new(JsonSerializerOptionsFactory.CamelCase) { WriteIndented = true }
            )
        );
    }

    public async Task SaveAsyncWithDebounce()
    {
        _last = DateTime.Now;
        await Task.Delay(1000);

        if ((DateTime.Now - _last).TotalMilliseconds > 900)
            Save();
    }
}
