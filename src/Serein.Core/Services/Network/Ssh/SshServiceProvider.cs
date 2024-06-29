using System;
using System.Net;

using FxSsh;
using FxSsh.Services;

using Microsoft.Extensions.Logging;

using Serein.Core.Services.Data;

namespace Serein.Core.Services.Network.Ssh;

public class SshServiceProvider(ILogger logger, SettingProvider settingProvider)
{
    private readonly ILogger _logger = logger;
    private readonly SettingProvider _settingProvider = settingProvider;
    private SshServer? _sshServer;

    public void Start()
    {
        if (_sshServer is not null)
            throw new InvalidOperationException();

        _sshServer = new SshServer(
            new(
                IPAddress.Parse(_settingProvider.Value.Ssh.IpAddress),
                _settingProvider.Value.Ssh.Port,
                "SSH-2.0-FxSsh" // default
            )
        );
        _sshServer.ConnectionAccepted += OnConnectionAccepted;
        _sshServer.ExceptionRasied += OnExceptionRasied;
        _sshServer.Start();
    }

    public void Stop()
    {
        if (_sshServer is null)
            throw new InvalidOperationException();

        _sshServer.Stop();
        _sshServer.Dispose();
        _sshServer = null;
    }

    private void OnExceptionRasied(object? sender, Exception e)
    {
        _logger.LogError(e, string.Empty);
    }

    private void OnConnectionAccepted(object? sender, Session e)
    {
        e.ServiceRegistered += OnServiceRegistered;
        e.Disconnected += OnDisconnected;
        e.KeysExchanged += OnKeysExchanged;
    }

    private void OnServiceRegistered(object? sender, SshService sshService)
    {
        if (sshService is UserauthService userauthService)
        {
            userauthService.Succeed += OnSucceed;
            userauthService.Userauth += OnUserauth;
        }
        else if (sshService is ConnectionService connectionService)
        {
            connectionService.EnvReceived += OnEnvReceived;
            connectionService.PtyReceived += OnPtyReceived;
            connectionService.WindowChange += OnWindowChange;
            connectionService.CommandOpened += OnCommandOpened;
        }
        else
            throw new NotSupportedException();
    }

    private void OnCommandOpened(object? sender, CommandRequestedArgs e) { }

    private void OnWindowChange(object? sender, WindowChangeArgs e) { }

    private void OnPtyReceived(object? sender, PtyArgs e) { }

    private void OnUserauth(object? sender, UserauthArgs e)
    {
        e.Result =
            !string.IsNullOrEmpty(e.Username)
            && !string.IsNullOrEmpty(e.Password)
            && _settingProvider.Value.Ssh.Users.TryGetValue(e.Username, out var pwd)
            && pwd == e.Password;
    }

    private void OnSucceed(object? sender, string e) { }

    private void OnEnvReceived(object? sender, EnvironmentArgs e) { }

    private void OnDisconnected(object? sender, EventArgs e) { }

    private void OnKeysExchanged(object? sender, KeyExchangeArgs e) { }
}
