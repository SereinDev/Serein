
### JS标准

[ECMAScript 5.1(ES5)](http://www.ecma-international.org/ecma-262/5.1/)

>[!ATTENTION]
>以下情况将导致Serein无响应
>
>- 在插件中写死循环
>
>   ```js
>   while(true){
>     serein.log("?");  
>   }
>   ```
>
> - 以极快的速度重复执行语句

### 直接使用NET对象/类

由于JS引擎的特性，你可以导入NET几乎所有的命名空间以及其对象、类和属性

`importNamespace(name:String)` 导入命名空间

>[!TIP] 配合一定C#基础食用更佳  
>C#语法详见<https://learn.microsoft.com/zh-cn/dotnet/api/?view=net-6.0>

#### 示例

```js
// https://learn.microsoft.com/zh-cn/dotnet/api/system.io.file?view=net-6.0
var File = importNamespace("System.IO").File;
File.WriteAllText(
    "1.txt", // 路径
    "一些文本"// 文本
);
// 输出到文件
```

```js
// https://learn.microsoft.com/zh-cn/dotnet/api/system.diagnostics.process?view=net-6.0
var Process = importNamespace("System.Diagnostics").Process;
Process.Start("cmd.exe");
// 启动cmd.exe
```

### 内置属性

#### Serein.exe所在文件夹

`serein.path`

```js
var path = serein.path; // Serein.exe所在文件夹，如C:\Serein
```

- 返回
  - `String`

#### Serein版本

`serein.version`

```js
var version = serein.version; // Serein版本，如v1.3.0
```

- 返回
  - `String`

### 内置函数

#### 输出日志

`serein.log(content:Object)`

```js
serein.log("这是一条日志");
serein.log(12345); // 你也可以输出数字
serein.log(new System.IO.StreamWriter('log.txt')); // 甚至可以输出对象
```

- 参数
  - `content` 输出内容
    - 支持`Number` `String`等类型
- 返回
  - 空

#### Debug输出

`serein.debugLog(content:Object)`

```js
serein.debugLog("这是一条Debug输出");
```

- 参数
  - `content` 输出内容
    - 支持`Number` `String`等类型
- 返回
  - 空

#### 注册插件

`serein.registerPlugin(name:String,version:String,author:String,description:String)`

```js
serein.registerPlugin("示例插件","v1.0","Zaitonn","这是一个示例插件"); 
```

- 参数
  - `name` 插件名称
  - `version` 版本
  - `author` 作者或版权信息
  - `description` 介绍
- 返回
  - `Boolean`
    - 成功为`true`，否则为`false`

#### 设置监听器

`serein.setListener(event:String,func:Function)`

```js
serein.setListener("onSereinStart",onSereinStart);
function onSereinStart(){
    serein.log("Serein启动");
}
```

```js
serein.setListener("onGroupPoke",onGroupPoke);
function onGroupPoke(group,user){
    serein.log("监听群群成员戳一戳当前账号 触发群："+group+"  QQ："+user);
}
```

- 参数
  - `event` 事件名称，具体值见下表（区分大小写）
  - `func` 函数
    - 不要包含`()`和参数
- 返回
  - `Boolean`
    - 成功为`true`，否则为`false`

| 事件名                  | 描述                 | 函数原型                                                       |
| ----------------------- | -------------------- | -------------------------------------------------------------- |
| onServerStart           | 服务器启动           | `( )`                                                          |
| onServerStop            | 服务器关闭           | `( )`                                                          |
| onServerOutput          | 服务器输出           | `(line:String)`                                                |
| onServerOriginalOutput  | 服务器原始输出       | `(line:String)`                                                |
| onServerSendCommand     | 服务器输入指令       | `(cmd:String)`                                                 |
| onGroupIncrease         | 监听群群成员增加     | `(group_id:Number,user_id:Number)`                             |
| onGroupDecrease         | 监听群群成员减少     | `(group_id:Number,user_id:Number)`                             |
| onGroupPoke             | 监听群戳一戳自身账号 | `(group_id:Number,user_id:Number)`                             |
| onReceiveGroupMessage   | 收到群消息           | `(group_id:Number,user_id:Number,msg:String,shownName:String)` |
| onReceivePrivateMessage | 收到私聊消息         | `(user_id:Number,msg:String,nickName:String)`                  |
| onReceivePacket         | 收到数据包           | `(packet:String)`                                              |
| onSereinStart           | Serein启动           | `( )`                                                          |
| onSereinClose           | Serein关闭           | `( )`                                                          |
| onPluginsReload         | 插件重载             | `( )`                                                          |

#### 注册服务器命令

`serein.registerCommand(command:String,func:Function)`

```js
serein.registerCommand("example",example);
function example(cmd){
    serein.log("你输入了注册的命令："+cmd);
}
```

>本质上是拦截命令输入

- 参数
  - `command` 命令名称
  - `func` 命令处理函数
    - 不要包含`()`和参数
    - 函数原型：`(cmd:String)`
      - `cmd` 输入的命令全文
    - 例：
- 返回
  - 空

#### 获取Serein设置

`serein.getSettings()`

```js
var settings = serein.getSettings();
```

- 参数
  - 空
- 返回
  - `String` 设置的json文本

<details>
<summary>返回示例（已格式化）</summary>
<pre><code>
{
  "Server": {
    "Path": "",
    "EnableRestart": false,
    "EnableOutputCommand": true,
    "EnableLog": false,
    "OutputStyle": 1,
    "StopCommand": "stop",
    "AutoStop": true,
    "EncodingIndex": 0,
    "EnableUnicode": false,
    "Type": 1,
    "Port": 19132
  },
  "Matches": {
    "Version": "(\\d+\\.\\d+\\.\\d+\\.\\d+)",
    "Difficulty": "(PEACEFUL|EASY|NORMAL|HARD|DIFFICULT[^Y])",
    "LevelName": "Level Name: (.+?)$",
    "Finished": "(Done|Started)",
    "PlayerList": "players\\sonline:"
  },
  "Bot": {
    "EnableLog": false,
    "GivePermissionToAllAdmin": false,
    "EnbaleOutputData": false,
    "GroupList": [],
    "PermissionList": [],
    "Uri": "127.0.0.1:6700",
    "Authorization": "",
    "Restart": false,
    "AutoEscape": false
  },
  "Serein": {
    "EnableGetUpdate": true,
    "EnableGetAnnouncement": true,
    "Debug": true,
    "DPIAware": true
  },
  "Event": {
    "Notice": "在这里你可以自定义每个事件触发时执行的命令。参考：https://serein.cc/Command.html、https://serein.cc/Event.html",
    "Bind_Success": [
      "g|[CQ:at,qq=%ID%] 绑定成功"
    ],
    "Bind_Occupied": [
      "g|[CQ:at,qq=%ID%] 该游戏名称被占用"
    ],
    "Bind_Invalid": [
      "g|[CQ:at,qq=%ID%] 该游戏名称无效"
    ],
    "Bind_Already": [
      "g|[CQ:at,qq=%ID%] 你已经绑定过了"
    ],
    "Unbind_Success": [
      "g|[CQ:at,qq=%ID%] 解绑成功"
    ],
    "Unbind_Failure": [
      "g|[CQ:at,qq=%ID%] 该账号未绑定"
    ],
    "Server_Start": [
      "g|服务器正在启动"
    ],
    "Server_Stop": [
      "g|服务器已关闭"
    ],
    "Server_Error": [
      "g|服务器异常关闭"
    ],
    "Group_Increase": [
      "g|欢迎[CQ:at,qq=%ID%]入群~"
    ],
    "Group_Decrease": [
      "g|用户%ID%退出了群聊，已自动解绑游戏ID",
      "unbind|%ID%"
    ],
    "Group_Poke": [
      "g|别戳我……(*/ω＼*)"
    ],
    "Serein_Crash": [
      "g|唔……发生了一点小问题(っ °Д °;)っ\n请查看Serein错误弹窗获取更多信息"
    ],
    "Motdpe_Success": [
      "g|服务器描述：%Description%\n版本：%Version%(%Protocol%)\n在线玩家：%OnlinePlayer%/%MaxPlayer%\n游戏模式：%GameMode%\n延迟：%Delay%ms"
    ],
    "Motdje_Success": [
      "g|服务器描述：%Description%\n版本：%Version%(%Protocol%)\n在线玩家：%OnlinePlayer%/%MaxPlayer%\n延迟：%Delay%ms\n%Favicon%"
    ],
    "Motd_Failure": [
      "g|Motd获取失败\n详细原因：%Exception%"
    ],
    "PermissionDenied_Private": [
      "p|你没有执行这个命令的权限"
    ],
    "PermissionDenied_Group": [
      "g|[CQ:at,qq=%ID%] 你没有执行这个命令的权限"
    ]
  }
}
</code></pre>
</details>

#### 获取系统信息

`serein.getSysInfo(type:String)`

```js
var cpuname = serein.getSysInfo("CPUName");
```

- 参数
  - `type` 信息类型，可为以下值之一
    - `NET` 当前NET版本
    - `OS` 操作系统名称
    - `CPUName` CPU名称
    - `TotalRAM` 总内存（MB）
    - `UsedRAM` 已用内存（MB)
    - `RAMPercentage` 内存占用率
    - `CPUPercentage` CPU占用率
    >[!WARNING]
    >  
    >- 不区分大小写  
    >- 若不在以上列表中则为返回空值

