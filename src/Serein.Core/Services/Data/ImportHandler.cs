using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Serein.Core.Models;
using Serein.Core.Models.Commands;
using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Data;

public class ImportHandler(
    ILogger<ImportHandler> logger,
    MatchProvider matchProvider,
    ScheduleProvider scheduleProvider,
    ServerManager serverManager
)
{
    public void Import(
        string[] datas,
        Func<ImportActionType, bool> comfirm,
        Func<ImportActionType, bool> shouldMerge
    )
    {
        if (datas.Length != 1)
        {
            throw new InvalidOperationException("一次只能导入一个文件");
        }

        var path = datas[0];

        logger.LogDebug("准备导入文件：{}", path);

        if (
            string.IsNullOrEmpty(path)
            || !Path.GetExtension(path).Equals(".json", StringComparison.InvariantCultureIgnoreCase)
        )
        {
            throw new InvalidOperationException("不支持此文件扩展名");
        }

        if (!File.Exists(path))
        {
            throw new FileNotFoundException("文件不存在", path);
        }

        var text = File.ReadAllText(path);
        var dataItemWrapper = JsonSerializer.Deserialize<DataItemWrapper<JsonNode>>(
            text,
            JsonSerializerOptionsFactory.Common
        );

        if (dataItemWrapper is null)
        {
            throw new InvalidDataException("文件为空");
        }

        HandleFile(path, dataItemWrapper, comfirm, shouldMerge);
    }

    private void HandleFile(
        string path,
        DataItemWrapper<JsonNode> dataItemWrapper,
        Func<ImportActionType, bool> comfirm,
        Func<ImportActionType, bool> shouldMerge
    )
    {
        if (dataItemWrapper.Type == typeof(ObservableCollection<Match>).ToString())
        {
            if (!comfirm.Invoke(ImportActionType.Match))
            {
                return;
            }

            var merge = shouldMerge.Invoke(ImportActionType.Match);

            lock (matchProvider.Value)
            {
                if (merge)
                {
                    matchProvider.Value.Clear();
                }

                foreach (
                    var match in dataItemWrapper.Data.Deserialize<Match[]>(
                        JsonSerializerOptionsFactory.Common
                    ) ?? []
                )
                {
                    matchProvider.Value.Add(match);
                }
            }
        }
        else if (dataItemWrapper.Type == typeof(ObservableCollection<Schedule>).ToString())
        {
            if (!comfirm.Invoke(ImportActionType.Schedule))
            {
                return;
            }

            var merge = shouldMerge.Invoke(ImportActionType.Schedule);

            lock (scheduleProvider.Value)
            {
                if (merge)
                {
                    scheduleProvider.Value.Clear();
                }

                foreach (
                    var schedule in dataItemWrapper.Data.Deserialize<Schedule[]>(
                        JsonSerializerOptionsFactory.Common
                    ) ?? []
                )
                {
                    scheduleProvider.Value.Add(schedule);
                }
            }
        }
        else if (dataItemWrapper.Type == typeof(Configuration).ToString())
        {
            if (!comfirm.Invoke(ImportActionType.Server))
            {
                return;
            }

            serverManager.Add(
                Path.GetFileNameWithoutExtension(path),
                dataItemWrapper.Data.Deserialize<Configuration>()!
            );
        }
        else
        {
            throw new NotSupportedException("不支持导入此文件类型：" + dataItemWrapper.Type);
        }
    }
}
