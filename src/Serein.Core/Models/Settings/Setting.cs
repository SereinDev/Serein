namespace Serein.Core.Models.Settings;

public class Setting
{
    public ConnectionSetting Connection { get; init; } = new();

    public WebApiSetting WebApi { get; init; } = new();

    public ApplicationSetting Application { get; init; } = new();
}
