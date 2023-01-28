
# 运行服务器

![服务器](../imgs/console.png)

单击服务器控制面板的"启动"按钮即可启动

## 推荐阅读

- [BDS开服教程 - 手把手教你开服务器](https://www.minebbs.com/threads/bds.9518/)**（极力推荐）**  
- [崩服 / 假死 / 卡顿情况排查与记录方法](https://www.minebbs.com/resources/bds.3403/)  
- [BDS服务端 与 LiteLoaderBDS 新手教程 & 常见问答](https://www.minebbs.com/threads/bds-liteloaderbds.10265/)

## 具体教程

### 下载服务端（基岩版）

在[Minebbs](https://www.minebbs.com/)、[MCBBS](https://www.mcbbs.net/)或其他论坛下载你喜欢的整合包或在官网[下载原版服务端](https://www.minecraft.net/zh-hans/download/server)，解压后打开目录

### 设置启动文件

你有两种设置方案

- 在 设置>服务器>启动文件 页面手动选择文件
- 将启动文件拖入窗口可直接识别

### 启动

点击"启动"按钮即可启动服务器

## 常见问题

### 服务器频繁重启

Serein没有设置重启上限次数，在投入生产环境前请确保服务器能正常运行，以免造成占用过多的系统资源

>[!TIP]
>
>- 暂时关闭 设置>服务器>服务器非正常退出时自动重启
>- 可能是服务器存在异常导致启动失败，请自行参照[https://www.minebbs.com/resources/bds.3403/](https://www.minebbs.com/resources/bds.3403/)向其他大佬询问

### 服务器输入乱码

编码不匹配

>[!TIP]
>在设置中选择相应的编码类型

>[!NOTE]
>在基岩版1.19之后，BDS服务端控制台输入编码被改为UTF-16，但由于一些问题，无论如何修改编码都无法正常输入中文。  
>你可以使用LLSE插件[Unescaper for Serein.js](https://www.minebbs.com/resources/unescaper-for-serein.5441/)，并在设置中开启 使用Unicode字符 选项或使用[Unicode命令输入](Function/Command.md#在服务器中执行命令)将命令内的非ASCII编码的符号使用Unicode字符表示即可

### 服务器输出乱码

编码不匹配

>[!TIP]
>在设置中选择相应的编码类型

>[!NOTE]
>对于Java启动的服务端（包括但不限于Nukkit、PNX），默认编码为GBK。因为NET对该编码不完全支持，所以你可以在启动的批处理文件中添加`-Dfile.encoding=utf-8`参数。  
>举个例子：
>```powershell
>java -Dfile.encoding=utf-8 -jar server.jar nogui
>```

### 使用批处理启动的服务器进程无法强制结束

使用批处理文件启动服务器，使用强制结束功能时发现服务器仍在后台运行

由于批处理启动服务器时，批处理作为Serein的子进程启动，而服务器作为批处理的子进程启动，结束进程功能仅能结束当前运行的进程，而无法结束子进程的子进程（即孙子进程）
