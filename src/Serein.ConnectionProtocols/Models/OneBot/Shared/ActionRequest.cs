namespace Serein.ConnectionProtocols.Models.OneBot.Shared;

public class ActionRequest<T>
    where T : notnull
{
    public string Action { get; set; } = string.Empty;

    public T? Params { get; set; }
}
