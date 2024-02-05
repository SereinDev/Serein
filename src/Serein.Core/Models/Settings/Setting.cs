using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Serein.Core.Models.Settings;

public partial class Setting
{
    private static Dictionary<ReactionType, ObservableCollection<string>> DefaultReactions =>
        new()
        {
            [ReactionType.BindingSucceed] = new() { "[g][CQ:at,qq={ID}] 绑定成功" },
            [ReactionType.BindingFailedDueToOccupation] = new() { "[g][CQ:at,qq={ID}] 该游戏名称被占用" },
            [ReactionType.BindingFailedDueToInvalidArgument] = new()
            {
                "[g][CQ:at,qq={ID}] 该游戏名称无效"
            },
            [ReactionType.BindingFailedDueToAlreadyBinded] = new() { "[g][CQ:at,qq={ID}] 你已经绑定过了" },
            [ReactionType.UnbindingSucceed] = new() { "[g][CQ:at,qq={ID}] 解绑成功" },
            [ReactionType.UnbindingFailed] = new() { "[g][CQ:at,qq={ID}] 该账号未绑定" },
            [ReactionType.BinderDisable] = new() { "[g][CQ:at,qq={ID}] 绑定功能已被禁用" },
            [ReactionType.ServerStart] = new() { "[g]服务器正在启动" },
            [ReactionType.ServerExitedNormally] = new() { "[g]服务器已关闭" },
            [ReactionType.ServerExitedUnexpectedly] = new() { "[g]服务器异常关闭" },
            [ReactionType.GroupIncreased] = new() { "[g]欢迎[CQ:at,qq={ID}]入群~" },
            [ReactionType.GroupDecreased] = new() { "[g]用户{ID}退出了群聊" },
            [ReactionType.GroupPoke] = new() { "[g]别戳我……(*/ω＼*)" },
            [ReactionType.SereinCrash] = new() { "[g]唔……发生了一点小问题(っ °Д °;)っ\n请查看Serein错误弹窗获取更多信息" },
            [ReactionType.PermissionDeniedFromGroupMsg] = new()
            {
                "[g][CQ:at,qq={ID}] 你没有执行这个命令的权限"
            },
            [ReactionType.PermissionDeniedFromPrivateMsg] = new() { "[p]你没有执行这个命令的权限" },
        };

    public ServerSetting Server { get; init; } = new();

    public NetworkSetting Network { get; init; } = new();

    public AutoRunSetting AutoRun { get; init; } = new();

    public PagesSetting Pages { get; set; } = new();

    public ApplicationSetting Application { get; set; } = new();

    public Dictionary<ReactionType, ObservableCollection<string>> Reactions { get; init; } =
        DefaultReactions;
}
