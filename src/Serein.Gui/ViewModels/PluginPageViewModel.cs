using System;

namespace Serein.Gui.ViewModels;

public class PluginPageViewModel
{
    public Type GetDefaultPageType()
    {
        return typeof(Pages.PluginConsolePage);
    }

    public Type ResolvePageType(object? tag)
    {
        return tag as Type ?? typeof(Pages.NotImplPage);
    }
}
