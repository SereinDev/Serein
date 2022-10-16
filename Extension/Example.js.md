
|     作者     | Zaitonn                                                                                          |
| :----------: | ------------------------------------------------------------------------------------------------ |
|   **介绍**   | `Serein`的一个JS示例文件                                                                         |
| **更新日期** | 2022.8.24                                                                                        |
| **下载链接** | [GitHub](https://github.com/Zaitonn/Serein/blob/plugins/%E7%A4%BA%E4%BE%8B%E6%96%87%E4%BB%B6.js) |
| **开源仓库** | [GitHub](https://github.com/Zaitonn/Serein/blob/plugins/%E7%A4%BA%E4%BE%8B%E6%96%87%E4%BB%B6.js) |

这个插件是供开发者阅读和学习的，不建议直接投入使用

### 试读片段

```js
// 注册插件，注册成功后会在插件栏显示具体内容
// 若不注册则只显示插件名
serein.registerPlugin("示例文件","v1.1","Zaitonn","示例文件");

/**
 * 注册一个服务器命令，若满足则在输入时拦截
 *
 * 以下情况无法注册
 *  1. 设置>服务器>关服命令中的命令
 *  2. 命令中包含空格
 *
 * 以下情况无法触发
 *  1. 处理函数参数不正确
 *  2. 未输入指定的命令
 *
 * 如下所示，当在服务器控制台输入"example test"时，插件控制台会输出"你输入了注册的命令：example test"
 *
*/
serein.registerCommand("example",example);
function example(cmd){
    serein.log("你输入了注册的命令："+cmd);
}
```