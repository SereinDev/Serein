
# 开发工具

## 开启方法

打开 settings/Serein.json 按需修改以下配置项

```json
{
    // ...
    "DevelopmentTool": {
        "EnableDebug": true,            // 开启调试模式
        "DetailDebug": false,           // 详细的调试输出（包含函数参数类型和返回值，仅当上一行启用时生效）
        "JSEventCoolingDownTime": 15,   // JS插件事件冷却时间（ms），调为0则为不限速，但是可能导致占用过高（主要是文件读写）导致无响应。默认值为15
        "NOTE": "以上设置内容为开发专用选项，请在指导下修改"
    }
}
```

>[!ATTENTION] **如果你不知道这些会产生什么效果，请勿随意修改**
