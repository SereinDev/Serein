namespace Serein.Core.Models.Automations;

public enum Events
{
    None,

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
