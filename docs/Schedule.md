## 定时任务
一个帮助执行周期性事件的功能
![截图](imgs/task.png)
### 入门  
#### 文档  
- [POSIX cron 语法](https://pubs.opengroup.org/onlinepubs/9699919799/utilities/crontab.html#tag_20_25_07)
- [Crontab Expression](https://github.com/atifaziz/NCrontab/wiki/Crontab-Expression)  

  
#### 生成器  
- [Crontab guru](https://crontab.guru/)   


### 特点
- 为减少计算量，可能存在一定偏差（<4000ms），但不会叠加
>举个例子   
假设一定时任务为``* * * * *``，代表在每一分钟执行该任务，可能在这分钟的第0秒到第4秒的任意时刻执行

### 操作方法
- 在定时任务表格中右键打开菜单
    - `添加任务`→添加任务
    - `修改任务`→编辑任务
      - __Cron表达式或命令为空或不合法时无法保存__
      - 在修改窗口中你可以直接看到下一次的执行时间
    - `删除任务`→删除任务
- 打开`data/task.tsv`直接编辑任务
    - 格式：`Cron表达式` `启用` `备注` `执行命令`（以制表符`\t`分隔）
    - 你可以将他人的记录整行复制在该文件中以合并添加他人的任务
    - 你可以直接将此文件分享给其他人供他人使用
- 也可将`task.tsv`拖入窗口覆盖导入任务，**此操作不可逆**