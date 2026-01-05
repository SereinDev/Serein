namespace Serein.Core.Models.Automations;

public readonly record struct EventTarget(
    string? ServerId = null,
    string? UserId = null,
    string? GroupId = null
);
