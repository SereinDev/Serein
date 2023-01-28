
# CHATEX

|     作者     | Zaitonn                                    |
| :----------: | :----------------------------------------- |
|   **介绍**   | 对群聊消息互通提供更多增强功能             |
| **更新日期** | 2023.1.12                                  |
| **下载** | [CHATEX.js](JS/CHATEX/CHATEX.js ':ignore') |

## 使用方法

删除或禁用原有的正则即可

## 配置文件说明

```json
{
    "disableColorSymbol": true, // 禁用颜色代码
    "enableGameID": true,       // 启用游戏ID转换
    "ignore": [],               // 忽略的群聊列表
    "prefix": "",               // 触发前缀
    "temple": "",               // 暂时没用
    "cqCode": {                 // CQ码替换字典
        "[CQ:face]": "[表情]",
        "[CQ:reply]": "[回复]",
        "[CQ:image]": "[图片]",
        "[CQ:video]": "[视频]",
        "[CQ:record]": "[语音]",
        "[CQ:music]": "[音乐]",
        "[CQ:redbag]": "[红包]",
        "[CQ:forward]": "[合并转发消息]",
        "[CQ:node]": "[合并转发消息]",
        "[CQ:xml]": "[XML卡片]",
        "[CQ:json]": "[JSON卡片]"
    }
}
```
