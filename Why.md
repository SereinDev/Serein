
![logo](https://socialify.git.ci/Zaitonn/Serein/image?description=1&descriptionEditable=%E6%96%B0%E6%97%B6%E4%BB%A3%E6%9E%81%E7%AE%80%E6%9C%8D%E5%8A%A1%E5%99%A8%E9%9D%A2%E6%9D%BF&font=KoHo&logo=https%3A%2F%2Fzaitonn.github.io%2FSerein%2FSerein.png&owner=1&pattern=Circuit%20Board&theme=Light ":no-zoom")

## 麻雀虽小，五脏俱全

**以极小的文件体积囊括多种功能**  
截至`Serein v1.3.2`，所有发行包大小均不超过3MB  
也就是说，不到一张~~色图~~表情包体积的软件就可以实现控制服务器等功能
[![文件体积](imgs/size.png)](https://github.com/Zaitonn/Serein/releases/latest)

## 永不收费且无广告

- `Serein`自从一开始便确定不提供任何增值服务  
- 软件中永远不会加入任何广告，广告统统给爷爪巴

## 永久免费咨询

[加群](https://jq.qq.com/?_wv=1027&amp;k=XNZqPSPv)询问作者或群内其他用户，我们将会尽己所能提供解答和支持

## 倾听用户声音

如果你有好的建议或者需求，都可以[加群](https://jq.qq.com/?_wv=1027&amp;k=XNZqPSPv)或提交Issue提出建议

## 控制面板

![控制台](imgs/console.png)

- **简洁的状态信息显示**  
  - 显示有关服务器的主要信息，快捷了解当前服务器状态
    - 状态
    - 版本
    - 难度
    - 存档名称
    - 运行时长
    - 启动进程的CPU占用率
- 理论上**可适配所有服务器**
  - 据群友反馈，可以**启动BDS、Java乃至于Terraria服务器**
  - ~~只有你想不到，没有`Serein`做不到~~
- 方便的一键控制按钮，包括大部分日常使用的操作，帮助您**一键管理服务器**
- **简洁且可自定义的彩色控制台**
  - 使用自研的正则处理原始输出中的彩色样式，并使用内嵌HTML渲染输出
  - 还有`语法高亮`的显示主题可供选择！可自动用不同颜色高亮`Info`、`Error`、`Warning`、插件前缀和长数字，使得更易于查找关键信息
  - 得益于使用内嵌浏览器渲染控制台，您可以在`./console`文件夹中自定义背景图片、高亮颜色、高亮内容、字体大小颜色等风格
    > 可能需要一点点HTML5、CSS3和JS的入门技巧
    >[!TIP]具体方法详见[教程](Tutorial/CustomConsole.md)
- 自动后台运行
  - 当服务器运行时关闭Serein将**自动最小化至托盘**，再也不用怕点错当场关服跑路
    ![后台运行提示](imgs/ballontooltip.png)

## 插件管理

![插件管理](imgs/plugin.png)

- 自动识别插件文件夹，分组列出所有的插件
- 一键拖入窗口导入插件，帮助小白服主解决导入难题
- 快捷启用/禁用插件，快速排除问题和更新服务器内容
- 直达文件夹，再也不用怕插件太多找不到插件在哪

## 正则

![正则](imgs/regex.png)

>[!TIP]详见👉[正则](Function/Regex.md)👈

- 简单易上手的正则表达式和[命令](Function/Command.md)，可以执行服务器指令、发送消息甚至备份存档（基于cmd，需要自行下载7z）等高级功能
  - *你可以咨询作者获取详细方法或定制*
- 通过此功能还能实现自动回复，与群信息互通和状态输出等功能
- 支持备注功能，再也不用担心写的是啥
- 简单明了的配置文件，可快捷分享或一键导入

## 定时任务

![定时任务](imgs/task.png)

>[!TIP]详见👉[定时任务](Function/Task.md)👈

- 简单易上手的Cron表达式和[命令](Function/Command.md)，可以执行服务器指令、发送消息甚至备份存档（基于cmd命令行，需要自行下载7z）等高级功能
  - *你可以咨询作者获取详细方法或定制*
- 通过此功能还能实现定时命令、准点报时和定时备份等功能
- 自动提示下一次执行时间，再也不用怕搞出逆天的表达式了
- 支持备注功能，再也不用担心写的是啥
- 简单明了的配置文件，可快捷分享或一键导入

## 机器人

![机器人](imgs/bot.png)

>[!TIP]详见👉[机器人](Function/Bot.md)👈

- 支持多种主流机器人，如[`go-cqhttp`](https://github.com/Mrs4s/go-cqhttp)、[`OneBot Mirai`](https://github.com/yyuueexxiinngg/onebot-kotlin)
- `Serein`与机器人窗口分离，方便拓展
  >[!TIP]
  >你可以使用`Nonebot`、`Nonebot2`等机器人框架或[Serein插件](Function/JSPlugin.md)继续拓展实现更多功能
- 通过Websocket一键连接机器人实现消息收发功能
- 允许多开`Serein`实现群组服的消息互通功能
- 显示状态和其他统计信息，帮助您了解运行状态
- 可以直接通过QQ控制服务器或消息互通（需要自行配置）

## 成员管理

![成员管理](imgs/members.png)

>[!TIP]详见👉[成员管理](Function/Member.md)👈

- 将用户的游戏ID与QQ号绑定，有效防止熊孩子使用小号作恶
- 一键删除/修改绑定用户，快捷管理

## 插件

![插件](imgs/javacriptplugins.png)

>[!TIP]详见👉[插件](Function/JSPlugin.md)👈  👉[插件文档](Function/JSDocs.md)👈

- 你可以使用他人编写好的插件直接实现指定功能，方便服主使用
- 由于js引擎的特性，你可以在js中插入部分`C#`代码，增添更多功能
- 提供二十余个函数可供使用和十余个事件可供监听
- 自定义机器人回复/交互/签到等功能

## 设置

![设置](imgs/setting.png)
> 设置内容可能因版本不同而有所差别，图片仅供参考

- 提供尽可能多的设置，满足不同服主的使用需求
- 将鼠标指针放在不同设置上方，可查看关于这一项的详细提示，不用担心不理解设置的意思
