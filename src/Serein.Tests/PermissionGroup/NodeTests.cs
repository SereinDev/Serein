using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Services.Data;
using Serein.Core.Services.Permissions;

using Xunit;

namespace Serein.Tests.PermissionGroup;

[Collection(nameof(Serein))]
public sealed class NodeTests : IDisposable
{
    private readonly IHost _app;
    private readonly PermissionManager _permissionManager;
    private readonly GroupManager _groupManager;
    private readonly PermissionGroupProvider _permissionGroupProvider;

    public NodeTests()
    {
        _app = HostFactory.BuildNew();
        _app.StartAsync();

        _groupManager = _app.Services.GetRequiredService<GroupManager>();
        _permissionManager = _app.Services.GetRequiredService<PermissionManager>();
        _permissionGroupProvider = _app.Services.GetRequiredService<PermissionGroupProvider>();
    }

    public void Dispose()
    {
        _app.StopAsync();
        _app.Dispose();
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("abc.abc")]
    [InlineData("a-b")]
    [InlineData("a-c.a-c")]
    [InlineData("a11.b22")]
    public void ShouldBeAbleToRegisterPermissions(string node)
    {
        _permissionManager.Register(nameof(NodeTests), node);
    }

    [Theory]
    [InlineData("")]
    [InlineData("abc.")]
    [InlineData("a..b")]
    [InlineData("114514.abc")]
    public void ShouldThrowWhenRegisteringPermissionsWithInvalidKey(string node)
    {
        Assert.Throws<ArgumentException>(
            () => _permissionManager.Register(nameof(NodeTests), node)
        );
    }

    [Fact]
    public void ShouldThrowWhenUnregisterNonExistentPermission()
    {
        Assert.Throws<KeyNotFoundException>(
            () => _permissionManager.Unregister(nameof(NodeTests), "test")
        );
    }
}
