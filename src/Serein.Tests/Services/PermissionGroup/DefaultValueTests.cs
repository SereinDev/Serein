using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.Core.Services.Data;
using Serein.Core.Services.Permissions;
using Xunit;

namespace Serein.Tests.Services.PermissionGroup;

[Collection(nameof(Serein))]
public sealed class DefaultValueTests
{
    private readonly IHost _app;
    private readonly GroupManager _groupManager;
    private readonly PermissionGroupProvider _permissionGroupProvider;

    public DefaultValueTests()
    {
        _app = HostFactory.BuildNew();
        _app.StartAsync();

        _groupManager = _app.Services.GetRequiredService<GroupManager>();
        _permissionGroupProvider = _app.Services.GetRequiredService<PermissionGroupProvider>();
    }

    [Fact]
    public void ShouldCreateDefaultGroup()
    {
        Assert.Contains("everyone", _permissionGroupProvider.Value.Keys);
        Assert.Equal(int.MinValue, _groupManager["everyone"].Priority);
    }
}
