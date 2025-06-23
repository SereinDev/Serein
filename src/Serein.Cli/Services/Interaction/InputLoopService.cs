using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PrettyPrompt;
using PrettyPrompt.Highlighting;
using Serein.Cli.Utils;
using Serein.Core.Services.Data;
using Serein.Core.Services.Servers;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;

namespace Serein.Cli.Services.Interaction;

public sealed class InputLoopService(
    ILogger<InputLoopService> logger,
    IServiceProvider serviceProvider,
    SettingProvider settingProvider,
    ServerManager serverManager,
    CommandPromptCallbacks commandPromptCallbacks,
    InputHandler inputHandler
) : IHostedService
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly Lazy<ServerSwitcher> _serverSwitcher = new(
        serviceProvider.GetRequiredService<ServerSwitcher>
    );

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(() => Loop(_cancellationTokenSource.Token), _cancellationTokenSource.Token);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }

    private void Loop(CancellationToken cancellationToken)
    {
        if (!Console.IsInputRedirected && CliConsole.IsColorful)
        {
            var flag = false;
            var prompt = new Prompt(
                PathConstants.ConsoleHistory,
                commandPromptCallbacks,
                configuration: new(
                    completionBoxBorderFormat: new() { Foreground = AnsiColor.BrightBlack },
                    selectedCompletionItemBackground: AnsiColor.Rgb(30, 30, 30),
                    selectedTextBackground: AnsiColor.Rgb(20, 61, 102)
                )
            );

            while (!cancellationToken.IsCancellationRequested)
            {
                if (
                    !string.IsNullOrEmpty(_serverSwitcher.Value.CurrentId)
                    && serverManager.Servers.TryGetValue(
                        _serverSwitcher.Value.CurrentId,
                        out var server
                    )
                    && server.Status
                )
                {
                    flag = true;
                    Console.CancelKeyPress += IgnoreCtrlC;
                    ProcessInput(Console.ReadLine());
                }
                else
                {
                    if (flag)
                    {
                        Console.CancelKeyPress -= IgnoreCtrlC;
                        flag = false;
                    }

                    try
                    {
                        var response = prompt.ReadLineAsync().WaitForResult();

                        if (response.IsSuccess)
                        {
                            ProcessInput(response.Text);
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError(
                            e,
                            "读取输入时发生错误。若此错误持续出现，请尝试关闭彩色输出"
                        );
                    }
                }
            }
        }
        else
        {
            logger.LogWarning(
                "当前输出流已被重定向 或 你在启动时指定了禁用彩色输出参数，输入自动补全功能已关闭，但你仍可以输入\"help\"查看帮助页面"
            );
            logger.LogWarning("若要体验此功能，请在终端内运行 Serein.Cli");

            while (!cancellationToken.IsCancellationRequested)
            {
                ProcessInput(Console.ReadLine());
            }
        }
    }

    private static void IgnoreCtrlC(object? sender, ConsoleCancelEventArgs e) => e.Cancel = true;

    private void ProcessInput(string? input)
    {
        if (input is null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(_serverSwitcher.Value.CurrentId))
        {
            if (
                serverManager.Servers.TryGetValue(_serverSwitcher.Value.CurrentId, out var server)
                && server.Status
            )
            {
                if (input.StartsWith(settingProvider.Value.Application.CliCommandHeader))
                {
                    input = input[settingProvider.Value.Application.CliCommandHeader.Length..];
                }
                else
                {
                    server.Input(input);
                    return;
                }
            }
        }

        inputHandler.Handle(
            input.Split(
                '\x20',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
            )
        );
    }
}
