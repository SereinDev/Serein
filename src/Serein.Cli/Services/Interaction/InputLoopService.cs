using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

using PrettyPrompt;

using Serein.Core.Services.Data;
using Serein.Core.Utils.Extensions;
using Serein.Core.Utils;
using PrettyPrompt.Highlighting;
using Microsoft.Extensions.Logging;

namespace Serein.Cli.Services.Interaction;

public class InputLoopService(
    ILogger<InputLoopService> logger,
    SettingProvider settingProvider,
    CommandPromptCallbacks commandPromptCallbacks,
    InputHandler inputHandler
) : IHostedService
{
    private readonly ILogger<InputLoopService> _logger = logger;
    private readonly SettingProvider _settingProvider = settingProvider;
    private readonly CommandPromptCallbacks _commandPromptCallbacks = commandPromptCallbacks;
    private readonly InputHandler _inputHandler = inputHandler;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

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
        if (!Console.IsInputRedirected)
        {
            var prompt = new Prompt(
                PathConstants.ConsoleHistory,
                _commandPromptCallbacks,
                configuration: new(
                    completionBoxBorderFormat: new() { Foreground = AnsiColor.BrightBlack },
                    selectedCompletionItemBackground: AnsiColor.Rgb(30, 30, 30),
                    selectedTextBackground: AnsiColor.Rgb(20, 61, 102)
                )
            );

            while (!cancellationToken.IsCancellationRequested)
            {
                var response = prompt.ReadLineAsync().Await();

                if (response.IsSuccess)
                    ProcessInput(response.Text);
            }
        }
        else
        {
            _logger.LogWarning("当前输出流已被重定向，已关闭输入自动补全功能，但你仍可以输入\"help\"查看帮助页面");
            _logger.LogWarning("若要体验此功能，请在终端内运行 Serein.Cli");

            while (!cancellationToken.IsCancellationRequested)
                ProcessInput(Console.ReadLine());
        }
    }

    private void ProcessInput(string? input)
    {
        if (input is null)
            return;

        if (
            input.StartsWith(_settingProvider.Value.Application.CliCommandHeader)
            && !string.IsNullOrEmpty(_settingProvider.Value.Application.CliCommandHeader)
        )
            input = input[_settingProvider.Value.Application.CliCommandHeader.Length..];

        _inputHandler.Handle(
            input.Split(
                '\x20',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
            )
        );
    }
}