- 返回
  - `String` 对应的值

#### 执行命令

`serein.runCommand(cmd:String)`

```js
serein.runCommand("g|hello")
```

- 参数
  - `cmd` 一条[Serein命令](Command.md)
  >[!WARNING] 此处无法执行绑定或解绑ID、获取motd和执行js代码的命令
- 返回
  - 空

#### 获取Motd原文

基岩版：`serein.getMotdpe(ip:String)`  
Java版：`serein.getMotdje(ip:String)`

```js
var pe = serein.getMotdpe("127.0.0.1:19132");
var je = serein.getMotdje("127.0.0.1:25565");
```

- 参数
  - `ip` 服务器IP
  >[!WARNING] 可含端口，如`example.com:11451`  
  >不填端口基岩版默认`19132`，Java版默认`25565`
- 返回
  - `String` Motd原文
    - 获取失败时返回`-`
    - 基岩版为纯字符串

    ```txt
    MCPE;Dedicated Server;503;1.18.33;0;10;12578007761032183218;Bedrock level;Survival;1;19132;19133;
    ```

    - Java版为Json文本

    ```txt
    {"description":{"text":"§bMinecraftOnline§f - §6Home of Freedonia§r\n§3Survival, Without the Grief!"},"players":{"max":120,"online":1,"sample":[{"id":"a4740a2c-1eec-4b7d-9d22-1c861e7045d7","name":"Biolord101"}]},"version":{"name":"1.12.2","protocol":340},"favicon":"……"}
    ```

