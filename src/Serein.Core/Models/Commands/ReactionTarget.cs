namespace Serein.Core.Models.Commands;

public readonly record struct ReactionTarget(
    string? ServerId = null,
    string? UserId = null,
    string? GroupId = null
);
