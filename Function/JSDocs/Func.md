
## 🧊 Serein相关

### 输出日志

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

>[!TIP]个人更推荐使用[Logger](Function/JSDocs/Class.md#logger)输出，可以方便区分输出等级

### Debug输出

`serein.debugLog(content:Object)`

```js
serein.debugLog("这是一条Debug输出");
```

- 参数
  - `content` 输出内容
    - 支持`Number` `String`等类型
- 返回
  - 空

### 注册插件

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
  - `Boolean` *(v1.3.2及以前)*
    - 成功为`true`，否则为`false`
  - `String`
    - 当前的命名空间

### 设置监听器

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

### 获取Serein设置

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

### 获取Serein设置对象

`serein.getSettingsObject()`

- 参数
  - 空
- 返回
  - `object` 设置内容对象（见上）

>[!TIP]推荐使用该函数，而不是上面的`serein.getSettings()`
>
>- 你可以直接通过对象的属性获取对应的设置项，而不用将其转成JSON后再获取
>- `serein.getSettings()`为了向下兼容将会存留两到三个版本，以后可能将被`serein.getSettingsObject()`取代

### 执行命令

`serein.runCommand(cmd:String)`

```js
serein.runCommand("g|hello")
```

- 参数
  - `cmd` 一条[Serein命令](Command.md)
  >[!WARNING] 此处无法执行绑定或解绑ID、获取motd和执行js代码的命令
- 返回
  - 空

### 获取插件列表

`serein.getPluginList()`

```js
var list = serein.getPluginList();
```

- 参数
  - 空
- 返回
  - `Array<PluginInfo>` 插件列表

```json
{ 
  "Namespace": "test",  // 命名空间
  "Available": true,    // 是否可用
  "File": "plugins\\test.js", // 相对路径
  "WebSockets": [], // 创建的WS对象状态列表
  "Name": "test",   // 注册的名称
  "Version": "-",   // 注册的版本
  "Author": "-",    // 注册的作者
  "Description": "-", // 注册的介绍
  "EventList": []   // 监听的事件列表
} 
```

>[!TIP]由于此函数为即时获取，获取时可能还未将所有插件载入，故建议使用`setTimeout()`延迟一段时间再获取

## 🌏 系统相关

### 获取系统信息

`serein.getSysInfo()`

```js
var info = serein.getSysInfo();
```

- 参数
  - 空
- 返回
  - `object` 系统信息对象

以json格式显示：

```json
{
  "Architecture": "64 位",                              // 架构（32位、64位、AMD）
  "Name": "Microsoft Windows 10 家庭版 SP0.0",
  "Hardware": {                                         // 硬件信息
    "CPUs": [                                           // CPU列表
      {
        "Name": "Intel Core i5-1035G4 CPU @ 1.10GHz",   // 名称
        "Brand": "GenuineIntel",                        // 供应商/品牌
        "Architecture": "x64",                          // 架构
        "Cores": 4,                                     // 核心数
        "Frequency": 1498                               // 频率（MHz）
      }
    ],
    "GPUs": [                                           // GPU列表
      {
        "Name": "Intel(R) Iris(R) Graphics Family",     // 名称
        "Brand": "Intel(R) Iris(R) Plus Graphics",      // 供应商/品牌
        "Resolution": "2736x1824",                      // 分辨率
        "RefreshRate": 59,                              // 刷新率（Hz）
        "MemoryTotal": 1048576                          // GPU内存 / 显存
      }
    ],
    "RAM": {
      "Free": 2350688,                                  // 可用内存（KB）
      "Total": 7964852                                  // 总内存（KB）
    }
  },
  "FrameworkVersion": {                                 // NET框架版本
    "Major": 4,
    "Minor": 0,
    "Build": 30319,
    "Revision": 42000,
    "MajorRevision": 0,
    "MinorRevision": -23536
  },
  "JavaVersion": {                                      // Java运行库版本（-1为无运行库）
    "Major": 0,
    "Minor": 0,
    "Build": -1,
    "Revision": -1,
    "MajorRevision": -1,
    "MinorRevision": -1
  },
  "IsMono": false,                                      // 当前软件是否使用Mono运行
  "OperatingSystemType": 4                              // 操作系统类型（枚举值）
  // Windows = 0, Linux, MacOSX, BSD, WebAssembly, Solaris, Haiku, Unity5, Other
}
```

### 获取CPU使用率

`serein.getCPUPersent()`

```js
var cpupersent = serein.getServerCPUPersent();
```

- 参数
  - 空
- 返回
  - `Number` ∈ [0,100]
    - 示例：`1.14514191981`
  - `undefined` *Linux版本*

### 获取网速

`serein.getNetSpeed()`

```js
var netSpeed = serein.getServerCPUPersent();
var uploadSpeed = netSpeed[0];
var downloadSpeed = netSpeed[1];
```

- 参数
  - 空
- 返回
  - `Array<String>[2]`，其中[0]为上传网速，[1]为下载网速

## 🚛 服务器相关

### 启动服务器

`serein.startServer()`

```js
var success = serein.startServer();
```

- 参数
  - 空
- 返回
  - `Boolean`
    - 成功为`true`，否则为`false`

### 关闭服务器

`serein.stopServer()`

```js
serein.stopServer();
```

- 参数
  - 空
- 返回
  - 空

>[!WARNING] 此方法不能保证服务器被关闭

### 强制结束服务器

`serein.killServer()`

```js
var success = serein.killServer();
```

- 参数
  - 空
- 返回
  - `Boolean`
    - 成功为`true`，否则为`false`

### 发送服务器命令

`serein.sendCmd(String:command)`

```js
serein.sendCmd("help");
```

- 参数
  - `command` 输入的命令
- 返回
  - 空

### 获取服务器状态

`serein.getServerStatus()`

```js
var serverStatus = serein.getServerStatus();
```

- 参数
  - 空
- 返回
  - `Boolean`
    - 已启动为`true`，未启动则为`false`

### 获取服务器运行时长

`serein.getServerTime()`

```js
var time = serein.getServerTime();
```

- 参数
  - 空
- 返回
  - `String`
    - 示例：`0.2m` `1.5h` `3.02d`

### 获取服务器进程占用

`serein.getServerCPUPersent()`

```js
var cpupersent = serein.getServerCPUPersent();
```

- 参数
  - 空
- 返回
  - `Number` ∈ [0,100]
    - 示例：`1.14514191981`

### 获取服务器文件

`serein.getServerFile()`

```js
var file = serein.getServerFile();
```

- 参数
  - 空
- 返回
  - `String`
    - 示例：`bedrock_server.exe`

### 获取Motd原文

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

    ```json
    {
      "description": {
        "text": "§bMinecraftOnline§f - §6Home of Freedonia§r\n§3Survival, Without the Grief!"
      },
      "players": {
        "max": 120,
        "online": 1,
        "sample": [
            {
                "id": "a4740a2c-1eec-4b7d-9d22-1c861e7045d7",
                "name": "Biolord101"
            }
        ]
      },
      "version": {
        "name": "1.12.2",
        "protocol": 340
      },
      "favicon": "……" // 此处限于篇幅省略其内容，实际上是base64编码的图片
    }
    ```

## 🤖 消息收发

### 发送群聊消息

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
    >[!WARNING] 此值仅代表此消息是否成功由WebSocket发出，并不代表消息能够成功发送至聊天

### 发送私聊消息

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
    >[!WARNING] 此值仅代表此消息是否成功由WebSocket发出，并不代表消息能够成功发送至聊天

### 发送数据包

`serein.sendPacket(packet:String)`

```js
serein.sendPackage("{\"action\": \"send_private_msg\",\"params\": {\"user_id\": \"10001\",\"message\": \"你好\"}}")
// 你可以通过这个功能实现自动同意好友请求等操作
```

- 参数
  - `packet` 发送的数据包
- 返回
  - `Boolean`
    - 成功为`true`，否则为`false`
    >[!WARNING] 此值仅代表此消息是否成功由WebSocket发出，并不代表消息能够成功发送至聊天

### 获取ws连接状态

`serein.getWsStatus()`

```js
var connected = serein.getWsStatus();
```

- 参数
  - 无
- 返回
  - `Boolean`
    - 已连接为`true`，否则为`false`

### 获取群成员昵称缓存字典

`serein.getGroupCache()`

```js
var dict = serein.getGroupCache();
var myname = dict["114514"]["1919810"];
```

- 参数
  - 无
- 返回
  - `Object` 群成员昵称缓存字典
    - 第一个`key`为群号
    - 第二个`key`为QQ号

>[!WARNING] 此处的`key`必须为字符串形式的群号或QQ号，直接使用`Number`类型作为`key`获取将导致Serein引发超出内存的异常

### 直接获取指定群的群成员昵称缓存

`serein.getUserName(groupid: Number, userid: Number)`

```js
var myname = serein.getUserName(114514, 1919810); // 与上面的函数示例等价
```

- 参数
  - `groupid` 群号
  - `userid` QQ号
- 返回
  - `String`
    - 若未找到或不存在则返回空字符串

## 👨🏻‍🤝‍👨🏻 绑定/解绑

### 绑定游戏ID

`serein.bindMember(userid:Number, gameid:String)`

```js
var success = serein.bindMember(114514, "Li_Tiansuo");
```

- 参数
  - `userid` QQ号
  - `gameid` 游戏ID
- 返回
  - `Boolean`
    - 成功为`true`，否则为`false`

### 删除绑定记录

`serein.unbindMember(userid:Number)`

```js
var success = serein.unbindMember(114514);
```

- 参数
  - `userid` QQ号
- 返回
  - `Boolean`
    - 成功为`true`，否则为`false`

### 获取指定用户QQ

`serein.getID(gameid:String)`

```js
var qq = serein.getID("Li_Tiansuo");
```

- 参数
  - `gameid` 游戏ID
- 返回
  - `Number` QQ号

### 获取指定游戏ID

`serein.getGameID(userid:Number)`

```js
var id = serein.getGameID(114514);
```

- 参数
  - `userid` QQ号
- 返回
  - `String` 游戏ID

## 🧬 从模块中加载

v1.3.4 后你可以创建新的js文件，并在里面写一些基本的函数方便日常调用，如判断是否为管理、格式化自定义的时间等

### 导出

>[!WARNING]
>
>- 对在 plugins 下的所有js文件有效
>   - 加载插件时仅会加载 plugins 下的`.js`结尾的文件（不包含子文件夹，且不以`.modules.js`结尾）
>   - 你可以把需要导出的函数直接放在 plugins 下或其子文件夹
>- 目前支持导出的类型：
>   - 值/对象
>   - 函数
>   - 类
>- 必须为导出的成员加上`export`关键字
>

```js
// plugins/eg.js

export const myvalue = 1;

export function isMyGroup(groupID) {
    return Boolean(JSON.parse(serein.getSettings()).Bot.GroupList.indexOf(groupID) + 1);
}

// ...
```

### 导入

`serein.loadFrom(file: String)` / `require(file: String)`

此处的`file`参数对应为你在 plugins 文件夹创建的js文件路径

>[!WARNING]
>
>- 必须使用相对路径（基目录为 plugins）
>- 需以`./`开头
>- 需包含扩展名
>- 导入时会被完整运行一次
>

```js
var isMyGroup = require('./eg.js').isMyGroup(114514);
// var isMyGroup = serein.loadFrom('./eg.js').isMyGroup(114514); 二者等价
```

这样你就可以导入已经导出了的内容了
