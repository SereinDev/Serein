namespace Serein.Core.Models.Settings;

public record ReactionTarget(
    string? ServerId = null,
    string? UserId = null,
    string? GroupId = null
);
