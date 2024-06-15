using Microsoft.Extensions.Logging;

namespace Serein.Core.Models.Output;

public interface IConnectionLogger
{
    void Log(LogLevel level, string message);
}
