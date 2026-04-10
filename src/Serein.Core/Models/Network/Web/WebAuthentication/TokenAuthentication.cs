namespace Serein.Core.Models.Network.Web.WebAuthentication;

public sealed class TokenAuthentication : AuthenticationBase
{
    public string Token { get; set; } = string.Empty;
}
