using System;

namespace Serein.Cli.Models;

public class InvalidArgumentException : Exception
{
    public InvalidArgumentException(string? message)
        : base(message) { }
}
