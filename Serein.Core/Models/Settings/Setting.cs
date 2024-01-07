using System.ComponentModel;

using PropertyChanged;

namespace Serein.Core.Models.Settings;

public class Setting
{
    public ServerSetting Server { get; set; } = new();

    public ConnectionSetting Connection { get; set; } = new();

    public ReactionSetting Reaction { get; set; } = new();

    public AutoRunSetting AutoRun { get; set; } = new();

    public FunctionSetting Function { get; set; } = new();

    public PagesSetting Pages { get; set; } = new();

    public ApplicationSetting Application { get; set; } = new();
}
