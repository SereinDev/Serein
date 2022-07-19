## 命令
- [命令](#命令)
  - [写在前面](#写在前面)
  - [调用cmd.exe执行一条命令](#调用cmdexe执行一条命令)
  - [服务器命令](#服务器命令)
    - [内置服务器指令](#内置服务器指令)
  - [发送私聊消息](#发送私聊消息)
  - [发送群聊消息](#发送群聊消息)
  - [绑定游戏ID](#绑定游戏id)
  - [解绑游戏ID](#解绑游戏id)
  - [获取服务器信息](#获取服务器信息)
  - [执行Javascript代码](#执行javascript代码)
    - [内置函数](#内置函数)
    - [NET对象/类（可直接使用）](#net对象类可直接使用)
    - [示例](#示例)
  - [调试输出](#调试输出)

### 写在前面
- 所有命令格式都为 `<命令名称>[:<参数>]|<执行内容>`  
  - 命令的名称部分不区分大小写   
  - `|`为竖线（分隔线）
    - 👆 U+007C，不是中文的`丨`(gǔn)
  - `<>`表示必选内容   
  - `[]`表示为可选内容 



### 调用cmd.exe执行一条命令   
`cmd|<命令>`   
运行结束后自动结束cmd进程
- 默认窗口编码：`936 ANSI/GBK`  
- 工作目录：`Serein.exe`所在目录  
- 显示窗口：否  

>#### ⭐ Tips 
>- 可搭配使用`7za.exe`(https://www.7-zip.org/download.html)实现压缩功能  
>- 可通过`echo.[文本内容]>>example.txt`实现写入文本
>- 可调用批处理文件执行一系列的工作
  

### 服务器命令
`s|<命令>`   
`server|<命令>`   
在服务器中执行命令  

>#### ⚠ 提示
>- 若服务器未启动则不执行 

`s:u|<命令>`    
`server:u|<命令>`  
`s:unicode|<命令>`     
`server:unicode|<命令>`  
在服务器中执行命令，不同的是将命令中的非ASCII的字符转换为Unicode字符输出，适用于`tellraw`等使用`json`文本的命令

>#### ⚠ 提示
>- 若服务器未启动则不执行 

#### 内置服务器指令
`start`👉启动服务器  
*仅当服务器未启动时能被触发  

### 发送私聊消息
`p|<消息>`  
`private|<消息>`  
发送一条消息给触发此命令的用户



>#### ⚠ 提示
>- 以上两项只能由正则匹配到私聊或群聊消息时触发，发送对象为触发这项正则的用户
>- 若触发对象不是机器人好友或账号不存在可能无法发送
>- 若Websocket未连接则不发送  

`p:<QQ>|<消息>`  
`private:<QQ>|<消息>`  
发送一条消息给指定用户

>#### ⚠ 提示
>- 若触发对象不是机器人好友或账号不存在可能无法发送
>- 若Websocket未连接则不发送


### 发送群聊消息
`g|<消息>`  
`group|<消息>`  
发送一条消息到触发此命令的群聊或默认群聊

>#### ⚠ 提示
>- 当正则匹配的到群聊消息时，发送对象为触发这条消息的群聊
>- 当正则匹配的到私聊消息时，不发送消息
>- 其他情况下则发送至`设置-监听群列表`的第一项，若此项为空不发送
>- 若机器人未入群或被禁言则发送失败
>- 若Websocket未连接则不发送

`g:<QQ>|<消息>`  
`group:<QQ>|<消息>`  
发送一条消息给指定群聊

>#### ⚠ 提示
>- 若机器人未入群或被禁言则发送失败
>- 若Websocket未连接则不发送

### 绑定游戏ID
`b|<ID>`  
`bind|<ID>`  
将所填ID与触发这条消息的账号绑定
>#### ⚠ 提示
>- 此命令只能被群聊消息触发
>- 若该ID不合法或已经绑定会返回消息提示
>- 提示内容可在`settings/Event.json`中自定义，详见[事件](Event.md)  


### 解绑游戏ID
`ub|<QQID>`  
`unbind|<QQID>`  
解除QQ号为`QQID`的游戏ID绑定

>#### ⚠ 提示
>- 此命令只能被群聊消息触发
>- 若该QQID未绑定会返回消息提示
>- 提示内容可在`settings/Event.json`中自定义，详见[事件](Event.md)

### 获取服务器信息
| 服务器类型 | 对应命令 | 默认端口 |  
| --- | --- | --- |
|Java | `motdje\|<IP>[:端口]`| 25565|
|基岩版 | `motdpe\|<IP>[:端口]`| 19132|

通过发送数据包查询服务器的介绍信息

>#### ⚠ 提示
>- 此命令只能被群聊消息触发
>- 以下几种情况将无法成功，并会返回错误消息 
>   - IP不正确
>   - 端口不正确
>   - 服务器不在运行中
>   - 域名无法解析
>   - 连接超时
>   - 数据包无法识别
>- 错误消息可在`settings/Event.json`中自定义，详见[事件](Event.md)

### 执行Javascript代码
`js|<代码>`  
`javascript|<代码>`
- 运行超时：1min
- js标准：[ECMAScript 5.1(ES5)](http://www.ecma-international.org/ecma-262/5.1/)

#### 内置函数
- `run(string)` `serein.run(string)` 运行一条新的Serein命令
  - 限制：不能执行`motdpe` `motdje` `js` `javascript`命令
- `debug(object)` `serein.log(object)` 输出调试消息到Debug窗口

#### NET对象/类（可直接使用）
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

>#### ⚠ 提示
>`Exception`将输出在Debug窗口

### 调试输出
`debug|<消息>`  
输出调试消息到Debug窗口
>#### ⚠ 提示
>你需要在`settings/Serein.json`中手动开启`Debug模式`