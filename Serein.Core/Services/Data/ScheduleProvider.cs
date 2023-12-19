using System.Collections.Generic;
using System.IO;
using System.Text.Json;

using Serein.Core.Models;
using Serein.Core.Models.Commands;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Data;

public class ScheduleProvider : IItemProvider<IReadOnlyList<Schedule>>
{
    private List<Schedule>? _schedules;

    public IReadOnlyList<Schedule> Value => _schedules ?? Read();

    public IReadOnlyList<Schedule> Read()
    {
        if (File.Exists(PathConstants.SchedulesFile))
        {
            var wrapper = JsonSerializer.Deserialize<DataItemWrapper<List<Schedule>>>(
                File.ReadAllText(PathConstants.SchedulesFile),
                JsonSerializerOptionsFactory.CamelCase
            );

            if (wrapper?.Type == nameof(Schedule))
                _schedules = wrapper.Data;
        }
        else
            Save();

        _schedules ??= new();
        return _schedules;
    }

    public void Save()
    {
        _schedules ??= new();
        Directory.CreateDirectory(PathConstants.DataDirectory);
        File.WriteAllText(
            PathConstants.SchedulesFile,
            JsonSerializer.Serialize(
                DataItemWrapper.Wrap(nameof(Schedule), _schedules),
                options: new(JsonSerializerOptionsFactory.CamelCase) { WriteIndented = true }
            )
        );
    }
}