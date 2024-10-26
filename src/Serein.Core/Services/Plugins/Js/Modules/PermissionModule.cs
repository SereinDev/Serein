using Serein.Core.Services.Permissions;

namespace Serein.Core.Services.Plugins.Js.Modules;

public class PermissionModule(
    string id,
    PermissionManager permissionManager,
    GroupManager groupManager
)
{
    private readonly string _id = id;
    private readonly PermissionManager _permissionManager = permissionManager;

    public GroupManager Groups { get; } = groupManager;

    public string? this[string key] => _permissionManager.Permissions[key];

    public void Register(string key, string? description = null) =>
        _permissionManager.Register(_id, key, description);

    public void Unregister(string key) => _permissionManager.Unregister(_id, key);
}
