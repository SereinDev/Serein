using Serein.Core.Services.Permissions;

namespace Serein.Core.Services.Plugins.Js.Properties;

public sealed class PermissionProperty
{
    private readonly string _id;
    private readonly PermissionManager _permissionManager;

    internal PermissionProperty(
        string id,
        PermissionManager permissionManager,
        GroupManager groupManager
    )
    {
        _id = id;
        _permissionManager = permissionManager;
        Groups = groupManager;
    }

    public GroupManager Groups { get; }

    public string? this[string key] => _permissionManager.Nodes[key];

    public void Register(string node, string? description = null) =>
        _permissionManager.Register(_id, node, description);

    public void Unregister(string node) => _permissionManager.Unregister(_id, node);
}
