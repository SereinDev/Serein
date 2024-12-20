using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Serein.Core.Services.Permissions;
using Xunit;

namespace Serein.Tests.Services.PermissionGroup;

[Collection(nameof(Serein))]
public sealed class NodeTests
{
    private readonly PermissionManager _permissionManager;

    public NodeTests()
    {
        var app = HostFactory.BuildNew();
        _permissionManager = app.Services.GetRequiredService<PermissionManager>();
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
