using System;
using System.Collections.Generic;
using System.Net;

using FxSsh;
using FxSsh.Services;

using Microsoft.Extensions.Logging;

using Serein.Core.Services.Data;

namespace Serein.Core.Services.Network.Ssh;

public class SshHost(
    ILogger<SshHost> logger,
    SettingProvider settingProvider,
    SshServerKeysProvider sshServerKeysProvider
)
{
    private readonly ILogger _logger = logger;
    private readonly SettingProvider _settingProvider = settingProvider;
    private readonly SshServerKeysProvider _sshServerKeysProvider = sshServerKeysProvider;
    private SshServer? _sshServer;

    private readonly List<SshPty> _ptys = [];

    public void Start()
    {
        if (_sshServer is not null)
            throw new InvalidOperationException();

        _sshServer = new SshServer(
            new(
                IPAddress.Parse(_settingProvider.Value.Ssh.IpAddress),
                _settingProvider.Value.Ssh.Port,
                "SSH-2.0-FxSsh"
            )
        );

        _sshServer.AddHostKey("ssh-rsa", _sshServerKeysProvider.Value.Rsa);
        _sshServer.AddHostKey("ssh-dss", _sshServerKeysProvider.Value.Dss);

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

    private void OnWindowChange(object? sender, WindowChangeArgs e)
    {
        var pty = _ptys.Find((p) => p.Channel == e.Channel);

        if (pty is null)
            return;

        pty.WidthPx = e.WidthPixels;
        pty.HeightPx = e.HeightPixels;
        pty.WidthChars = e.WidthColumns;
        pty.HeightChars = e.HeightRows;
    }

    private void OnPtyReceived(object? sender, PtyArgs e)
    {
        var pty = new SshPty(e.Channel, e.Terminal)
        {
            WidthPx = e.WidthPx,
            HeightPx = e.HeightPx,
            WidthChars = e.WidthChars,
            HeightChars = e.HeightRows,
        };

        _ptys.Add(pty);

        e.Channel.CloseReceived += (_, _) => _ptys.Remove(pty);
    }

    private void OnEnvReceived(object? sender, EnvironmentArgs e) { }


    private void OnUserauth(object? sender, UserauthArgs e)
    {
        e.Result =
            !string.IsNullOrEmpty(e.Username)
            && !string.IsNullOrEmpty(e.Password)
            && _settingProvider.Value.Ssh.Users.TryGetValue(e.Username, out var pwd)
            && pwd == e.Password;
    }

    private void OnSucceed(object? sender, string e) { }

    private void OnDisconnected(object? sender, EventArgs e) { }

    private void OnKeysExchanged(object? sender, KeyExchangeArgs e) { }
}
