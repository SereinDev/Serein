using System;

namespace Serein.Core.Models.Bindings;

/// <summary>
/// 绑定失败异常
/// </summary>
public sealed class BindingFailureException(string message) : Exception(message);
