using System;
using System.Collections.Generic;

using Microsoft.Extensions.Logging;

using Serein.Cli.Models;

namespace Serein.Cli.Services.Interaction;

public class InputHandler(ILogger<InputHandler> logger, CommandProvider commandProvider)
{
    private readonly ILogger<InputHandler> _logger = logger;
    private readonly CommandProvider _commandProvider = commandProvider;

    public void Handle(IReadOnlyList<string> args)
    {
        if (args.Count == 0)
        {
            _logger.LogError("未知命令。请使用\"help\"查看所有命令");
            return;
        }

        if (!_commandProvider.Handlers.TryGetValue(args[0].ToLowerInvariant(), out var parser))
            _logger.LogError("未知命令。请使用\"help\"查看所有命令");
        else
            try
            {
                parser.Invoke(args);
            }
            catch (InvalidArgumentException e)
            {
                _logger.LogError("参数错误：{}", e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "运行命令时出现异常");
            }
    }
}
