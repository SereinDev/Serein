serein.registerPlugin("示例插件","v1.0","Zaitonn","这是一个示例插件"); // 注册插件，注册后会在插件管理页面显示

serein.registerCommand("example",example);
function example(cmd){
    serein.log("你输入了注册的命令："+cmd);
}

serein.setListener("onServerOutput",onServerOutput)
function onServerOutput(line){
    serein.log("服务器控制台输出："+line);
}

serein.setListener("onSereinStart",onSereinStart);
function onSereinStart(){
    serein.log("Serein启动");
}

serein.setListener("onSereinClose",onSereinClose);
function onSereinClose(){
    serein.log("Serein关闭");
}

serein.setListener("onServerStart",onServerStart);
function onServerStart(){
    serein.log("服务器启动");
}

serein.setListener("onServerStop",onServerStop);
function onServerStop(){
    serein.log("服务器关闭");
}

serein.setListener("onServerSendCommand",onServerSendCommand);
function onServerSendCommand(command){
    serein.log("你输入了指令："+command);
}

serein.setListener("onGroupIncrease",onGroupIncrease);
function onGroupIncrease(group,user){
    serein.log("监听群群成员增加 触发群："+group+"  QQ："+user);
}

serein.setListener("onGroupDecrease",onGroupDecrease);
function onGroupDecrease(group,user){
    serein.log("监听群群成员减少 触发群："+group+"  QQ："+user);
}

serein.setListener("onGroupPoke",onGroupPoke);
function onGroupPoke(group,user){
    serein.log("监听群群成员戳一戳当前账号 触发群："+group+"  QQ："+user);
}

serein.setListener("onReceiveGroupMessage",onReceiveGroupMessage);
function onReceiveGroupMessage(group,user,msg,shownname){
    serein.log("监听群聊消息 "+group+"  "+shownname+"（"+user+"）："+msg);
}

serein.setListener("onReceivePrivateMessage",onReceivePrivateMessage);
function onReceivePrivateMessage(user,msg,nickname){
    serein.log("监听私聊消息 "+nickname+"（"+user+"）："+msg);
}

serein.setListener("onReceivePacket",onReceivePacket);
function onReceivePacket(json){
    serein.log("收到数据包："+json)
}



serein.sendPacket("{\"action\": \"send_private_msg\",\"params\": {\"user_id\": \"10001\",\"message\": \"111\"}}") // 发送数据包
