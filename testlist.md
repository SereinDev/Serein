- Serein
  - Console
    - Base
    - Input
  - Base
    - AutoRun
    - Binder
    - Command
    - CrashInterception
    - EventTrigger
    - IO
    - Log
    - Logger
    - Matcher
    - Net
    - ResourcesManager
    - SystemInfo
    - TaskRunner
    - Websocket
  - Item
    - Motd
      - Motdje
      - Motdpe
    - BuilidInfo
    - CommandType
    - LogType
    - Member
    - Regex
    - Task
  - JSPlugin
  - Event
    - JSEngine
    - JSFunc
    - JSLogger
    - JSPluginManager
    - JSWebSocket
    - Plugin
  - Server
    - PluginManager
    - ServerManager
  - Settings
    - Bot
    - Category
    - Event
    - Matches
    - Serein
    - Server

```json
{
  "type": "REGEX",
  "comment": "非必要请不要直接修改文件，语法错误可能导致数据丢失",
  "data": [
    {
      "Regex": "^test$",
      "Remark": "",
      "Command": "g|[CQ:at,qq=%id%] %datetime% %date% %time% %dayofweek% %year%.%month%.%day%.%hour%.%minute%.%second%\n%NET% %OS% %CPUName% %TotalRAM% %UsedRAM% %RAMUsage% %CPUUsage% %UploadSpeed% %DownloadSpeed%\n%Status% %LevelName% %Version% %Difficulty% %RunTime% %ServerCPUUsage% %MaxPlayer% %OnlinePlayer% %Description%\n%Protocol% %GameMode% %Delay% %Favicon% %Origin% %GameID% %Age% %ID% %Nickname% %Sex% %Title% %Role% %ShownName%",
      "Area": 2,
      "IsAdmin": false
    }
  ]
}
```
