namespace Serein.Core.Models.Settings;

public class Setting
{
    public ServerSetting Server { get; set; } = new();

    public BotSetting Bot { get; set; } = new();

    public ReactionSetting Reaction { get; set; } = new();

    public AutoRunSetting AutoRun { get; set; } = new();

    public FunctionSetting Function { get; set; } = new();

    public PagesSetting Pages { get; set; } = new();

    public ApplicationSetting Application { get; set; } = new();
}
