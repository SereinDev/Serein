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

public class ScheduleProvider : IItemProvider<ObservableCollection<Schedule>>
{
    private DateTime _last;
    public ObservableCollection<Schedule> Value { get; }

    public ScheduleProvider()
    {
        Value = [];
        Read();
    }

    public ObservableCollection<Schedule> Read()
    {
        try
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
        catch (Exception e)
        {
            throw new InvalidOperationException($"加载定时任务文件（{PathConstants.SchedulesFile}）时出现异常", e);
        }
    }

    public void Save()
    {
        try
        {
            Directory.CreateDirectory(PathConstants.Root);
            File.WriteAllText(
                PathConstants.SchedulesFile,
                JsonSerializer.Serialize(
                    DataItemWrapper.Wrap(nameof(Schedule), Value),
                    options: new(JsonSerializerOptionsFactory.CamelCase) { WriteIndented = true }
                )
            );
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"保存定时任务文件（{PathConstants.SchedulesFile}）时出现异常", e);
        }
    }

    public async Task SaveAsyncWithDebounce()
    {
        _last = DateTime.Now;
        await Task.Delay(1000);

        if ((DateTime.Now - _last).TotalMilliseconds > 900)
            Save();
    }
}
