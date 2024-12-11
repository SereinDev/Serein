namespace Serein.Core.Models.Settings;

public enum ReactionType
{
    ServerStart,

    ServerExitedNormally,

    ServerExitedUnexpectedly,

    GroupIncreased,

    GroupDecreased,

    GroupPoke,

    BindingSucceeded,

    UnbindingSucceeded,

    PermissionDeniedFromPrivateMsg,

    PermissionDeniedFromGroupMsg,

    SereinCrash,
}
