namespace Serein.Core.Models.Network.Web.WebAuthentication;

public sealed class UserAuthentication : AuthenticationBase
{
    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
