using System;
using Serein.Gui.Pages;

namespace Serein.Gui.ViewModels;

public class PluginPageViewModel
{
    public Type GetDefaultPageType()
    {
        return typeof(PluginConsolePage);
    }

    public Type ResolvePageType(object? tag)
    {
        return tag as Type ?? typeof(NotImplPage);
    }
}
