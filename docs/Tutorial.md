## 教程

### 服务器
1. 从[Release](https://github.com/Zaitonn/Serein/releases/latest)下载Serein
2. 解压后双击运行`Serein.exe`
3. 在[Minebbs](https://www.minebbs.com/)下载整合包或在[官网](https://www.minecraft.net/zh-hans/download/server/bedrock)下载服务端，解压后打开目录
4. 将启动文件拖入窗口，设置为启动文件
5. 点击`启动`按钮即可启动服务器


### [机器人](Bot.md)

- （以[go-cqhttp](https://github.com/Mrs4s/go-cqhttp)为例）
1. 下载并运行，首次运行时会释放启动文件和配置文件
2. 再次运行，当出现提示选择通信方式时，选择``正向 Websocket 通信``
4. 运行并登录帐号（可以在配置文件中直接填写账户密码，也可以直接扫码登陆）
5. 在``Serein``>``设置``>``机器人``>``本地Websocket端口``中设置与机器人配置文件中一致的端口号  
6. 单击``Serein``>``机器人``>``连接``按钮连接Websocket

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

### 插件管理
1. 在插件列表中右键打开菜单
   - 选择`导入插件`添加插件
   - 选择`删除插件`删除插件
   - 选择`启用插件`启用插件
   - 选择`禁用插件`禁用插件
     - 以上两个功能实质是在插件的文件名后面增加`.lock`使加载器不能识别，不更改插件源文件
     - 禁用功能在服务器运行时不可用
   - 选择`打开文件夹`在资源管理器中显示这个所选插件
2. 将所选插件直接拖入窗口，可快捷导入插件
### [正则](Regex.md)

1. 正则表达式的基本语法  
   - [.NET 正则表达式  Microsoft Docs](https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/regular-expressions)  
   - [C# 正则表达式  菜鸟教程](https://www.runoob.com/csharp/csharp-regular-expressions.html)
2. 在正则表格中右键打开菜单
    - 选择`新建记录`添加正则
    - 选择`修改记录`编辑正则
    - 选择`删除记录`删除正则
3. 打开`data/regex.tsv`直接编辑正则
    - 格式：`正则表达式` `作用域序号` `管理权限` `备注` `执行命令`（以制表符`\t`分隔）
    - 你可以将他人的记录整行复制在该文件中以合并添加他人的正则
    - 你可以直接将此文件分享给其他人
4. 将`regex.tsv`拖入窗口覆盖导入正则，**此操作不可逆**

### [定时任务](Schedule.md)
1. 语法
   - [POSIX cron 语法](https://pubs.opengroup.org/onlinepubs/9699919799/utilities/crontab.html#tag_20_25_07)
   - [Crontab Expression](https://github.com/atifaziz/NCrontab/wiki/Crontab-Expression)  
2. 生成器（推荐）
   - [Crontab guru](https://crontab.guru/)   
3. 在定时任务表格中右键打开菜单
    - 选择`添加任务`添加任务
    - 选择`修改任务`编辑任务
      - 在修改窗口中你可以直接看到下一次的执行时间
    - 选择`删除任务`删除任务
4. 打开`data/task.tsv`直接编辑任务
    - 格式：`Cron表达式` `启用` `备注` `执行命令`（以制表符`\t`分隔）
    - 你可以将他人的记录整行复制在该文件中以合并添加他人的任务
    - 你可以直接将此文件分享给其他人
5. 将`task.tsv`拖入窗口覆盖导入任务，**此操作不可逆**