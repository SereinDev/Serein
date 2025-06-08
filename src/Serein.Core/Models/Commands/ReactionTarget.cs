namespace Serein.Core.Models.Commands;

public record ReactionTarget(
    string? ServerId = null,
    string? UserId = null,
    string? GroupId = null
);
