using System;

using Serein.Plus.Pages;

namespace Serein.Plus.ViewModels;

public class ShellViewModel
{
    public Type Home { get; } = typeof(HomePage);

    public Type Server { get; } = typeof(ServerPage);

    public Type Connection { get; } = typeof(ConnectionPage);

    public Type Match { get; } = typeof(MatchPage);

    public Type Schedule { get; } = typeof(SchedulePage);

    public Type Plugin { get; } = typeof(PluginPage);
}

