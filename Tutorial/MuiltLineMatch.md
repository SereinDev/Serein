
# 多行匹配

>对`list`等多行反馈的命令进行匹配

由于Serein中为单行匹配服务器输出，所以针对跨行输出的匹配提供了一个额外的解决方案

>[!TIP] 你可能需要一点关于[JSON](https://www.runoob.com/json/json-tutorial.html)经验

### 操作方法

以如下的文本为例，现需要匹配玩家数量和玩家列表

```txt
>list
12:53:54 INFO [Server] There are 1/20 players online:
12:53:54 INFO [Server] Player1
```

1. 在 settings/Matches.json 中修改`MuiltLines`属性
2. 在其中添加能够匹配第一行的正则，此处则为`players\sonline:`
3. 在Serein的正则页面添加匹配两行的正则及命令，此处则应为`(.+?)\splayers\sonline\n.+?\](.+?)$`

>[!WARNING]此处你需要注意的是，添加进去时应以`"`包裹，且文本其中的`"`应换成`\"`，`\`换成`\\`  

修改后文件的应该长这样

```json
{
    // ...
    "MuiltLines": [
        "players\\sonline:",
        "其他多行匹配的正则"
    ]
    // ...
}
```

>[!TIP]若不只有一种输出涉及多行匹配，你可以像上面这样添加多个正则
