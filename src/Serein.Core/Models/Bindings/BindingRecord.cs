using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Serein.Core.Models.Bindings;

/// <summary>
/// 绑定记录
/// </summary>
public sealed class BindingRecord
{
    private DateTime _time;

    [Key]
    public required string UserId { get; set; }

    public List<string> GameIds { get; init; } = [];

    public string ShownName { get; set; } = string.Empty;

    public DateTime Time
    {
        get => _time;
        init => _time = value;
    }

    internal void Update()
    {
        _time = DateTime.Now;
    }
}
