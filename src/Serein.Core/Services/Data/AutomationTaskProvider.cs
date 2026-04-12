using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Automations;
using Serein.Core.Models.Automations.Triggers;
using Serein.Core.Models.Commands;
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

    public override ObservableCollection<AutomationTask> Value { get; } = [.. Preset];

    public override ObservableCollection<AutomationTask> Read()
    {
        try
        {
            if (File.Exists(PathConstants.AutomationTasksFile))
            {
                var wrapper = JsonSerializer.Deserialize<
                    DataItemWrapper<ObservableCollection<AutomationTask>>
                >(
                    File.ReadAllText(PathConstants.AutomationTasksFile),
                    JsonSerializerOptionsFactory.Common
                );

                if (wrapper?.Type != typeof(ObservableCollection<AutomationTask>).ToString())
                {
                    return Value;
                }

                lock (Value)
                {
                    Value.Clear();

                    if (wrapper.Data is null)
                    {
                        return Value;
                    }

                    foreach (var task in wrapper.Data)
                    {
                        Value.Add(task);
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

    private static readonly AutomationTask[] Preset =
    [
        new()
        {
            Triggers =
            [
                new EventTrigger { Event = Events.ServerStart },
                new EventTrigger { Event = Events.ServerExitedNormally },
            ],
            Commands =
            [
                new()
                {
                    Type = CommandType.SendGroupMsg,
                    Body = "服务器（{server.name}）状态变更：{server.status}",
                },
            ],
            Description = "预设·服务器状态变更通知",
        },
        new()
        {
            Triggers = [new EventTrigger { Event = Events.ServerExitedUnexpectedly }],
            Commands =
            [
                new() { Type = CommandType.SendGroupMsg, Body = "服务器（{server.name}）异常退出" },
            ],
            Description = "预设·服务器异常退出通知",
        },
        new()
        {
            Triggers =
            [
                new MatchTrigger
                {
                    FieldType = MatchFieldType.GroupMsg,
                    Pattern = "^/开服$",
                    IsRegex = true,
                    RequireAdminPermission = true,
                },
            ],
            Commands = [new() { Type = CommandType.InputServer, Body = "start" }],
            Description = "预设·群服互通：开服",
        },
        new()
        {
            Triggers =
            [
                new MatchTrigger
                {
                    FieldType = MatchFieldType.GroupMsg,
                    Pattern = "^/关服$",
                    IsRegex = true,
                    RequireAdminPermission = true,
                },
            ],
            Commands = [new() { Type = CommandType.InputServer, Body = "stop" }],
            Description = "预设·群服互通：关服",
        },
        new()
        {
            Triggers =
            [
                new MatchTrigger
                {
                    IsEnabled = false,
                    FieldType = MatchFieldType.GroupMsg,
                    Pattern = "^/备份$",
                    IsRegex = true,
                    RequireAdminPermission = true,
                },
                new ScheduleTrigger { IsEnabled = false, CronExpression = "0 */2 * * *" },
            ],
            Commands = [new() { Type = CommandType.InputServer, Body = "backup" }],
            Description = "预设·备份（群内控制和每两小时执行一次）",
        },
        new()
        {
            Triggers = [new EventTrigger { Event = Events.SereinCrash }],
            Commands =
            [
                new()
                {
                    Type = CommandType.SendGroupMsg,
                    Body = "唔……发生了一点小问题(っ °Д °;)っ\n请查看Serein错误弹窗获取更多信息",
                },
            ],
            Description = "预设·Serein崩溃通知",
        },
        new()
        {
            Triggers =
            [
                new EventTrigger { Event = Events.PermissionDeniedFromGroupMsg },
                new EventTrigger { Event = Events.PermissionDeniedFromPrivateMsg },
            ],
            Commands = [new() { Type = CommandType.SendReply, Body = "你没有执行这个命令的权限" }],
            Description = "预设·权限不足反馈",
        },
    ];
}
