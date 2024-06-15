using Microsoft.Extensions.Logging;

namespace Serein.Core.Models.Output;

public interface IPluginLogger
{
    void Log(LogLevel level, string name, string message);
}
