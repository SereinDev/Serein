var config = {
    autoApproveFriendAddRequest: true, // 自动同意加好友请求
    autoApproveGroupAddRequest: true, // 自动同意入群邀请
    randomReplyWhenPoked: true, // 戳一戳随机回复
    oneSentence: true, // 一言
    todayLuck: true, // 今日运势
    websitePreview: true, // 网站预览
    acgImg: true, // 二次元图片
    mcUUID: true, // Minecraft正版用户UUID获取
    bilibili: true, // B站视频解析
    wiki:true // MC维基百科快捷搜索
}

serein.registerPlugin("机器人功能拓展", "v1.1", "Zaitonn", "拓展功能");
var logger = new Logger("机器人功能拓展");
var StreamReader = importNamespace("System.IO").StreamReader;
var WebRequest = importNamespace("System.Net").WebRequest;

function getContent(url) {
    var streamreader = new StreamReader(WebRequest.Create(url).GetResponse().GetResponseStream());
    var response = streamreader.ReadToEnd()
    streamreader.Dispose();
    return response;
}

serein.setListener("onReceivePacket", onReceivePacket);
function onReceivePacket(msg) {
    var json = JSON.parse(msg);
    if (json.post_type == "request") {
        switch (json.request_type) {
            case "friend":
                if (config.autoApproveFriendAddRequest)
                    serein.sendPacket("{\"action\": \"set_friend_add_request\",\"params\": {\"flag\": \"" + jobject.flag + "\",\"approve\": true}}")
                break;
            case "group":
                if (config.autoApproveGroupAddRequest)
                    serein.sendPacket("{\"action\": \"set_group_add_request\",\"params\": {\"flag\": \"" + jobject.flag + "\",\"approve\": true,\"sub_type\":" + jobject.sub_type + "}}")
                break;
        }
    }
}

var strs = [  // 你可以在此处自定义回复内容
    "哇酷哇酷",
    "别戳啦~",
    "[CQ:image,file=https://voidtech.cn/i/2022/11/13/fkk4iv.gif]",
    "[CQ:image,file=https://voidtech.cn/i/2022/11/13/frouu3.jpg]",
    "[CQ:image,file=https://voidtech.cn/i/2022/11/13/froupv.jpg]",
    "[CQ:image,file=https://voidtech.cn/i/2022/11/13/frp6h7.gif]",
    "[CQ:image,file=https://voidtech.cn/i/2022/11/13/fsuhrv.jpg]",
    "不！许！戳！我！",
];
serein.setListener("onGroupPoke", onGroupPoke);
function onGroupPoke(group, user) {
    if (config.randomReplyWhenPoked) {
        serein.sendGroup(group, "[CQ:poke,qq=" + user + "]"); // 回戳
        serein.sendGroup(group, strs[Math.floor(Math.random() * strs.length)]);  // 随机回复一句话
    }
}

