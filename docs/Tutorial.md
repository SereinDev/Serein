## 教程
- [教程](#教程)
  - [安装Serein](#安装serein)
    - [Release](#release)
    - [Beta](#beta)
    - [自行编译](#自行编译)
    - [如何选择不同Net的版本](#如何选择不同net的版本)
  - [服务器](#服务器)
  - [自定义控制台样式](#自定义控制台样式)
  - [多开Serein](#多开serein)
  - [机器人](#机器人)
    - [支持的条件](#支持的条件)
    - [配置方法（以go-cqhttp为例）](#配置方法以go-cqhttp为例)
  - [插件管理](#插件管理)
  - [正则](#正则)
    - [正则表达式的基本语法](#正则表达式的基本语法)
    - [操作方法](#操作方法)
    - [特点](#特点)
  - [定时任务](#定时任务)
    - [语法](#语法)
    - [生成器（推荐）](#生成器推荐)
    - [操作方法](#操作方法-1)
    - [特点](#特点-1)

### 安装Serein
#### Release
1. 从[Release](https://github.com/Zaitonn/Serein/releases/latest)下载Serein
2. 解压后双击运行`Serein.exe`

#### Beta
在[Action](https://github.com/Zaitonn/Serein/actions)下载最新的构建  
>**Beta版不能保证稳定性，可能存在Bug，使用前务必自行备份**

#### 自行编译
从[Release](https://github.com/Zaitonn/Serein/releaseslatest)下载`Source.zip`，使用vs或其他编译器编译运行（需要SDK）

#### 如何选择不同Net的版本
- .NET 6.0 
  - 运行速度较快，若追求处理速度可选择此版本 
  > 详见[Performance Improvements in .NET 6 （.NET 6 中的性能改进）](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-6/)

  - 系统一般不自带运行库，需自行安装 [`.NET 运行时`](https://dotnet.microsoft.com/download/dotnet/6.0/runtime/desktop/x64)

- .NET Framework 4.7.2
  - Win10及以上等大部分系统自带，可不需要手动安装，方便使用
    - 你也可以自行安装 [`.NET Framework`](https://dotnet.microsoft.com/zh-cn/download/dotnet-framework/net472)



### 服务器
![服务器](imgs/console.png)
>**首先你应该先看看：**  
[BDS开服教程 - 手把手教你开服务器](https://www.minebbs.com/threads/bds.9518/)**（推荐）**  
[崩服 / 假死 / 卡顿情况排查与记录方法](https://www.minebbs.com/resources/bds.3403/)  
[BDS服务端 与 LiteLoaderBDS 新手教程 & 常见问答](https://www.minebbs.com/threads/bds-liteloaderbds.10265/)

1. 在[Minebbs](https://www.minebbs.com/)、[MCBBS](https://www.mcbbs.net/)下载你喜欢的整合包或在[官网](https://www.minecraft.net/zh-hans/download/server/bedrock)下载原版服务端，解压后打开目录
2. 设置为启动文件
   - 在`设置`>`服务器`>`启动文件`设置 
   - 将启动文件拖入窗口可直接识别设置
3. 点击`启动`按钮即可启动服务器

### 自定义控制台样式
1. 打开`./console`文件夹
2. 在Visual Studio Code或者其他编辑器中编辑其中的文件
   - 应该注意：
      1. `console.js`中定义的函数**不建议更改**，错误的修改可能导致无法输出信息
      2. 由于显示网页组件以IE为内核，故内嵌网页需**适配IE浏览器**
      > 听说IE于2022年6月15日停止支持？不慌 ~~管他的，大不了再修复 反正已经过了~~
3. 重启`Serein`后生效



### 多开Serein

> Mulit-Open，指在本地开启多个服务端并接入同一个机器人统一控制，适用于群组服等   

> 通常不建议直接多次双击`Serein.exe`来开启多个窗口，这可能**导致数据无法保存甚至崩溃**

```
│
├─文件夹1
│  │ Serein.exe  
│  └─...
├─文件夹2
│  │ Serein.exe  
│  └─...
└──文件夹3
   │ Serein.exe  
   └─...
```
1. 将`Serein.exe`上层文件夹复制多份，并重命名区分。目录结构如上所示
2. 分别启动其中的`Serein.exe`，选择不同的文件启动，可以独立保存数据和设置
   - 你可能需要对不同的`Serein`的正则或定时任务乃至设置进行相应配置，否则可能出现一呼百应的情况
3. 对于机器人你可以选择开启`上报自身消息`，通过对该消息进行匹配从而实现群组服消息互通

### 机器人

![机器人](imgs/bot.png)
#### 支持的条件
- 使用[OneBot-11](https://github.com/botuniverse/onebot-11)标准
- 可使用WS正向连接

>由于不同机器人之间标准可能存在差异，不一定保证100%适配所有机器人 

>目前已完全支持的机器人：[`go-cqhttp`](https://github.com/Mrs4s/go-cqhttp)、[`OneBot Mirai`](https://github.com/yyuueexxiinngg/onebot-kotlin)  
>❗ **此处列举的机器人只代表已经经过测试且可用，并不是只有以上两种机器人可用**

#### 配置方法（以go-cqhttp为例）
1. 下载并运行，首次运行时会释放启动文件和配置文件
2. 再次运行，当出现提示选择通信方式时，选择``正向 Websocket 通信``
3. 运行并登录帐号（可以在配置文件中直接填写账户密码，也可以直接扫码登陆）
4. 在``Serein``>``设置``>``机器人``>``本地Websocket端口``中设置与机器人配置文件中一致的端口号  
5. 单击``Serein``>``机器人``>``连接``按钮连接Websocket

```
未找到配置文件，正在为您生成配置文件中！  
请选择你需要的通信方式:  
 0: HTTP通信  
 1: 云函数服务  
 2: 正向 Websocket 通信
 3: 反向 Websocket 通信  
 4: pprof 性能分析服务器  
请输入你需要的编号(0-9)，可输入多个，同一编号也可输入多个(如: 233)
``` 
如上所示，你应该选择 `2: 正向 Websocket 通信`  
   
其他机器人可能需要直接修改配置文件，但操作方法类似
### 插件管理
![插件管理](imgs/plugin.png)

-  在插件列表中右键打开菜单
   - `导入插件`→添加插件
   - `删除插件`→删除插件
   - `启用插件`→启用插件
   - `禁用插件`→禁用插件
     - 以上两个功能实质是在插件的文件名后面增加`.lock`使加载器不能识别，并不更改插件文件内容
     - 禁用功能在服务器运行时不可用
   - `打开文件夹`→在资源管理器中显示这个所选插件
- 将所选插件直接拖入窗口，可快捷导入插件


### 正则
![正则](imgs/regex.png)

#### 正则表达式的基本语法  
- [.NET 正则表达式  Microsoft Docs](https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/regular-expressions)  
- [C# 正则表达式  菜鸟教程](https://www.runoob.com/csharp/csharp-regular-expressions.html)

#### 操作方法
- 在正则表格中右键打开菜单
    - `新建记录`→添加新的正则
    - `修改记录`→编辑所选正则
      - **正则表达式或命令为空或不合法时无法保存**
      - 参考文档：[变量](Variables.md)、[命令](Command.md)
    - `删除记录`→删除所选正则
- 文件保存在`data/regex.json`
    - 你可以将他人的记录整行复制在该文件中以合并添加他人的正则
    - 你可以直接将此文件分享给其他人供他人使用
    - 也可将数据文件拖入窗口覆盖导入正则，但是要注意**此操作不可逆**
  
```jsonc
{ // 示例文件
  "type": "REGEX",
  "comment": "非必要请不要直接修改文件，语法错误可能导致数据丢失",
  "data": [
    {
      "Regex": "^(.+?)$",  // 正则表达式
      "Remark": "",  // 备注
      "Command": "",  // 执行命令
      "Area": 0,  // 作用域
      "IsAdmin": false  // 需要管理权限 
    }
  ]
}
```

#### 特点
- 若无特别标记，正则表达式仅**匹配第一个符合条件的文本** 
>举个例子  
`(.+?)`匹配``我是一段文本``仅返回第一个字``我``（即使使用贪婪模式也是如此）  
>>解决方法：  
强制匹配整段文本`(.+?)`→`^(.+?)$`
- [条件匹配的表达式](https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/alternation-constructs-in-regular-expressions#conditional-matching-with-an-expression)、[基于有效的捕获组的条件匹配](https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/alternation-constructs-in-regular-expressions#conditional-matching-based-on-a-valid-captured-group)等原生功能**理论可用，但未测试**
- [替换命名组](https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/substitutions-in-regular-expressions#substituting-a-named-group)、[替换整个匹配项](https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/substitutions-in-regular-expressions#substituting-the-entire-match)、[替换匹配项前的文本](https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/substitutions-in-regular-expressions#substituting-the-entire-match)、[替换匹配项后的文本](https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/substitutions-in-regular-expressions#substituting-the-text-after-the-match)、[替换最后捕获的组](https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/substitutions-in-regular-expressions#substituting-the-last-captured-group)、[替换整个输入字符串](https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/substitutions-in-regular-expressions#substituting-the-entire-input-string)等用法**暂不支持**

### 定时任务
![定时任务](imgs/task.png)
#### 语法
- [POSIX cron 语法](https://pubs.opengroup.org/onlinepubs/9699919799/utilities/crontab.html#tag_20_25_07)
- [Crontab Expression](https://github.com/atifaziz/NCrontab/wiki/Crontab-Expression) 

#### 生成器（推荐）
[Crontab guru](https://crontab.guru/)

#### 操作方法
- 在定时任务表格中右键打开菜单
    - `添加任务`→添加新的任务
    - `修改任务`→编辑所选任务
      - **Cron表达式或命令为空或不合法时无法保存**
      - 参考文档：[变量](Variables.md)、[命令](Command.md)
      - 在修改窗口中你可以直接看到下一次的执行时间
    - `删除任务`→删除所选任务
- 文件保存在`data/task.json`
    - 你可以将他人的记录整行复制在该文件中以合并添加他人的任务
    - 你可以直接将此文件分享给其他人供他人使用
    - 也可将数据文件拖入窗口覆盖导入任务，但是要注意**此操作不可逆**
```jsonc
{  // 示例文件
  "type": "TASK",
  "comment": "非必要请不要直接修改文件，语法错误可能导致数据丢失",
  "data": [
    {
      "Cron": "* * * * *",  // Cron表达式
      "Command": "s|测试",  // 执行命令
      "Remark": "",  // 备注
      "Enable": true  // 启用
    }
  ]
}
```

#### 特点
为减少计算量，可能存在一定偏差（<4000ms），但不会叠加
>举个例子   
假设一定时任务为``* * * * *``，代表在每一分钟执行该任务，可能在这分钟的第0秒到第4秒的任意时刻执行