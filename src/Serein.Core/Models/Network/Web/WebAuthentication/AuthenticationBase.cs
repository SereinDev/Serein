using System.Text.Json.Serialization;
using Serein.Core.Models.Abstractions;

namespace Serein.Core.Models.Network.Web.WebAuthentication;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(TokenAuthentication), typeDiscriminator: "token")]
[JsonDerivedType(typeof(UserAuthentication), typeDiscriminator: "user")]
public abstract class AuthenticationBase : NotifyPropertyChangedModelBase
{
    public string Description { get; set; } = string.Empty;
}
