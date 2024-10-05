using System;

using Serein.Plus.Pages;

namespace Serein.Plus.ViewModels;

public class PluginViewModel
{
    public Type Console { get; } = typeof(PluginConsolePage);

    public Type List { get; } = typeof(PluginListPage);

    //public Type Market { get; } = typeof(PluginMarketPage);
}
