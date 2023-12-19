namespace Serein.Core.Models.Settings;

public class AppSetting
{
    public ServerSetting Server { get; set; } = new();

    public BotSetting Bot { get; set; } = new();

    public SereinSetting Serein { get; set; } = new();

    public ReactionSetting Reaction { get; set; } = new();

    public AutoRunSetting AutoRun = new();

    public DevelopmentSetting Development = new();

    public FunctionSetting Function = new();

    public PagesSetting Pages = new();
}