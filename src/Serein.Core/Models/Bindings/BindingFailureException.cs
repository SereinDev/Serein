using System;

namespace Serein.Core.Models.Bindings;

/// <summary>
/// 绑定失败异常
/// </summary>
public class BindingFailureException(string message) : Exception(message);
