using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Lite.Ui.Function;

namespace Serein.Lite.Loggers;

public class ConnectionLogger(ConnectionPage connectionPage) : IConnectionLogger
{
    private readonly ConnectionPage _connectionPage = connectionPage;

    public void Log(LogLevel level, string message) { }
}