#### 启动服务器

`serein.startServer()`

```js
var success = serein.startServer();
```

- 参数
  - 空
- 返回
  - `Boolean`
    - 成功为`true`，否则为`false`

#### 关闭服务器

`serein.stopServer()`

```js
serein.stopServer();
```

- 参数
  - 空
- 返回
  - 空

>[!WARNING] 此函数不能保证服务器被关闭

#### 强制结束服务器

`serein.killServer()`

```js
var success = serein.killServer();
```

- 参数
  - 空
- 返回
  - `Boolean`
    - 成功为`true`，否则为`false`

#### 发送服务器命令

`serein.sendCmd(String:command)`

```js
serein.sendCmd("help");
```

- 参数
  - `command` 输入的命令
- 返回
  - 空

#### 获取服务器状态

`serein.getServerStatus()`

```js
var serverStatus = serein.getServerStatus();
```

- 参数
  - 空
- 返回
  - `Boolean`
    - 已启动为`true`，未启动则为`false`

#### 获取服务器运行时长

`serein.getServerTime()`

```js
var time = serein.getServerTime();
```

- 参数
  - 空
- 返回
  - `String`
    - 示例：`0.2m` `1.5h` `3.02d`

#### 获取服务器进程占用

`serein.getServerCPUPersent()`

```js
var cpupersent = serein.getServerCPUPersent();
```

