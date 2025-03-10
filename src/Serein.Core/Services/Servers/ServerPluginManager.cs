using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serein.Core.Models.Server;

namespace Serein.Core.Services.Servers;

/// <summary>
/// 服务器插件管理器
/// </summary>
public sealed class ServerPluginManager
{
    /// <summary>
    /// 文件夹
    /// </summary>
    public static readonly IReadOnlyList<string> AcceptableDirectories =
    [
        "mods",
        "plugins",
        "mod",
        "plugin",
    ];

    /// <summary>
    /// 可接受的扩展名
    /// </summary>
    public static readonly IReadOnlyList<string> AcceptableExtensions =
    [
        ".dll",
        ".jar",
        ".js",
        ".lua",
        ".py",
    ];

    /// <summary>
    /// 禁用插件的扩展名
    /// </summary>
    public static readonly string DisabledPluginExtension = ".disabled";

    public event EventHandler? Updated;
    public IReadOnlyList<ServerPlugin> Plugins => _plugins;
    public string? CurrentPluginsDirectory { get; private set; }

    private readonly Server _server;
    private readonly List<ServerPlugin> _plugins;
    private bool _updating;

    internal ServerPluginManager(Server server)
    {
        _server = server;
        _plugins = [];

        _server.Configuration.PropertyChanged += (_, _) => Update();
        Update();
    }

    /// <summary>
    /// 更新插件
    /// </summary>
    /// <exception cref="NotSupportedException"></exception>
    public void Update()
    {
        if (_updating)
        {
            return;
        }

        lock (_plugins)
        {
            try
            {
                _updating = true;

                CurrentPluginsDirectory = null;
                _plugins.Clear();

                if (!File.Exists(_server.Configuration.FileName))
                {
                    return;
                }

                foreach (var file in EnumerateFiles())
                {
                    var extension = Path.GetExtension(file).ToLowerInvariant();
                    if (extension == DisabledPluginExtension)
                    {
                        extension = Path.GetExtension(Path.GetFileNameWithoutExtension(file))
                            .ToLowerInvariant();
                    }

                    if (AcceptableExtensions.Contains(extension))
                    {
                        _plugins.Add(
                            new(
                                file,
                                extension switch
                                {
                                    ".dll" => PluginType.Library,
                                    ".jar" => PluginType.Java,
                                    ".js" => PluginType.JavaScript,
                                    ".lua" => PluginType.Lua,
                                    ".py" => PluginType.Python,
                                    _ => throw new NotSupportedException(),
                                }
                            )
                        );
                    }
                }
            }
            finally
            {
                _updating = false;
            }

            Updated?.Invoke(this, EventArgs.Empty);
        }
    }

    private IEnumerable<string> EnumerateFiles()
    {
        var root = Path.GetDirectoryName(_server.Configuration.FileName) ?? string.Empty;

        foreach (var dir in AcceptableDirectories)
        {
            var path = Path.GetFullPath(Path.Join(root, dir));

            if (!Directory.Exists(path))
            {
                continue;
            }

            CurrentPluginsDirectory = path;
            return Directory.EnumerateFiles(path, "*.*");
        }

        return [];
    }

    /// <summary>
    /// 添加插件
    /// </summary>
    /// <param name="paths">文件路径</param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Add(params string[] paths)
    {
        if (
            string.IsNullOrEmpty(CurrentPluginsDirectory)
            || !Directory.Exists(CurrentPluginsDirectory)
        )
        {
            throw new InvalidOperationException("无法获取插件文件夹");
        }

        foreach (var path in paths)
        {
            File.Copy(path, Path.Join(CurrentPluginsDirectory, Path.GetFileName(path)));
        }
    }

    /// <summary>
    /// 删除插件
    /// </summary>
    /// <param name="serverPlugin">插件对象</param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public void Remove(ServerPlugin serverPlugin)
    {
        if (!_plugins.Remove(serverPlugin))
        {
            throw new InvalidOperationException("无法通过此插件管理器删除该插件");
        }

        if (!serverPlugin.FileInfo.Exists)
        {
            throw new FileNotFoundException($"插件\"{serverPlugin.Path}\"不存在");
        }

        serverPlugin.FileInfo.Delete();
    }

    /// <summary>
    /// 禁用插件
    /// </summary>
    /// <param name="serverPlugin">插件对象</param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public void Disable(ServerPlugin serverPlugin)
    {
        if (!Plugins.Contains(serverPlugin))
        {
            throw new InvalidOperationException("无法通过此插件管理器禁用该插件");
        }

        if (!serverPlugin.IsEnabled)
        {
            throw new InvalidOperationException("不能禁用已经被禁用的插件");
        }

        if (!serverPlugin.FileInfo.Exists)
        {
            throw new FileNotFoundException($"插件\"{serverPlugin.Path}\"不存在");
        }

        serverPlugin.FileInfo.MoveTo(serverPlugin.Path + DisabledPluginExtension);
    }

    /// <summary>
    /// 启用插件
    /// </summary>
    /// <param name="serverPlugin">插件对象</param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public void Enable(ServerPlugin serverPlugin)
    {
        if (!Plugins.Contains(serverPlugin))
        {
            throw new InvalidOperationException("无法通过此插件管理器启用该插件");
        }

        if (serverPlugin.IsEnabled)
        {
            throw new InvalidOperationException("不能禁用未被禁用的插件");
        }

        if (!serverPlugin.FileInfo.Exists)
        {
            throw new FileNotFoundException($"插件\"{serverPlugin.Path}\"不存在");
        }

        if (Path.GetExtension(serverPlugin.Path) == DisabledPluginExtension)
        {
            serverPlugin.FileInfo.MoveTo(
                Path.Join(
                    CurrentPluginsDirectory,
                    Path.GetFileNameWithoutExtension(serverPlugin.Path)
                )
            );
        }
    }
}
