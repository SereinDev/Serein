namespace Serein.Core.Models.Automations;

public enum Events
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