var timeDictionary = new Array();
var luckDictionary = new Array();
serein.setListener("onReceiveGroupMessage", onReceiveGroupMessage)
function onReceiveGroupMessage(group_id, user_id, msg, _shownname) {
    switch (msg.split(' ')[0]) {
        case "一言":
            if (config.oneSentence) {
                if (!timeDictionary["oneSentence"] || Date.now() - timeDictionary["oneSentence"] > 2000) {
                    serein.sendGroup(group_id, JSON.parse(getContent("https://v1.hitokoto.cn")).hitokoto);
                    timeDictionary["oneSentence"] = Date.now();
                } else
                    serein.sendGroup(group_id, "技能冷却中！！");
            }
            break;
        case "今日运势":
            if (config.todayLuck) {
                if (!luckDictionary[user_id])
                    luckDictionary[user_id] = Math.floor(Math.random() * 100);
                serein.sendGroup(group_id, "[CQ:at,qq=" + user_id + "]今日运势：" + luckDictionary[user_id]);
            }
            break;
        case "二次元图片":
        case "acg":
        case "ACG":
            if (config.acgImg) {
                if (!timeDictionary["acgImg"] || Date.now() - timeDictionary["acgImg"] > 5000) {
                    timeDictionary["acgImg"] = Date.now();
                    serein.sendGroup(group_id, "图片正在路上！！");
                    serein.sendGroup(group_id, "[CQ:image,cache=0,file=https://tenapi.cn/acg]");
                    timeDictionary["acgImg"] = Date.now();
                } else
                    serein.sendGroup(group_id, "技能冷却中！！");
            }
            break;
        case "查询":
            if (config.mcUUID) {
                if (msg.split(' ').length != 2)
                    serein.sendGroup(group_id, "[CQ:at,qq=" + user_id + "]格式错误\n使用方法：查询+ID\n例：查询 Zi_Min");
                else {
                    var id = msg.split(' ')[1];
                    var json = JSON.parse(getContent("https://tenapi.cn/mc/?uid=" + id));
                    logger.info(getContent("https://tenapi.cn/mc/?uid=" + id));
                    if (json.code == 202)
                        serein.sendGroup(group_id, "[CQ:at,qq=" + user_id + "]" + json.msg);
                    else if (json.code == 200)
                        serein.sendGroup(group_id, "[CQ:at,qq=" + user_id + "] 正版用户查询结果\nID：" + json.name + "\nUUID：" + json.id);
                }
            }
            break;
        case "wiki":
        case "/wiki":
            if(config.wiki){
                if (msg.split(' ').length != 2)
                    serein.sendGroup(group_id, "[CQ:at,qq=" + user_id + "]格式错误\n使用方法：wiki+搜索的关键词\n例：wiki 基岩版");
                serein.sendGroup(group_id,"[CQ:at,qq=" + user_id + "] \nhttps://minecraft.fandom.com/zh/wiki/Special:%E6%90%9C%E7%B4%A2?query="+encodeURI(msg.split(' ')[1])+"&scope=internal&navigationSearch=true");
            }
            break;
        default:
            if (config.websitePreview && /^[A-Za-z0-9]([\x21-\x7e]\.[\x21-\x7e]?)+$/i.test(msg)) {
                try {
                    logger.info("网站查询：" + msg);
                    var json = JSON.parse(getContent("https://tenapi.cn/title/?url=" + msg));
                    if (json.code != 200)
                        json = JSON.parse(getContent("https://tenapi.cn/title/?url=http://" + msg));
                    if (json.code != 200)
                        json = JSON.parse(getContent("https://tenapi.cn/title/?url=https://" + msg));
                    if (json.data)
                        serein.sendGroup(group_id, msg + "\n网页标题：" + json.data.title + "\n介绍：" + json.data.description + "\n关键词：" + json.data.keywords);
                } catch (e) {
                    logger.error(e);
                }
            }
            else if (config.bilibili && /(av\d+|bv[a-zA_Z0-9]+)/i.test(msg)) {
                try {
                    var addr = /(av\d+|bv[a-zA_Z0-9]+)/i.exec(msg)[1];
                    var json = JSON.parse(getContent("https://tenapi.cn/bv/?id=" + addr));
                    if (json.code == 200)
                        serein.sendGroup(group_id, "[CQ:image,cache=0,file=" + json.data.cover + "]\n" + json.data.id + "\nhttps://www.bilibili.com/video/" + json.data.id + "\n" + json.data.title + "\nUP主：" + json.data.name + "\n简介：" + json.data.description + "\n\n播放量：" + json.data.view + "\n点赞：" + json.data.like + "\n投币：" + json.data.coin + "\n收藏：" + json.data.collect);
                    else
                        serein.sendGroup(group_id, "[CQ:at,qq=" + user_id + "]查询失败");
                }
                catch (e) {
                    logger.error(e);
                    serein.sendGroup(group_id, "[CQ:at,qq=" + user_id + "]查询失败");
                }
            }
            break;
    }
}