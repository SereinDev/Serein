using System.Security.Principal;
using System.Timers;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Commands;
using Serein.Core.Services.Data;

namespace Serein.Gui.Services;

public sealed class TitleUpdater : NotifyPropertyChangedModelBase
{
    private const string Name = "Serein.Gui";
    private readonly SettingProvider _settingProvider;
    private readonly CommandParser _commandParser;
    private readonly Timer _timer;
    private readonly bool _isSystemAdmin;

    public TitleUpdater(SettingProvider settingProvider, CommandParser commandParser)
    {
        _isSystemAdmin = GetIsSystemAdmin();
        _settingProvider = settingProvider;
        _commandParser = commandParser;
        _timer = new(2000) { Enabled = true };
        _timer.Elapsed += (_, _) => Update();
        _settingProvider.Value.Application.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(ApplicationSetting.CustomTitle))
            {
                Update();
            }
        };
    }

    public string CustomTitle { get; private set; } = Name;

    public void Update()
    {
        var postfix = _commandParser.ApplyVariables(
            _settingProvider.Value.Application.CustomTitle,
            null
        );

        if (string.IsNullOrEmpty(postfix) && _isSystemAdmin)
        {
            postfix = "管理员";
        }

        CustomTitle = string.IsNullOrEmpty(postfix) ? Name : $"{Name} - {postfix}";
    }

    private static bool GetIsSystemAdmin()
    {
        try
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(
                WindowsBuiltInRole.Administrator
            );
        }
        catch
        {
            return false;
        }
    }
}
