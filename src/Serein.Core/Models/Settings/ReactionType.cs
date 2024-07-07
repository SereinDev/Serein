namespace Serein.Core.Models.Settings;

public enum ReactionType
{
    Unknown,

    BindingSucceed,

    BindingFailedDueToOccupation,

    BindingFailedDueToInvalidArgument,

    BindingFailedDueToAlreadyBinded,

    UnbindingSucceed,

    UnbindingFailed,

    BinderDisable,

    ServerStart,

    ServerExitedNormally,

    ServerExitedUnexpectedly,

    GroupIncreased,

    GroupDecreased,

    GroupPoke,

    SereinCrash,

    PermissionDeniedFromPrivateMsg,

    PermissionDeniedFromGroupMsg
}
