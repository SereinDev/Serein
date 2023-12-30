using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

using Serein.Core.Models;
using Serein.Core.Models.Commands;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Data;

public class ScheduleProvider : IItemProvider<ObservableCollection<Schedule>>
{
    public ObservableCollection<Schedule> Value { get; }

    public ScheduleProvider()
    {
        Value = new();
    }

    public ObservableCollection<Schedule> Read()
    {
        if (File.Exists(PathConstants.SchedulesFile))
        {
            var wrapper = JsonSerializer.Deserialize<DataItemWrapper<List<Schedule>>>(
                File.ReadAllText(PathConstants.SchedulesFile),
                JsonSerializerOptionsFactory.CamelCase
            );

            if (wrapper?.Type == nameof(Schedule))
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
            PathConstants.SchedulesFile,
            JsonSerializer.Serialize(
                DataItemWrapper.Wrap(nameof(Schedule), Value),
                options: new(JsonSerializerOptionsFactory.CamelCase) { WriteIndented = true }
            )
        );
    }
}
