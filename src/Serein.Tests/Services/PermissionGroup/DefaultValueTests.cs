using Microsoft.Extensions.DependencyInjection;
using Serein.Core.Services.Data;
using Serein.Core.Services.Permissions;
using Xunit;

namespace Serein.Tests.Services.PermissionGroup;

[Collection(nameof(Serein))]
public sealed class DefaultValueTests
{
    private readonly GroupManager _groupManager;
    private readonly PermissionGroupProvider _permissionGroupProvider;

    public DefaultValueTests()
    {
        var app = HostFactory.BuildNew();
        app.StartAsync();

        _groupManager = app.Services.GetRequiredService<GroupManager>();
        _permissionGroupProvider = app.Services.GetRequiredService<PermissionGroupProvider>();
    }

    [Fact]
    public void ShouldCreateDefaultGroup()
    {
        Assert.Contains("everyone", _permissionGroupProvider.Value.Keys);
        Assert.Equal(int.MinValue, _groupManager["everyone"].Priority);
    }
}
