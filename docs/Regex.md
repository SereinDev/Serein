## 正则
一个处理群消息和控制台消息的功能
![截图](imgs/regex.png)
### 入门
正则表达式的基本语法  
- [.NET 正则表达式  Microsoft Docs](https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/regular-expressions)  
- [C# 正则表达式  菜鸟教程](https://www.runoob.com/csharp/csharp-regular-expressions.html)

### 特点
- 若无特别标记，正则表达式仅**匹配第一个符合条件的文本** 
>举个例子  
`(.+?)`匹配``我是一段文本``仅返回第一个字``我``（即使使用贪婪模式也是如此）  
>>解决方法：  
强制匹配整段文本`(.+?)`→`^(.+?)$`
- [条件匹配的表达式](https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/alternation-constructs-in-regular-expressions#conditional-matching-with-an-expression)、[基于有效的捕获组的条件匹配](https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/alternation-constructs-in-regular-expressions#conditional-matching-based-on-a-valid-captured-group)等原生功能**理论可用，但未测试**
- [替换命名组](https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/substitutions-in-regular-expressions#substituting-a-named-group)、[替换整个匹配项](https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/substitutions-in-regular-expressions#substituting-the-entire-match)、[替换匹配项前的文本](https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/substitutions-in-regular-expressions#substituting-the-entire-match)、[替换匹配项后的文本](https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/substitutions-in-regular-expressions#substituting-the-text-after-the-match)、[替换最后捕获的组](https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/substitutions-in-regular-expressions#substituting-the-last-captured-group)、[替换整个输入字符串](https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/substitutions-in-regular-expressions#substituting-the-entire-input-string)等用法**暂不支持**

### 操作方法
- 在正则表格中右键打开菜单
    - `新建记录`→添加正则
    - `修改记录`→编辑正则
      - __正则表达式或命令为空或不合法时无法保存__
    - `删除记录`→删除正则
- 打开`data/regex.tsv`直接编辑正则
    - 格式：`正则表达式` `作用域序号` `管理权限` `备注` `执行命令`（以制表符`\t`分隔）
    - 你可以将他人的记录整行复制在该文件中以合并添加他人的正则
    - 你可以直接将此文件分享给其他人供他人使用
- 也可将`regex.tsv`拖入窗口覆盖导入正则，**此操作不可逆**