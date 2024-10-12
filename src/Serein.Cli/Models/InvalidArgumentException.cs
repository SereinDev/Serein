using System;

namespace Serein.Cli.Models;

public class InvalidArgumentException(string? message) : Exception(message);
