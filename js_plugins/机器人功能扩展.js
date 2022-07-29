serein.registerPlugin("机器人功能拓展","v1.0","Zaitonn","提供自动同意好友、自动入群、戳一戳回复等功能");

serein.setListener("onReceivePacket",onReceivePacket);
function onReceivePacket(json){
    var jobject = JSON.parse(json);
    if(jobject.post_type == "request"){
        if(jobject.request_type == "friend") // 自动同意加好友请求
        {
            serein.sendPacket("{\"action\": \"set_friend_add_request\",\"params\": {\"flag\": \""+jobject.flag+"\",\"approve\": true}}")
        }
        if(jobject.request_type == "group") // 自动同意入群邀请
        {
            serein.sendPacket("{\"action\": \"set_group_add_request\",\"params\": {\"flag\": \""+jobject.flag+"\",\"approve\": true,\"sub_type\":"+jobject.sub_type+"}}")
        }
    }
}

serein.setListener("onGroupPoke",onGroupPoke);
function onGroupPoke(group,user){ // 戳一戳反馈
    serein.sendGroup(group,"[CQ:poke,qq="+user+"]"); // 回戳
    strs=[  // 你可以在此处自定义回复内容
        "嗯哼哼啊啊啊啊啊啊",
        "哇酷哇酷",
        "别戳啦>_<",
    ]; 
    serein.sendGroup(group,strs[Math.floor(Math.random() * strs.length)]);  // 随机回复一句话
}

serein.setListener("onReceiveGroupMessage",onReceiveGroupMessage)
function onReceiveGroupMessage(group_id,user_id,msg,shownname){
    if(msg == "一言"){
        serein.sendGroup(group_id,JSON.parse(serein.createRequest("https://v1.hitokoto.cn")).hitokoto); 
    }
}
