using Microsoft.Extensions.Logging;

namespace Serein.Core.Models.Abstractions;

public interface IPluginLogger
{
    void Log(LogLevel level, string name, string message);
}
