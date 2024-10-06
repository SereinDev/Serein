using Microsoft.Extensions.Logging;

namespace Serein.Core.Services.Plugins.Storages;

public class SessionStorage(ILogger<SessionStorage> logger) : StorageBase(logger);