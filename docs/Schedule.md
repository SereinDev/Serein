## 定时任务
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