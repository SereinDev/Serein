using System;
using System.IO;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Core.Utils.Extensions;

using Spectre.Console;

namespace Serein.Cli.Interaction.Commands;

[CommandDescription("version", "显示详细的版本和版权信息", Priority = -1)]
public class VersionCommand : Command
{
    public VersionCommand(IHost host)
        : base(host) { }

    private IOutputHandler Logger => Services.GetRequiredService<IOutputHandler>();

    public override void Parse(string[] args)
    {
        Logger.LogInformation("Copyright (C) 2022 Zaitonn. All rights reserved.");
        var table = new Table()
            .RoundedBorder()
            .AddColumns(
                new TableColumn("名称") { Alignment = Justify.Center },
                new(Assembly.GetExecutingAssembly().FullName ?? string.Empty)
                {
                    Alignment = Justify.Center
                }
            )
            .AddRow(
                "详细版本",
                Assembly
                    .GetExecutingAssembly()
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    ?.InformationalVersion ?? string.Empty
            );
        var commandlineArgs = Environment.GetCommandLineArgs();
        if (commandlineArgs.Length > 0 && File.Exists(commandlineArgs[0]))
            table
                .AddRow("文件名", Path.GetFileName(commandlineArgs[0]))
                .AddRow("MD5", File.ReadAllBytes(commandlineArgs[0]).CalculateMD5())
                .AddRow("创建时间", File.GetCreationTime(commandlineArgs[0]).ToString("o"))
                .AddRow("修改时间", File.GetLastWriteTime(commandlineArgs[0]).ToString("o"));

        AnsiConsole.Write(table);
    }
}
