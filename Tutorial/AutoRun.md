
# 启动自动运行

>启动时自动执行部分功能（启动服务器和连接WS）

## 通过配置文件

打开 settings/Serein.json 按需修改以下配置项

```json
{
    // ...
    "AutoRun": {
        "StartServer": false,   // 启动服务器
        "ConnectWS": false      // 连接WS
    }
    // ...
}
```

## 通过启动参数

详见[命令行启动参数](Tutorial/SetupArgs.md)
