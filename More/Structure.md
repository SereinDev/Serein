
# 目录结构

## 主分支

- .github *github相关*
- Serein
  - Console
    - Console *控制台输入输出处理*
  - Universal *三个类型共通的代码*
    - Base *一些基础的方法*
    - ConsoleHtml *控制台html文件*
    - Items *一坨类*
      - Motd *关于Motd实现的类*
    - JSPlugin *JS插件*
    - Server *服务器及其插件管理*
    - Settings *设置*
    - Sources *图标资源*
  - WinForm
    - UI *界面代码*
  - WPF
    - Windows *界面代码*

## 程序

- 文件夹
  - console *仅WPF和Winform*
    - console.html *控制台html文件*
    - preset.css *预设*
  - data *数据*
    - members.json *成员管理*
    - regex.json *正则*
    - task.json *定时任务*
  - logs *日志*
    - console *控制台输出*
    - crash *崩溃日志*
    - debug *调试日志*
    - msg *机器人接收数据包*
  - plugins *[插件](Function/JSPlugin.md)文件夹*
  - Serein-???.dll *仅dotnet6.0*
  - Serein-???.exe *程序主体*
  - Serein-???.exe.config *仅dotnet framework 4.7.2*
