using System;

using Serein.Pro.Views.Settings;

namespace Serein.Pro.ViewModels;

public class SettingViewModel
{
    public Type Connection { get; } = typeof(ConnectionSettingPage);

    public Type About { get; } = typeof(AboutPage);
}
