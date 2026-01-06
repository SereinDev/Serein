using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Automations;
using Serein.Core.Utils;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Data;

public sealed class AutomationTaskProvider
    : FileDataProviderBase<ObservableCollection<AutomationTask>>
{
    public AutomationTaskProvider()
    {
         Read();
    }

    public override ObservableCollection<AutomationTask> Value { get; } = [];

    public override ObservableCollection<AutomationTask> Read()
    {
        try
        {
            if (File.Exists(PathConstants.AutomationTasksFile))
            {
                var wrapper = JsonSerializer.Deserialize<DataItemWrapper<ObservableCollection<AutomationTask>>>(
                    File.ReadAllText(PathConstants.AutomationTasksFile),
                    JsonSerializerOptionsFactory.Common
                );

                if (wrapper?.Type == typeof(ObservableCollection<AutomationTask>).ToString())
                {
                    lock (Value)
                    {
                        Value.Clear();

                        if (wrapper.Data is not null)
                        {
                            foreach (var task in wrapper.Data)
                            {
                                Value.Add(task);
                            }
                        }
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
                $"加载自动化任务文件（{PathConstants.AutomationTasksFile}）时出现异常",
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
                PathConstants.AutomationTasksFile,
                JsonSerializer.Serialize(DataItemWrapper.Wrap(Value), Options)
            );
        }
        catch (Exception e)
        {
            throw new InvalidOperationException(
                $"保存自动化任务文件（{PathConstants.AutomationTasksFile}）时出现异常",
                e
            );
        }
    }
}
