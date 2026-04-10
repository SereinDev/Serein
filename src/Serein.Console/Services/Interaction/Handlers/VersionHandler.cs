using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Serein.Console.Utils;
using Spectre.Console;

namespace Serein.Console.Services.Interaction.Handlers;

public sealed class VersionHandler : CommandHandler
{
    public override string Name { get; } = "版本";

    public override string[] Description { get; } = ["查看版本信息", "查看版权声明"];

    public override string RootCommand { get; } = "version";

    public override Dictionary<string, string> SubCommands { get; } = [];

    public override bool AllowToExecuteRootCommand { get; } = true;

    public override void Invoke(string subCommand, IReadOnlyList<string> args)
    {
        var table = new Table()
            .AddColumn(new("") { Alignment = Justify.Center })
            .AddColumn("")
            .AddRow("程序集", typeof(Program).Assembly.ToString())
            .AddRow("详细版本", ThisAssembly.Info.InformationalVersion.EscapeMarkup())
            .AddRow("分支", ThisAssembly.Git.Branch.EscapeMarkup())
            .AddRow("编译时根目录", ThisAssembly.Git.Root.EscapeMarkup())
            .AddRow(
                "运行时",
                $"{RuntimeInformation.FrameworkDescription} ({RuntimeInformation.RuntimeIdentifier})"
            )
            .HideHeaders()
            .RoundedBorder()
            .Expand();

        SimpleConsole.Log(LogLevel.Information, table);
        SimpleConsole.Log(LogLevel.Information, "Copyright (C) 2022 Zaitonn. All rights reserved.");
    }
}
