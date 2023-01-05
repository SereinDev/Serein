
|     作者     | Zaitonn                                                                           |
| :----------: | :-------------------------------------------------------------------------------- |
|   **介绍**   | 提供HTTPApi控制                                                                   |
| **更新日期** | 2022.12.20                                                                        |
| **下载链接** | [Serein扩展 - 百度网盘](https://pan.baidu.com/s/1aDcF4ofPpjUIU3jbCMgL5Q?pwd=1234) |

>[!WARNING] 此插件需要在v1.3.3及以上的版本运行，否则可能出现一些意想不到的问题

## 使用示例

[BackupManager对接serein](https://www.minebbs.com/resources/backupmanager-serein.5294/)

## 功能

提供HTTPApi控制，可外接第三方软件使用Serein的部分功能

- [x] 服务器控制
  - [x] 启动服务器
  - [x] 关闭服务器
  - [x] 强制结束服务器
  - [x] 发送命令
- [x] 绑定/解绑
- [x] 通信
  - [x] 数据包
  - [x] 私聊
  - [x] 群聊
- [x] 执行[Serein命令](Function/Command.md)

## 使用方法

1. 将插件复制到 plugins 文件夹下后启动Serein
2. 第一次加载时会生成配置文件，你需要打开 plugins/HTTPApi/config.json 按需修改端口和鉴权凭证
3. 重新加载插件应用配置
4. 使用你的第三方软件控制

## 已知bug

HTTP监听器成功打开后，重新加载插件将导致原端口被占用，可能无法使用一段时间，可以过几分钟后重新加载重试

## 对接文档

>[!TIP] 你需要有一定的编程基础

基网址 `http://127.0.0.1:{端口号}/serein`，使用GET请求

### 返回内容

由于JS解释器兼容性问题，无法写入返回文本，故目前只返回状态码

| 返回代码 | 解释                                             |
| :------: | ------------------------------------------------ |
|  `100`   | 验证成功但未执行任何操作                         |
|  `200`   | 执行成功                                         |
|  `400`   | 操作无效或请求地址错误                           |
|  `401`   | 未通过验证                                       |
|  `406`   | 执行失败（服务器未开启/WS未连接/绑定或解绑失败） |

### 鉴权凭证

参数：`auth`  
类型：`string`

执行任何操作都需要添加此参数，未添加或无效则返回`401`

`http://127.0.0.1:{端口号}/serein?auth=test`

### 操作内容

参数：`operation`  
类型：`string` （不区分大小写）

#### 启动服务器

`startserver`

若服务器已运行或启动文件未找到返回`406`

#### 关闭服务器

`stopserver`

若服务器已关闭返回`406`

#### 强制结束服务器

`killserver`

若服务器已关闭返回`406`

#### 发送命令

`sendcmd`

子参数：`command` 类型：`string`

若服务器已关闭或未提供子参数返回`406`

>[!TIP]
>例：  
>`http://127.0.0.1:{端口号}/serein?auth=test&operation=sendcmd&command=help`  
> 发送`help`命令

#### 发送群聊消息

`sendgroup`

- 子参数
  - `target` 群号 类型：`number`
  - `msg` 消息内容 类型：`string`

若WS未连接返回`406`

#### 发送私聊消息

`sendprivate`

- 子参数
  - `target` QQ号 类型：`number`
  - `msg` 消息内容 类型：`string`

若WS未连接返回`406`

#### 发送数据包

`sendpacket`

子参数：`packet` 类型：`string`

若WS未连接返回`406`

#### 绑定

`bindmember`

- 子参数
  - `userid` QQ号 类型：`number`
  - `gameid` 游戏ID 类型：`string`

若绑定失败返回`406`

#### 解绑

`unbindmember`

子参数：`userid` 类型：`number`

若解绑失败返回`406`
