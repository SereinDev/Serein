using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Services.Data;
using Serein.Core.Services.Permissions;

using Xunit;

namespace Serein.Tests.PermissionGroup;

[Collection(nameof(Serein))]
public sealed class LogicTests : IDisposable
{
    private readonly IHost _app;
    private readonly PermissionManager _permissionManager;
    private readonly GroupManager _groupManager;
    private readonly PermissionGroupProvider _permissionGroupProvider;

    public LogicTests()
    {
        _app = HostFactory.BuildNew();
        _app.StartAsync();

        _groupManager = _app.Services.GetRequiredService<GroupManager>();
        _permissionManager = _app.Services.GetRequiredService<PermissionManager>();
        _permissionGroupProvider = _app.Services.GetRequiredService<PermissionGroupProvider>();
    }

    private static bool? TryGet(Dictionary<string, bool?> dict, string key)
    {
        return dict.TryGetValue(key, out bool? result) ? result : null;
    }

    public void Dispose()
    {
        _app.StopAsync();
        _app.Dispose();
    }

    [Theory]
    [InlineData(nameof(NodeTests) + ".*", true, true)]
    [InlineData(nameof(NodeTests) + ".foo.*", true, null)]
    public void ShouldWorkWithWildcard(string permissionKey, bool? result1, bool? result2)
    {
        _permissionManager.Register(nameof(NodeTests), "foo.foo");
        _permissionManager.Register(nameof(NodeTests), "bar");

        _groupManager.Add(nameof(ShouldWorkWithWildcard), new()
        {
            Members = [114514],
            Nodes = new()
            {
                [permissionKey] = true
            }
        });

        var result = _groupManager.GetAllNodes(114514);
        Assert.Equal(result1, TryGet(result, nameof(NodeTests) + ".foo.foo"));
        Assert.Equal(result2, TryGet(result, nameof(NodeTests) + ".bar"));
    }

    [Fact]
    public void ShouldInheritFromDefaultGroup()
    {
        _groupManager.Add("1", new()
        {
            Members = [114514],
        });
        _groupManager["everyone"].Nodes[nameof(NodeTests) + ".foo.bar"] = true;

        Assert.Equal(
            true,
            TryGet(_groupManager.GetAllNodes(114514), nameof(NodeTests) + ".foo.bar")
            );
    }

    [Fact]
    public void ShouldWorkWithInheritance()
    {
        _groupManager.Add("1", new()
        {
            Nodes =
            {
                ["foo.bar"] = true
            }
        });
        _groupManager.Add("2", new()
        {
            Members = [114514],
            Parents = ["1"]
        });

        Assert.Equal(
            true,
            TryGet(_groupManager.GetAllNodes(114514), "foo.bar")
            );
    }

    [Theory]
    [InlineData(1, -1, true)]
    [InlineData(-1, 1, false)]
    [InlineData(0, 0, false)]
    public void ShouldWorkWithInheritanceAndPriority(int priority1, int priority2, bool expected)
    {
        _groupManager.Add("1", new()
        {
            Nodes =
            {
                ["foo.bar"] = true
            },
            Priority = priority1
        });
        _groupManager.Add("2", new()
        {
            Members = [114514],
            Nodes =
            {
                ["foo.bar"] = false
            },
            Parents = ["1"],
            Priority = priority2
        });

        Assert.Equal(
            expected,
            TryGet(_groupManager.GetAllNodes(114514), "foo.bar")
            );
    }

    [Fact]
    public void ShouldAvoidCyclingInheritance()
    {
        _groupManager.Add("1", new()
        {
            Nodes =
            {
                ["a.b"] = true
            },
            Parents = ["2"]
        });
        _groupManager.Add("2", new()
        {
            Members = [114514],
            Parents = ["1"]
        });

        Assert.Equal(true, TryGet(_groupManager.GetAllNodes(114514), "a.b"));
    }
}
