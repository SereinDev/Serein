## Javascript
- [Javascript](#javascript)
  - [标准](#标准)
  - [可直接使用的NET对象/类](#可直接使用的net对象类)
    - [示例](#示例)
  - [内置属性](#内置属性)
    - [Serein.exe所在文件夹](#sereinexe所在文件夹)
    - [Serein版本](#serein版本)
  - [内置函数](#内置函数)
    - [输出日志](#输出日志)
    - [Debug输出](#debug输出)
    - [注册插件](#注册插件)
    - [设置监听器](#设置监听器)
    - [获取系统信息](#获取系统信息)
    - [执行命令](#执行命令)
    - [获取Motd原文](#获取motd原文)
    - [启动服务器](#启动服务器)
    - [关闭服务器](#关闭服务器)
    - [强制结束服务器](#强制结束服务器)
    - [获取服务器状态](#获取服务器状态)
    - [获取服务器运行时长](#获取服务器运行时长)
    - [获取服务器进程占用](#获取服务器进程占用)
    - [发送群聊消息](#发送群聊消息)
    - [发送私聊消息](#发送私聊消息)
    - [获取ws连接状态](#获取ws连接状态)
    - [绑定游戏ID](#绑定游戏id)
    - [删除绑定记录](#删除绑定记录)
    - [获取指定用户QQ](#获取指定用户qq)
    - [获取指定游戏ID](#获取指定游戏id)

### 标准
[ECMAScript 5.1(ES5)](http://www.ecma-international.org/ecma-262/5.1/)

### 可直接使用的NET对象/类
- [`System.IO.File`](https://docs.microsoft.com/zh-cn/dotnet/api/system.io.file) 提供用于创建、复制、删除、移动和打开单一文件的静态方法，并协助创建 FileStream 对象。
- [`System.IO.Directory`](https://docs.microsoft.com/zh-cn/dotnet/api/system.io.directory) 公开用于通过目录和子目录进行创建、移动和枚举的静态方法。
- [`System.IO.DirectoryInfo`]() 公开用于创建、移动和枚举目录和子目录的实例方法。 
- [`System.IO.Path`](https://docs.microsoft.com/zh-cn/dotnet/api/system.io.path) 对包含文件或目录路径信息的 String 实例执行操作。 这些操作是以跨平台的方式执行的。
- [`System.IO.StreamReader`](https://docs.microsoft.com/zh-cn/dotnet/api/system.io.streamreader) 实现一个 TextReader，使其以一种特定的编码从字节流中读取字符。
- [`System.IO.StreamWriter`](https://docs.microsoft.com/zh-cn/dotnet/api/system.io.streamwriter) 实现一个 TextWriter，使其以一种特定的编码向流中写入字符。
- [`System.Text.Encoding`](https://docs.microsoft.com/zh-cn/dotnet/api/system.text.encoding) 表示字符编码。
- [`System.Diagnostics.Process`](https://docs.microsoft.com/zh-cn/dotnet/api/system.diagnostics.process) 提供对本地和远程进程的访问权限并使你能够启动和停止本地系统进程。
- [`System.Diagnostics.ProcessStartInfo`](https://docs.microsoft.com/zh-cn/dotnet/api/system.diagnostics.processstartinfo) 指定启动进程时使用的一组值。

#### 示例

输出到文件
```js
var file = new System.IO.StreamWriter('log.txt');
file.WriteLine('Hello World !');
file.Dispose();
```

启动`cmd.exe`
```js
System.Diagnostics.Process.Start("cmd.exe");
```

### 内置属性

#### Serein.exe所在文件夹
`serein.path`
- 返回
  - `String` 

#### Serein版本
`serein.version`
- 返回
  - `String`
### 内置函数

#### 输出日志
`serein.log(content:Object)`
- 参数
  - `content` 输出内容
    - 支持`Number` `String`等类型
- 返回
  - 空

#### Debug输出
`serein.debugLog(content:Object)`
- 参数
  - `content` 输出内容
    - 支持`Number` `String`等类型
- 返回
  - 空

#### 注册插件
`serein.registerPlugin(name:String,version:String,author:String,description:String)`

- 参数
  - `name` 插件名称
    - 不允许与其他插件出现重复
  - `version` 版本
  - `author` 作者或版权信息
  - `description` 介绍
- 返回
  - `Boolean`
    - 成功为`true`，否则为`false`

#### 设置监听器
`serein.setListener(event:String,funcname:String)`
- 参数
  - `event` 事件名称，具体值见下表（区分大小写）
  - `funcname` 函数名
    - 不要包含`()`和参数
- 返回
  - `Boolean`
    - 成功为`true`，否则为`false`

| 事件名 | 描述 | 函数原型 |
| --- | --- | --- |
| onServerStart | 服务器启动 | `()` |
| onServerStop | 服务器关闭 | `()` |
| onServerSendCommand | 服务器输入指令 | `(cmd:String)` |
| onGroupIncrease | 监听群群成员增加 | `(group_id:Number,user_id:Number)` |
| onGroupDecrease | 监听群群成员减少 | `(group_id:Number,user_id:Number)` |
| onGroupPoke | 监听群戳一戳自身账号 | `(group_id:Number,user_id:Number)` |
| onReceiveGroupMessage | 收到群消息 | `(group_id:Number,user_id:Number,msg:String,shownName:String)` |
| onReceivePrivateMessage | 收到私聊消息 | `(user_id:Number,msg:String,nickName:String)` |
| onReceivePackage | 收到数据包 | `(parkage:String)` |
| onSereinStart | Serein启动 | `( )` |
| onSereinClose | Serein关闭 | `( )` |

#### 获取系统信息
`serein.getSysInfo(type:String)`
- 参数
  - `type` 信息类型，可为以下值之一
    - `NET` 当前NET版本
    - `OS` 操作系统名称
    - `CPUName` CPU名称
    - `TotalRAM` 总内存（MB）
    - `UsedRAM` 已用内存（MB)
    - `RAMPercentage` 内存占用率
    - `CPUPercentage` CPU占用率
    >#### ⚠ 提示
    >- 不区分大小写  
    >- 若不在以上列表中则为返回空值
- 返回
  - `String` 对应的值

#### 执行命令
`serein.runCommand(cmd:String)`

- 参数
  - `cmd` 一条[Serein命令](Command.md)
  >#### ⚠ 提示
  >此处无法执行绑定或解绑ID、获取motd和执行js代码的命令
- 返回
  - 空

#### 获取Motd原文
基岩版：`serein.getMotdpe(ip:String)`  
Java版：`serein.getMotdje(ip:String)` 

- 参数
  - `ip` 服务器IP
  >#### ⚠ 提示
  >可含端口，如`example.com:11451`  
  >不填端口基岩版默认`19132`，Java版默认`25565`
- 返回
  - `String` Motd原文
    - 获取失败时返回`-`
    - 基岩版为纯字符串
    ```
    MCPE;Dedicated Server;503;1.18.33;0;10;12578007761032183218;Bedrock level;Survival;1;19132;19133;
    ``` 
    - Java版为Json文本
    ```
    {"description":{"text":"§bMinecraftOnline§f - §6Home of Freedonia§r\n§3Survival, Without the Grief!"},"players":{"max":120,"online":1,"sample":[{"id":"a4740a2c-1eec-4b7d-9d22-1c861e7045d7","name":"Biolord101"}]},"version":{"name":"1.12.2","protocol":340},"favicon":"……"}
    ``` 

#### 启动服务器
`serein.startServer()`

- 参数
  - 空
- 返回
  - `Boolean`
    - 成功为`true`，否则为`false`

#### 关闭服务器
`serein.stopServer()`

- 参数
  - 空
- 返回
  - 空

>#### ⚠ 提示
>此函数不能保证服务器被关闭

#### 强制结束服务器
`serein.killServer()`

- 参数
  - 空
- 返回
  - `Boolean`
    - 成功为`true`，否则为`false`

#### 获取服务器状态
`serein.getServerStatus()`

- 参数
  - 空
- 返回
  - `Boolean`
    - 已启动为`true`，未启动则为`false`

#### 获取服务器运行时长
`serein.getServerTime()`

- 参数
  - 空
- 返回
  - `String`
    - 示例：`0.2m` `1.5h` `3.02d`

#### 获取服务器进程占用
`serein.getServerCPUPersent()`

- 参数
  - 空
- 返回
  - `String`
    - 示例：`1.14` `5.14`

#### 发送群聊消息
`serein.sendGroup(target:Number,msg:String)`

- 参数
  - `target` 群号
  - `msg` 消息内容
- 返回
  - `Boolean`
    - 成功为`true`，否则为`false`
    >#### ⚠ 提示
    >此值仅代表此消息是否成功发送至机器人，并不代表消息能够成功发出

#### 发送私聊消息
`serein.sendPrivate(target:Number,msg:String)`

- 参数
  - `target` 对方QQ号
  - `msg` 消息内容
- 返回
  - `Boolean`
    - 成功为`true`，否则为`false`
    >#### ⚠ 提示
    >此值仅代表此消息是否成功发送至机器人，并不代表消息能够成功发出

#### 获取ws连接状态
`serein.getWsStatus()`

- 参数
  - 无
- 返回
  - `Boolean`
    - 已连接为`true`，否则为`false`

#### 绑定游戏ID
`serein.bindMember(userId:Number,gameId:String)`

- 参数
  - `userId` QQ号
- 返回
  - `Boolean`
    - 成功为`true`，否则为`false`

#### 删除绑定记录
`serein.unbindMember(userId:Number)`

- 参数
  - `userId` QQ号
- 返回
  - `Boolean`
    - 成功为`true`，否则为`false`

#### 获取指定用户QQ
`serein.getID(gameId:String)`

- 参数
  - `gameId` 游戏ID
- 返回
  - `Number` QQ号

#### 获取指定游戏ID
`serein.getGameID(userId:Number)`

- 参数
  - `userId` QQ号
- 返回
  - `String` 游戏ID