using System.Collections.Generic;
using Serein.Core.Models.Commands;

namespace Serein.Core.Models.Settings;

public class Setting
{
    public static readonly IReadOnlyDictionary<ReactionType, string[]> DefaultReactions =
        new Dictionary<ReactionType, string[]>
        {
            [ReactionType.ServerStart] = ["[g]服务器{server.name}正在启动"],
            [ReactionType.ServerExitedNormally] = ["[g]服务器{server.name}已关闭"],
            [ReactionType.ServerExitedUnexpectedly] = ["[g]服务器{server.name}异常关闭"],
            [ReactionType.GroupIncreased] = ["[g]欢迎[CQ:at,qq={sender.id}]入群~"],
            [ReactionType.GroupDecreased] = ["[g]用户{sender.id}退出了群聊"],
            [ReactionType.GroupPoke] = ["[g]别戳我……(*/ω＼*)"],
            [ReactionType.BindingSucceeded] = ["[g]绑定成功"],
            [ReactionType.UnbindingSucceeded] = ["[g]解绑成功"],
            [ReactionType.SereinCrash] =
            [
                "[g]唔……发生了一点小问题(っ °Д °;)っ\n请查看Serein错误弹窗获取更多信息",
            ],
            [ReactionType.PermissionDeniedFromGroupMsg] =
            [
                "[g][CQ:at,qq={sender.id}] 你没有执行这个命令的权限",
            ],
            [ReactionType.PermissionDeniedFromPrivateMsg] = ["[p]你没有执行这个命令的权限"],
        };

    public ConnectionSetting Connection { get; init; } = new();

    public WebApiSetting WebApi { get; init; } = new();

    public ApplicationSetting Application { get; init; } = new();

    public Dictionary<ReactionType, string[]> Reactions { get; init; } = new(DefaultReactions);
}
