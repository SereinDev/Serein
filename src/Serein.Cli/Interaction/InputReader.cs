using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Cli.Models;
using Serein.Core;
using Serein.Core.Services.Data;

namespace Serein.Cli.Interaction;

public class InputReader : IHostedService
{
    private readonly Dictionary<string, Command> _commandParser;
    private readonly string _allCommands;
    private static readonly char[] Separator = ['\x20'];
    private readonly IHost _host;
    private readonly ILogger _logger;
    private readonly SettingProvider _settingProvider;


    public InputReader(IHost host, ILogger logger, SettingProvider settingProvider)
    {
        _host = host;
        _logger = logger;
        _settingProvider = settingProvider;

        var stringBuilder = new StringBuilder();
        var attributes = new List<(CommandDescriptionAttribute, CommandUsageAttribute[])>();
        var commands = new Dictionary<string, Command>();

        stringBuilder.AppendLine($"Serein.Cli {SereinApp.Version}");
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            var attribute = type.GetCustomAttribute<CommandDescriptionAttribute>();
            if (attribute is null)
                continue;

            var handler = (Command?)Activator.CreateInstance(type, _host);

            if (handler is null)
                continue;

            commands[attribute.RootCommand] = handler;
            attributes.Add(
                (attribute, type.GetCustomAttributes<CommandUsageAttribute>().ToArray())
            );
        }

        attributes.Sort((a, b) => b.Item1.Priority - a.Item1.Priority);
        foreach (var attributePair in attributes)
        {
            stringBuilder.AppendLine(
                $"▪ {attributePair.Item1.RootCommand}  {attributePair.Item1.Description}"
            );

            foreach (var usage in attributePair.Item2)
                stringBuilder.AppendLine($"  ▫ {usage.Example}  {usage.Description}");
        }

        _allCommands = stringBuilder.ToString();
        _commandParser = commands;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(() => Loop(cancellationToken), cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void Loop(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var input = Console.ReadLine();

            if (input is null)
                continue;

            // if (
            //     ServerManager.Status == ServerStatus.Running
            //     && !input.StartsWith(Setting.Application.CliCommandHeader)
            // )
            // {
            //     ServerManager.Input(input, null, true);
            //     continue;
            // }

            if (
                input.StartsWith(_settingProvider.Value.Application.CliCommandHeader)
                && !string.IsNullOrEmpty(_settingProvider.Value.Application.CliCommandHeader)
            )
                input = input[_settingProvider.Value.Application.CliCommandHeader.Length..];

            if (Parse(input.Trim(), out var args))
                Handle(args);
            else
                _logger.LogError("语法错误：含有未闭合的冒号（\"）。若要作为参数的一部分传输，请使用\\\"进行转义");
        }
    }

    public static bool Parse(string line, [NotNullWhen(true)] out List<string>? result)
    {
        var args = new List<string>();

        if (!line.Contains('"') || !line.Contains(' '))
            args.AddRange(line.Split(Separator, options: StringSplitOptions.RemoveEmptyEntries));
        else
        {
            var inColon = false;
            var colonIndex = -1;

            var stringBuilder = new StringBuilder();
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];

                switch (c)
                {
                    case '\x20':
                        if (!inColon)
                        {
                            args.Add(stringBuilder.ToString());
                            stringBuilder.Clear();
                        }
                        else
                            stringBuilder.Append(c);
                        break;

                    case '"':
                        if (inColon && colonIndex > 0)
                            colonIndex = -1;

                        if (i > 1 && line[i - 1] != '\\')
                        {
                            inColon = !inColon;
                            colonIndex = i;
                        }
                        else
                        {
                            stringBuilder.Remove(stringBuilder.Length - 1, 1);
                            stringBuilder.Append(c);
                        }

                        break;

                    default:
                        stringBuilder.Append(c);
                        break;
                }
            }

            if (inColon)
            {
                result = null;
                return false;
            }
            if (stringBuilder.Length != 0)
                args.Add(stringBuilder.ToString());
        }
        result = args;
        return true;
    }

    private void Handle(List<string> args)
    {
        if (args.Count == 0)
        {
            _logger.LogError("未知命令。请使用\"help\"查看所有命令");
            return;
        }

        if (args[0] == "help" || args[0] == "?")
            _logger.LogInformation("{}", _allCommands);
        else if (!_commandParser.TryGetValue(args[0], out var parser))
            _logger.LogError("未知命令。请使用\"help\"查看所有命令");
        else
            try
            {
                parser.Parse([.. args]);
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
