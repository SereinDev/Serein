using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Serein.Core.Models;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Automations;
using Serein.Core.Models.Commands;
using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Data;

public class ImportHandler(
    ILogger<ImportHandler> logger,
    ServerManager serverManager,
    AutomationTaskProvider automationTaskProvider
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
        var dataItemWrapper =
            JsonSerializer.Deserialize<DataItemWrapper<JsonNode>>(
                text,
                JsonSerializerOptionsFactory.Common
            ) ?? throw new InvalidDataException("文件为空");

        HandleFile(path, dataItemWrapper, comfirm, shouldMerge);
    }

    private void HandleFile(
        string path,
        DataItemWrapper<JsonNode> dataItemWrapper,
        Func<ImportActionType, bool> comfirm,
        Func<ImportActionType, bool> shouldMerge
    )
    {
        if (
            TryUnwrap(
                dataItemWrapper,
                typeof(List<AutomationTask>).ToString(),
                out AutomationTask[]? tasks,
                JsonSerializerOptionsFactory.Common
            )
        )
        {
            if (!comfirm.Invoke(ImportActionType.AutomationTask))
            {
                return;
            }

            var merge = shouldMerge.Invoke(ImportActionType.AutomationTask);

            lock (automationTaskProvider.Value)
            {
                if (!merge)
                {
                    automationTaskProvider.Value.Clear();
                }

                foreach (var task in tasks)
                {
                    automationTaskProvider.Value.Add(task);
                }
            }
        }
        else if (
            TryUnwrap(
                dataItemWrapper,
                out Configuration? configuration,
                JsonSerializerOptionsFactory.Common
            )
        )
        {
            if (!comfirm.Invoke(ImportActionType.Server))
            {
                return;
            }

            serverManager.Add(Path.GetFileNameWithoutExtension(path), configuration);
        }
        else
        {
            throw new NotSupportedException("不支持导入此文件类型：" + dataItemWrapper.Type);
        }
    }

    private static bool TryUnwrap<TItem>(
        DataItemWrapper<JsonNode> wrapper,
        [NotNullWhen(true)] out TItem? item,
        JsonSerializerOptions? jsonSerializerOptions = null
    )
        where TItem : notnull
    {
        return TryUnwrap(wrapper, typeof(TItem).ToString(), out item, jsonSerializerOptions);
    }

    private static bool TryUnwrap<TItem>(
        DataItemWrapper<JsonNode> wrapper,
        string expectedTypeName,
        [NotNullWhen(true)] out TItem? item,
        JsonSerializerOptions? jsonSerializerOptions = null
    )
        where TItem : notnull
    {
        if (wrapper is null || wrapper.Type != expectedTypeName)
        {
            item = default;
            return false;
        }

        item = wrapper.Data.Deserialize<TItem>(jsonSerializerOptions);
        return item is not null;
    }
}
