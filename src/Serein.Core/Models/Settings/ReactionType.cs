namespace Serein.Core.Models.Settings;

public enum ReactionType
{
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

    PermissionDeniedFromPrivateMsg,

    PermissionDeniedFromGroupMsg,

    SereinCrash,

}