- 参数
  - 空
- 返回
  - `String`
    - 示例：`1.14` `5.14`

#### 获取服务器文件

`serein.getServerFile()`

```js
var file = serein.getServerFile();
```

- 参数
  - 空
- 返回
  - `String`
    - 示例：`bedrock_server.exe`

#### 发送群聊消息

`serein.sendGroup(target:Number,msg:String)`

```js
var success = serein.sendGroup(114514,"大家好");
```

- 参数
  - `target` 群号
  - `msg` 消息内容
- 返回
  - `Boolean`
    - 成功为`true`，否则为`false`
    >[!WARNING] 此值仅代表此消息是否成功发送至机器人，并不代表消息能够成功发出

#### 发送私聊消息

`serein.sendPrivate(target:Number,msg:String)`

```js
var success = serein.sendPrivate(114514,"你好");
```

- 参数
  - `target` 对方QQ号
  - `msg` 消息内容
- 返回
  - `Boolean`
    - 成功为`true`，否则为`false`
    >[!WARNING] 此值仅代表此消息是否成功发送至机器人，并不代表消息能够成功发出

#### 发送数据包

`serein.sendPacket(json:String)`

```js
serein.sendPackage("{\"action\": \"send_private_msg\",\"params\": {\"user_id\": \"10001\",\"message\": \"你好\"}}")
// 你可以通过这个功能实现自动同意好友请求等操作
```

- 参数
  - `json` 发送的json数据
- 返回
  - `Boolean`
    - 成功为`true`，否则为`false`
    >[!WARNING] 此值仅代表此消息是否成功发送至机器人，并不代表消息能够成功发出

#### 获取ws连接状态

`serein.getWsStatus()`

```js
var connected = serein.getWsStatus();
```

- 参数
  - 无
- 返回
  - `Boolean`
    - 已连接为`true`，否则为`false`

#### 绑定游戏ID

`serein.bindMember(userId:Number,gameId:String)`

```js
var success = serein.bindMember(114514,"Li_Tiansuo");
```

- 参数
  - `userId` QQ号
- 返回
  - `Boolean`
    - 成功为`true`，否则为`false`

#### 删除绑定记录

`serein.unbindMember(userId:Number)`

```js
var success = serein.unbindMember(114514);
```

- 参数
  - `userId` QQ号
- 返回
  - `Boolean`
    - 成功为`true`，否则为`false`

#### 获取指定用户QQ

`serein.getID(gameId:String)`

```js
var qq = serein.getID("Li_Tiansuo");
```

- 参数
  - `gameId` 游戏ID
- 返回
  - `Number` QQ号

#### 获取指定游戏ID

`serein.getGameID(userId:Number)`

```js
var id = serein.getGameID(114514);
```

- 参数
  - `userId` QQ号
- 返回
  - `String` 游戏ID

### 内置类

#### WebSocket客户端

```js
// 由于该js解释器不支持ws，所以这里用C#封装了一个，部分函数和js原生的有所不同
var ws = new WebSocket("ws://127.0.0.1:11451"); // 实例化ws

ws.onopen = function(){
  // ws开启事件
  // ...
};
ws.onclose = function(){
  // ws关闭事件
  // ...
};
ws.onerror = function(message){ // 错误信息
  // ws发生错误事件
  // ...
};
ws.onmessage = function(message){ // 收到数据
  // ws收到数据事件
  // ...
};

ws.open(); // 连接ws
var state = ws.state; // 连接状态
/*
 * 此状态有以下五个可能的枚举值
 *
 *  -1  未知或无效
 *  0   正在连接
 *  1   连接成功
 *  2   正在关闭
 *  3   已关闭
*/
ws.send("hello"); // 发送数据
ws.close(); // 关闭ws
ws.dispose(); // 释放对象
```

#### Logger

```js
var logger = Logger("Example"); // 插件名称
logger.info("这是一条信息输出");
logger.warn("这是一条警告输出");
logger.error("这是一条错误输出");
logger.debug("这是一条信息输出"); // 此消息将输出到Serein的debug窗口而不是插件控制台
```

![logger](../imgs/logger.png)