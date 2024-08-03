using System;

namespace Serein.Core.Models.Bindings;

public class BindingFailureException(string message) : Exception(message);