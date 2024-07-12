using System.Collections.Generic;

namespace Serein.Core.Models.Settings;

public class Setting
{
    private static readonly Dictionary<ReactionType, string[]> DefaultReactions =
        new()
        {
            [ReactionType.BindingSucceed] = ["[g][CQ:at,qq={ID}] 绑定成功"],
            [ReactionType.BindingFailedDueToOccupation] = ["[g][CQ:at,qq={ID}] 该游戏名称被占用"],
            [ReactionType.BindingFailedDueToInvalidArgument] =
            [
                "[g][CQ:at,qq={ID}] 该游戏名称无效"
            ],
            [ReactionType.BindingFailedDueToAlreadyBinded] = ["[g][CQ:at,qq={ID}] 你已经绑定过了"],
            [ReactionType.UnbindingSucceed] = ["[g][CQ:at,qq={ID}] 解绑成功"],
            [ReactionType.UnbindingFailed] = ["[g][CQ:at,qq={ID}] 该账号未绑定"],
            [ReactionType.BinderDisable] = ["[g][CQ:at,qq={ID}] 绑定功能已被禁用"],
            [ReactionType.ServerStart] = ["[g]服务器正在启动"],
            [ReactionType.ServerExitedNormally] = ["[g]服务器已关闭"],
            [ReactionType.ServerExitedUnexpectedly] = ["[g]服务器异常关闭"],
            [ReactionType.GroupIncreased] = ["[g]欢迎[CQ:at,qq={ID}]入群~"],
            [ReactionType.GroupDecreased] = ["[g]用户{ID}退出了群聊"],
            [ReactionType.GroupPoke] = ["[g]别戳我……(*/ω＼*)"],
            [ReactionType.SereinCrash] = ["[g]唔……发生了一点小问题(っ °Д °;)っ\n请查看Serein错误弹窗获取更多信息"],
            [ReactionType.PermissionDeniedFromGroupMsg] =
            [
                "[g][CQ:at,qq={ID}] 你没有执行这个命令的权限"
            ],
            [ReactionType.PermissionDeniedFromPrivateMsg] = ["[p]你没有执行这个命令的权限"],
        };

    public ConnectionSetting Connection { get; init; } = new();

    public SshSetting Ssh { get; init; } = new();

    public WebApiSetting WebApi { get; init; } = new();

    public ApplicationSetting Application { get; init; } = new();

    public Dictionary<ReactionType, string[]> Reactions { get; init; } = new(DefaultReactions);
}
