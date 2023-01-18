serein.registerPlugin("绑定插件", "v1.0", "slemons", "绑定");
serein.setListener("onReceiveGroupMessage", onReceiveGroupMessage);

function onReceiveGroupMessage(group_id, user_id, msg, nickName) {
    bind_id(group_id, user_id, msg, nickName);
    leunbind_id(group_id, user_id, msg);
}


//鉴权
var settings = serein.getSettings();
let opqqlist = JSON.parse(settings).Bot.PermissionList;
let opqq = opqqlist;
serein.log("管理员" + opqq + "可执行解绑");



bind_id = function (group_id, user_id, msg, nickName) {
    let pattern = /^绑定\s(.{3,20})$/g
    if (pattern.test(msg)) {
        let be_bind_id = msg.replace(pattern, "$1");
        let name_pattern = /^(.*)-(.{3,20})$/g;
        let success = serein.bindMember(user_id, be_bind_id, "sbd", "sbd2");
        if (success) {
            serein.sendCmd("whitelist add \"" + be_bind_id + "\"");
            if (/-/g.test(nickName)) { //如果已经有-了
                if (nickName.replace(name_pattern, "$2") != be_bind_id) {
                    editCard(group_id, user_id, nickName.replace(name_pattern, "$1") + "-" + be_bind_id);
                    serein.sendGroup(group_id, "绑定成功\n已将 [CQ:at,qq=" + user_id + "] 的群昵称修改为 " + nickName.replace(name_pattern, "$1") + "-" + be_bind_id + "\nԾ ̮ Ծ")
                } else {
                    // 群名称正确
                    serein.sendGroup(group_id, "[CQ:at,qq=" + user_id + "]绑定成功\nԾ ̮ Ծ")
                }
            } else { //如果没有-
                if (nickName.replace(name_pattern, "$2") != be_bind_id) {
                    editCard(group_id, user_id, nickName + "-" + be_bind_id);
                    serein.sendGroup(group_id, "绑定成功\n已将 [CQ:at,qq=" + user_id + "] 的群昵称修改为 " + nickName + "-" + be_bind_id + "\nԾ ̮ Ծ")
                } else {
                    // 群名称正确
                    serein.sendGroup(group_id, "[CQ:at,qq=" + user_id + "] 绑定成功\nԾ ̮ Ծ")
                }
            }
        } else {
            serein.sendGroup(group_id, "[CQ:at,qq=" + user_id + "] 已绑定" + serein.getGameID(user_id) + " \n请不要重复绑定，如果绑定错误，请发送解绑\nԾ‸ Ծ")
        }
        serein.log(nickName + "(" + user_id + ")" + "绑定" + (success ? "成功" : "失败,已绑定" + serein.getGameID(user_id)))
    }
}


leunbind_id = function (group_id, user_id, msg) {
    let unbind_pattern2 = /解绑\s.*?qq=(.*?)\].*?$/g;
    let unbind_qq = msg.replace(unbind_pattern2, "$1");
    if (unbind_pattern2.test(msg)) {
        if (opqq.indexOf(user_id) != -1) {
            let unbind_id = serein.getGameID(unbind_qq);
            // if (PermissionList.indexOf(user_id) != -1) {
            if (serein.unbindMember(unbind_qq)) {
                serein.sendCmd("whitelist remove \"" + unbind_id + "\"");
                serein.sendGroup(group_id, unbind_id + "白名单已移出\n[CQ:at,qq=" + unbind_qq + "] 管理员给你解绑账号啦\nԾ ̮ Ծ");
            } else {
                serein.sendGroup(group_id, "[CQ:at,qq=" + unbind_qq + "] 未绑定账号呀\nԾ‸ Ծ")
            }
            serein.log("管理员" + user_id + "解绑了" + unbind_qq);
        } else {
            serein.sendGroup(group_id, "[CQ:at,qq=" + user_id + "]你好像没有这个权限哦\nԾ‸ Ծ")
        }
    }
}



function editCard(group_id, user_id, card) {
    serein.log("group_id" + group_id + "\n user_id" + user_id)
    serein.sendPacket(JSON.stringify({
        "action": "set_group_card",
        "params": {
            "group_id": group_id,
            "user_id": user_id,
            "card": card
        }
    }));
}




var Encoding = importNamespace("System.Text").Encoding;
var UTF8Encoding = importNamespace("System.Text").UTF8Encoding;
var SearchOption = importNamespace("System.IO").SearchOption;
var File = importNamespace("System.IO").File;



var shijian = File.ReadAllText(
    "settings\\Event.json", // 路径
    Encoding.UTF8 // 编码（可省略，默认为Encoding.UTF8）
);
let unbang = JSON.parse(shijian)


if (unbang.GroupDecrease.length == 2) {
    unbang.GroupDecrease.splice(0, 2, "g|玩家%ID%退出群聊 已删除白名单，已自动解绑游戏ID\nԾ‸ Ծ", "s|whitelist remove \"%GameID:%ID%%\"", "unbind|%ID%");
    let shijian = JSON.stringify(unbang, null, 2)
    File.WriteAllText(
        "settings\\Event.json", // 路径
        shijian,
        Encoding.UTF8
    )
    serein.log("事件修改完毕")
}
//
var peiZhi = File.ReadAllText(
    "data\\SlemonsBind.json", // 路径
    Encoding.UTF8 // 编码（可省略，默认为Encoding.UTF8）
);
var PeiZhi = JSON.parse(peiZhi)

var zhunTai = 0



serein.log("柠檬的ID白名单基础插件插件，加载完毕！感谢@cofeng@NaN等人的帮助");