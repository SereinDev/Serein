## 事件

- [事件](#事件)
  - [这是个啥](#这是个啥)
  - [事件一览表](#事件一览表)
  - [绑定/解绑](#绑定解绑)
    - [Bind_Success](#bind_success)
    - [Bind_Occupied](#bind_occupied)
    - [Bind_Invalid](#bind_invalid)
    - [Bind_Already](#bind_already)
    - [Unbind_Success](#unbind_success)
    - [Unbind_Failure](#unbind_failure)
  - [服务器状态变更](#服务器状态变更)
    - [Server_Start](#server_start)
    - [Server_Stop](#server_stop)
    - [Server_Error](#server_error)
  - [QQ群事件](#qq群事件)
    - [Group_Increase](#group_increase)
    - [Group_Decrease](#group_decrease)
    - [Group_Poke](#group_poke)
  - [Motd指令反馈](#motd指令反馈)
    - [Motdpe_Success](#motdpe_success)
    - [Motdje_Success](#motdje_success)
    - [Motd_Failure](#motd_failure)
  - [Serein](#serein)
    - [Serein_Crash](#serein_crash)

### 这是个啥
`Serein`可以根据`settings/Event.json`中的设置响应触发的事件  

在配置文件中，每个事件下有一个数组，你可以在里面填写该事件被触发时执行的[命令](Command.md)，并且可以在其中插入[变量](Variables.md)

>#### ⚠ 提示
>所有事件均无法使用[消息变量（私聊）](Variables.md#消息变量私聊)、[消息变量（群聊）](Variables.md#消息变量群聊)变量

<details>
  <summary>配置文件预览</summary>
  <img src="imgs/Event.png">
</details>

>#### ⚠ 提示
> 将来可能制作专用的编辑窗口

### 事件一览表


>#### ⚠ 提示
>以下所有事件均无法使用[消息变量（私聊）](Variables.md#消息变量私聊)、[消息变量（群聊）](Variables.md#消息变量群聊)变量

| 事件名 | 描述 |
| --- | --- |
| Bind_Success | 绑定成功 |
| Bind_Occupied | 绑定失败-游戏ID占用 |
| Bind_Invalid | 绑定失败-该游戏ID不符合规范|
| Bind_Already | 绑定失败-该QQ已经绑定 |
| Unbind_Success | 解绑成功 |
| Unbind_Failure | 解绑成功 |
| Server_Start | 服务器启动 |
| Server_Stop | 服务器正常关闭 |
| Server_Error | 服务器异常退出 |
| Group_Increase | 监听群有人进群 |
| Group_Decrease | 监听群有人退群 | 
| Group_Poke | 监听群内当前账号被他人戳一戳 |
| Motdpe_Success | 命令`motdpe`执行成功 |
| Motdje_Success | 命令`motdje`执行成功 |
| Motd_Failure | 命令`motdpe`或`motdje`执行失败|
| Serein_Crash | `Serein`崩溃 |

### 绑定/解绑

>#### ⚠ 提示
>本节的所有命令可以使用`ID`变量获取触发这个事件的QQ

#### Bind_Success 
绑定成功  
默认值： `g|[CQ:at,qq=%ID%] 绑定成功`
#### Bind_Occupied
游戏ID占用  
默认值： `g|[CQ:at,qq=%ID%] 该游戏名称被占用`
#### Bind_Invalid
该游戏ID不符合规范  
默认值： `g|[CQ:at,qq=%ID%] 该游戏名称无效`
#### Bind_Already
该QQ已经绑定  
默认值： `g|[CQ:at,qq=%ID%] 你已经绑定过了`
#### Unbind_Success
解绑成功  
默认值： `g|[CQ:at,qq=%ID%] 解绑成功`
#### Unbind_Failure
解绑成功  
默认值： `g|[CQ:at,qq=%ID%] 该账号未绑定`

### 服务器状态变更

#### Server_Start
服务器启动   
默认值： `g|服务器正在启动`
#### Server_Stop
服务器正常关闭   
默认值： `g|服务器已关闭`
#### Server_Error
服务器异常退出  
默认值： `g|服务器异常关闭`

### QQ群事件
>#### ⚠ 提示
>本节的所有命令可以使用`ID`变量获取触发这个事件的QQ

#### Group_Increase 
监听群有人进群  
默认值： `g|欢迎[CQ:at,qq=%ID%]入群~`
#### Group_Decrease 
监听群有人退群  
默认值： 
```
g|用户%ID%退出了群聊，已自动解绑游戏ID
ubind|%ID%
```
#### Group_Poke
监听群内当前账号被他人戳一戳  
默认值： `g|别戳我……(*/ω＼*)`

>#### ⭐ Tips 
>以上三个事件其实很有用，可以欢迎新成员/引导玩家绑定ID/快捷获取服务器状态/提示退群消息/~~整活~~  
>- 示例：
>   ```
>   [CQ:at,qq=%ID%] 呐呐呐~！欢迎来到XXX服务器★(笑)♡今后也.请.多.多.指.教.喔?~
>   ``` 
>   ``` 
>   [CQ:at,qq=%ID%]
>   服务器描述：Dedicated Server
>   版本：1.19.2(527)
>   在线玩家：0/10
>   游戏模式：Survival
>   延迟：114.514ms
>   ```


### Motd指令反馈
#### Motdpe_Success
命令`motdpe`执行成功  
默认值： 
```
g|
服务器描述：%Description%
版本：%Version%(%Protocol%)
在线玩家：%OnlinePlayer%/%MaxPlayer%
游戏模式：%GameMode%
延迟：%Delay%ms
```
#### Motdje_Success
命令`motdje`执行成功  
默认值：
```
g|
服务器描述：%Description%
版本：%Version%(%Protocol%)
在线玩家：%OnlinePlayer%/%MaxPlayer%
延迟：%Delay%ms
```
#### Motd_Failure 
命令`motdpe`或`motdje`执行失败  
默认值： 
```
g|
Motd获取失败
%Exception%
```

>#### ⚠ 提示
>`Motd_Failure`可以使用额外的`Exception`变量输出错误信息

### Serein
#### Serein_Crash
`Serein`崩溃  
默认值： 
```
g|
唔……发生了一点小问题(っ °Д °;)っ
请查看Serein错误弹窗获取更多信息
```