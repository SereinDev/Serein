using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Serein.Console.Models;

namespace Serein.Console.Services.Interaction;

public sealed class InputHandler(ILogger<InputHandler> logger, CommandProvider commandProvider)
{
    public void Handle(IReadOnlyList<string> args)
    {
        if (
            args.Count == 0
            || !commandProvider.Handlers.TryGetValue(args[0].ToLowerInvariant(), out var handler)
        )
        {
            logger.LogError("未知命令。请使用\"help\"查看所有命令");
            return;
        }

        var subCommand = args.Count > 1 ? args[1] : string.Empty;

        if (!string.IsNullOrWhiteSpace(subCommand))
        {
            if (!handler.SubCommands.ContainsKey(subCommand))
            {
                if (handler.SubCommands.Count > 0)
                {
                    logger.LogError(
                        "子命令无效。可用的值有：{}",
                        string.Join(", ", handler.SubCommands.Keys.Select(k => $"\"{k}\""))
                    );
                }
                else
                {
                    logger.LogError("此命令不支持子命令。使用\"help\"查看命令的用法");
                }

                return;
            }
        }
        else if (!handler.AllowToExecuteRootCommand)
        {
            logger.LogError("需要指定子命令。使用\"help\"查看命令的用法");
            return;
        }

        try
        {
            handler.Invoke(subCommand, args);
        }
        catch (InvalidArgumentException e)
        {
            logger.LogError("参数错误：{}", e.Message);
        }
        catch (Exception e)
        {
            logger.LogError(e, "运行命令时出现异常");
        }
    }
}
