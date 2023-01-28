
# 定时任务

>安排定期执行的任务

![定时任务](../imgs/task.png)

任务数据保存在 data/task.json

> [!TIP]
>
>- 将他人的记录复制在该文件中以合并添加他人的任务
>- 直接将此文件分享给其他人供他人使用
>- 将数据文件拖入窗口覆盖导入任务，但是要注意**此操作不可逆**

## 食用方法

在任务表格中右键打开菜单

> [!TIP]
>在编辑窗口中你可以直接看到下一次的执行时间

>[!WARNING]Cron表达式或命令为空或不合法时无法保存

## 功能介绍

### Cron表达式

指定任务执行的时间和周期

> [!TIP]
>生成器（推荐）：[Crontab guru](https://crontab.guru/)  
>语法：[POSIX cron 语法](https://pubs.opengroup.org/onlinepubs/9699919799/utilities/crontab.html#tag_20_25_07) ，[Crontab Expression](https://github.com/atifaziz/NCrontab/wiki/Crontab-Expression)

### 执行命令

执行一条[Serein命令](Function/Command.md)，你可以在其中插入[变量](Function/Variables.md)

### 备注

对这项内容的备注或注释，不影响匹配

## 文件格式

```json
{
  "type": "TASK",
  "comment": "非必要请不要直接修改文件，语法错误可能导致数据丢失",
  "data": [
    {
      "Cron": "* * * * *",  // Cron表达式
      "Command": "s|测试",  // 执行命令
      "Remark": "",  // 备注
      "Enable": true  // 启用
    }
  ]
}
```
