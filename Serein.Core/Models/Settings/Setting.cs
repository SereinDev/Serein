using System.ComponentModel;

namespace Serein.Core.Models.Settings;

public class Setting : INotifyPropertyChanged
{
    public ServerSetting Server { get; init; } = new();

    public ConnectionSetting Connection { get; init; } = new();

    public ReactionSetting Reaction { get; init; } = new();

    public AutoRunSetting AutoRun { get; init; } = new();

    public FunctionSetting Function { get; init; } = new();

    public PagesSetting Pages { get; set; } = new();

    public ApplicationSetting Application { get; set; } = new();

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
