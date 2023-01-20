serein.registerPlugin("聊天增强", "v1.2", "Zaitonn", "对群聊消息互通提供更多增强功能");

let config = {
    disableColorSymbol: true,
    enableGameID: true,
    ignore: [],
    prefix: '',
    temple: '',
    cqCode: {
        '[CQ:face]': '[表情]',
        '[CQ:reply]': '[回复]',
        '[CQ:image]': '[图片]',
        '[CQ:video]': '[视频]',
        '[CQ:record]': '[语音]',
        '[CQ:music]': '[音乐]',
        '[CQ:redbag]': '[红包]',
        '[CQ:forward]': '[合并转发消息]',
        '[CQ:node]': '[合并转发消息]',
        '[CQ:xml]': '[XML卡片]',
        '[CQ:json]': '[JSON卡片]',
    }
};

const File = importNamespace('System.IO').File;
const Directory = importNamespace('System.IO').Directory;
let cache, playCount = -1, settings = {};

setInterval(() => { // 判断服务器是否有人
    settings = JSON.parse(serein.getSettings());
    if (serein.getServerStatus()) {
        let motd;
        switch (settings.Server.Type) {
            case 1:
                motd = new Motdpe(`127.0.0.1:${settings.Server.Port}`);
                break;
            case 2:
                motd = new Motdje(`127.0.0.1:${settings.Server.Port}`);
                break;
            default:
                motd = { OnlinePlayer: -1 };
        }
        playCount = motd.OnlinePlayer === '-' ? -1 : (Number)(motd.OnlinePlayer);
    }
}, 1000);

setInterval(() => { // 每10s保存一次缓存
    File.WriteAllText('plugins/CHATEX/cache.json', JSON.stringify(cache, null, 4));
}, 10000);

read('plugins/CHATEX', 'cache.json', '{}');
cache = JSON.parse(read('plugins/CHATEX', 'cache.json', null));
read('plugins/CHATEX', 'config.json', JSON.stringify(config, null, 4));
config = JSON.parse(read('plugins/CHATEX', 'config.json', null));

serein.setListener('onReceiveGroupMessage', (groupid, userid, msg, shownName) => {
    if (settings.Bot.GroupList.indexOf(groupid) >= 0) {
        // 缓存用户昵称信息
        if (!cache[(String)(groupid)]) {
            cache[(String)(groupid)] = {};
        }
        cache[(String)(groupid)][(String)(userid)] = shownName;
        cache["last_update"] = Date.now();
        if (
            msg.indexOf(config.prefix) === 0 &&
            config.ignore.indexOf(groupid) === -1 &&
            playCount > 0) {
            let text = parseAt(fitter(msg), groupid);
            let date = new Date();
            let time = `${date.getHours()}:${date.getMinutes()}:${date.getSeconds()}`;
            serein.log(`[${time}]${shownName}:${text}`);
            switch (settings.Server.Type) {
                case 1:
                    serein.sendCmd(`tellraw @a ${JSON.stringify({
                        rawtext: [
                            {
                                text: `§b[${time}]§r${config.disableColorSymbol ? shownName.replace(/\u00a7./, '') : shownName}:${text}`.replace(/\%u/g, '\\u')
                            }
                        ]
                    })
                        }`);
                    break;
                case 2:
                    serein.sendCmd(`tellraw @a ${JSON.stringify({
                        rawtext: {
                            text: `§b[${time}]§r${config.disableColorSymbol ? shownName.replace(/\u00a7./, '') : shownName}:${text}`
                        }
                    })
                        }`);
                    break;
                default:
                    throw new Error('你需要在<设置-服务器-服务器类型>指定你所使用的服务器类型');
            }
        }
    }
});

/**
 * @description 去除不必要的CQ字符
 * @param {String} text 文本
 * @returns 过滤后的文本
 */
function fitter(text) {
    if (config.disableColorSymbol) {
        text = text.replace(/\u00a7[a-f0-9]/i, "");
    }
    if (text.indexOf('CQ') < 0) {
        return text;
    } else {
        text = text.replace(/\[CQ:at,qq=all\]/g, '@全体成员');
        text = text.replace(/\[CQ:at,.+?name=([^,]+?).+?\]/g, '@$1');
        text = text.replace(/\[CQ:at,qq=(\d+)\]/g, '@$1');
        text = text.replace(/\[CQ:([^,]+?),.+?\]/g, '[CQ:$1]');
        text = text.replace(/\[CQ:\w+\]/g, (j) => {
            let k = config.cqCode[j];
            if (typeof (k) == "undefined") {
                return '[不支持预览的内容]';
            }
            else {
                return k;
            }
        });
        return text;
    }
}

/**
 * @description 转义At
 * @param {string} text 
 * @returns 转义后文本
 */
function parseAt(text, groupid) {
    if (!/@\d+/.test(text) || cache[(String)(groupid)] == undefined) {
        return text;
    } else {
        let match = text.match(/(?<=@)\d+/g);
        serein.log(JSON.stringify(match));
        for (let i = 0; i < match.length; i++) {
            let gameid = serein.getGameID((Number)(match[i]));
            if (config.enableGameID && gameid) {
                text = text.replace(`@${match[i]}`, `@${gameid}`);
            }
            else if (cache[(String)(groupid)][match[i]]) {
                text = text.replace(`@${match[i]}`, `@${cache[(String)(groupid)][match[i]]}`);
            }
        }
        return text;
    }
}

/**
 * @description 读取文件
 * @param {String} directory 目录
 * @param {String} file 文件名
 * @param {String} _default 默认值
 * @returns 读取内容
 */
function read(directory, file, _default) {
    if (!Directory.Exists(directory)) {
        Directory.CreateDirectory(directory);
    }
    if (!File.Exists(directory + '/' + file) && _default) {
        File.WriteAllText(directory + '/' + file, _default);
    } else {
        return File.ReadAllText(directory + '/' + file);
    }
    return undefined;
}
