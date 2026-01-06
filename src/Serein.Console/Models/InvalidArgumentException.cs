using System;

namespace Serein.Console.Models;

public class InvalidArgumentException(string? message) : Exception(message);
