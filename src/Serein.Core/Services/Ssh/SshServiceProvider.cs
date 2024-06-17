using System;
using System.Net;

using FxSsh;

using Microsoft.Extensions.Logging;

using Serein.Core.Services.Data;

namespace Serein.Core.Services.Ssh;

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
        _sshServer.ConnectionAccepted += ConnectionAccepted;
        _sshServer.ExceptionRasied += ExceptionRasied;
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

    private void ExceptionRasied(object? sender, Exception ex)
    {
        _logger.LogError(ex, string.Empty);
    }

    private void ConnectionAccepted(object? sender, Session e) { }
}
