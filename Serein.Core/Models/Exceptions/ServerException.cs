using System;

namespace Serein.Core.Models.Exceptions;

public class ServerException : Exception
{
    public ServerException(string message)
        : base(message) { }
}
