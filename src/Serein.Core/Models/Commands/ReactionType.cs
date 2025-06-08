namespace Serein.Core.Models.Commands;

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
