serein.registerPlugin("统计信息记录", "v1.0", "Zaitonn", "自动按时间记录系统信息、在线人数等");

const File = importNamespace('System.IO').File;
const Directory = importNamespace('System.IO').Directory;
const interval = 3000; // 保存周期（ms）
const header = [
    'Time',                     // 时间
    'ServerStatus',             // 服务器状态
    'ServerRunningTime',        // 服务器运行时长
    'ServerProcessCPUUSage',    // 服务器CPU使用率
    'OnlinePlayers',            // 在线玩家数
    'CPUUsage',                 // CPU使用率
    'UsedRAM',                  // 已用内存
    'RAMUsage',                 // 内存使用率
    'UploadSpeed',              // 上行速度
    'DownloadSpeed'             // 下行速度
];

record();
setInterval(record, interval);

function record() {
    if (!Directory.Exists('plugins/StatRecorder')) {
        Directory.CreateDirectory('plugins/StatRecorder');
    }
    let date = new Date();
    let dateStr = `${date.getFullYear()}-${date.getMonth() + 1}-${date.getDate()}`;
    if (!File.Exists(`plugins/StatRecorder/${dateStr}.csv`)) {
        File.WriteAllText(`plugins/StatRecorder/${dateStr}.csv`, header.join(',') + '\n');
    }
    let motd;
    let settings = JSON.parse(serein.getSettings());
    switch (settings.Server.Type) {
        case 0:
            motd = {
                OnlinePlayer: -1
            };
            break;
        case 1:
            motd = new Motdpe('127.0.0.1', settings.Server.Port);
            break;
        case 2:
            motd = new Motdje('127.0.0.1', settings.Server.Port);
            break;
    }
    let sysinfo = serein.getSysInfo();
    let netSpeed = serein.getNetSpeed();
    let list = [
        `${date.getHours()}:${date.getMinutes()}:${date.getSeconds()}`,
        serein.getServerStatus ? '1' : '0',
        serein.getServerTime(),
        serein.getServerCPUUsage(),
        motd.OnlinePlayer,
        serein.getCPUUsage().toFixed(1),
        ((sysinfo.Hardware.RAM.Total - sysinfo.Hardware.RAM.Free) / 1024).toFixed(1),
        ((sysinfo.Hardware.RAM.Total - sysinfo.Hardware.RAM.Free) / sysinfo.Hardware.RAM.Total * 100).toFixed(1),
        netSpeed[0],
        netSpeed[1]
    ];
    File.AppendAllText(`plugins/StatRecorder/${dateStr}.csv`, list.join(',') + '\n');
    serein.log(`[${list[0]}] 写入成功`);
}