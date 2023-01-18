/**
    @description Serein JS插件辅助

    @summary 使用方法：
        0. 建议使用 Visual Studio Code 编写插件 下载：https://code.visualstudio.com/

        1. 将此文件复制到插件的同文件夹下

        2. 在你的插件第一行加上下面这一行（只需复制"///"及其之后的部分）
            /// <reference path="SereinJSPluginHelper v1.3.3.js"/>

        3. 然后你就可以快乐地写插件了，这时候就可以自动补全和显示函数参数了！！

    @version 适用版本：
        Serein v1.3.3.1 +
 */


var serein = {
    /**
     * @description 所在文件夹
     * @see https://serein.cc/#/Function/JSDocs/Properties?id=sereinexe%e6%89%80%e5%9c%a8%e6%96%87%e4%bb%b6%e5%a4%b9
     */
    path: "",

    /**
     * @description Serein版本
     * @see https://serein.cc/#/Function/JSDocs/Properties?id=serein%e7%89%88%e6%9c%ac
     */
    version: "",

    /**
     * @description JS命名空间
     * @see https://serein.cc/#/Function/JSDocs/Properties?id=js%e5%91%bd%e5%90%8d%e7%a9%ba%e9%97%b4
     */
    namespace: "",

    /**
     * @description 输出日志
     * @param {*} content 内容
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e8%be%93%e5%87%ba%e6%97%a5%e5%bf%97
     */
    log: function (content) { },

    /**
     * @description Debug输出
     * @param {*} content 内容
     * @see https://serein.cc/#/Function/JSDocs/Func?id=debug%e8%be%93%e5%87%ba
     */
    debugLog: function (content) { },

    /**
     * @description 注册插件
     * @param {String} name 名称
     * @param {String} version 版本
     * @param {String} author 作者
     * @param {String} description 详细描述
     * @returns {String} 注册结果
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e6%b3%a8%e5%86%8c%e6%8f%92%e4%bb%b6
     */
    registerPlugin: function (name, version, author, description) { },

    /**
     * @description 设置监听器
     * @param {String} eventname 事件名称，详见下方链接
     * @param {Function} func 回调函数
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e8%ae%be%e7%bd%ae%e7%9b%91%e5%90%ac%e5%99%a8
     */
    setListener: function (eventname, func) { },

    /**
     * @description 获取Serein设置
     * @returns {String} 设置的json文本
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e8%8e%b7%e5%8f%96serein%e8%ae%be%e7%bd%ae
     */
    getSettings: function () { },

    /**
     * @description 执行命令
     * @param {String} command 一条Serein命令
     * @summary 此处无法执行绑定或解绑ID、获取motd和执行js代码的命令
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e6%89%a7%e8%a1%8c%e5%91%bd%e4%bb%a4
     */
    runCommand: function (command) { },

    /**
     * @description 获取系统信息
     * @returns {Object} 系统信息对象
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e8%8e%b7%e5%8f%96%e7%b3%bb%e7%bb%9f%e4%bf%a1%e6%81%af
     */
    getSysInfo: function () { },

    /**
     * @description 获取CPU使用率
     * @returns {Number | undefined} CPU使用率
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e8%8e%b7%e5%8f%96cpu%e4%bd%bf%e7%94%a8%e7%8e%87
     */
    getCPUPersent: function () { },

    /**
     * @description 获取网速
     * @returns {Array<String>} 网速字符串数组，[0]为上传网速，[1]为下载网速
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e8%8e%b7%e5%8f%96%e7%bd%91%e9%80%9f
     */
    getNetSpeed: function () { },

    /**
     * @description 启动服务器
     * @returns {Boolean} 启动结果
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e5%90%af%e5%8a%a8%e6%9c%8d%e5%8a%a1%e5%99%a8
     */
    startServer: function () { },

    /**
     * @description 关闭服务器
     * @returns {Boolean} 关闭结果
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e5%85%b3%e9%97%ad%e6%9c%8d%e5%8a%a1%e5%99%a8
     */
    stopServer: function () { },

    /**
     * @description 强制结束服务器
     * @returns {Boolean} 强制结束结果
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e5%bc%ba%e5%88%b6%e7%bb%93%e6%9d%9f%e6%9c%8d%e5%8a%a1%e5%99%a8
     */
    killServer: function () { },

    /**
     * @description 发送服务器命令
     * @param {String} command 命令内容
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e5%8f%91%e9%80%81%e6%9c%8d%e5%8a%a1%e5%99%a8%e5%91%bd%e4%bb%a4
     */
    sendCmd: function (command) { },

    /**
     * @description 获取服务器状态
     * @returns {Boolean} 当前状态
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e8%8e%b7%e5%8f%96%e6%9c%8d%e5%8a%a1%e5%99%a8%e7%8a%b6%e6%80%81
     */
    getServerStatus: function () { },

    /**
     * @description 获取服务器运行时长
     * @returns {String} 时长字符串，格式见文档
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e8%8e%b7%e5%8f%96%e6%9c%8d%e5%8a%a1%e5%99%a8%e8%bf%90%e8%a1%8c%e6%97%b6%e9%95%bf
     */
    getServerTime: function () { },

    /**
     * @description 获取服务器进程占用
     * @returns {Boolean} 进程CPU占用率
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e8%8e%b7%e5%8f%96%e6%9c%8d%e5%8a%a1%e5%99%a8%e8%bf%9b%e7%a8%8b%e5%8d%a0%e7%94%a8
     */
    getServerCPUPersent: function () { },

    /**
     * @description 获取服务器文件
     * @returns {String} 文件名
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e8%8e%b7%e5%8f%96%e6%9c%8d%e5%8a%a1%e5%99%a8%e6%96%87%e4%bb%b6
     */
    getServerFile: function () { },

    /**
     * @description 获取Motd原文（基岩版）
     * @param {String} addr 服务器地址（支持域名、端口）
     * @returns {String} 获取到的文本（需要自己处理）
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e8%8e%b7%e5%8f%96motd%e5%8e%9f%e6%96%87
     */
    getMotdpe: function (addr) { },

    /**
     * @description 获取Motd原文（Java版）
     * @param {String} addr 服务器地址（支持域名、端口）
     * @returns {String} 获取到的文本（需要自己处理）
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e8%8e%b7%e5%8f%96motd%e5%8e%9f%e6%96%87
     */
    getMotdje: function (addr) { },

    /**
     * @description 发送群聊消息
     * @param {Number} groupid 群号
     * @param {String} msg 消息文本
     * @returns {Boolean} 发送结果
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e5%8f%91%e9%80%81%e7%be%a4%e8%81%8a%e6%b6%88%e6%81%af
     */
    sendGroup: function (groupid, msg) { },

    /**
     * @description 发送私聊消息
     * @param {Number} userid 对方qq
     * @param {String} msg 消息文本
     * @returns {Boolean} 发送结果
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e5%8f%91%e9%80%81%e7%a7%81%e8%81%8a%e6%b6%88%e6%81%af
     */
    sendPrivate: function (userid, msg) { },

    /**
     * @description 发送数据包
     * @param {String} packet 数据包文本
     * @returns {Boolean} 发送结果
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e5%8f%91%e9%80%81%e6%95%b0%e6%8d%ae%e5%8c%85
     */
    sendPacket: function (packet) { },

    /**
     * @description 获取ws连接状态
     * @returns {Boolean} 连接状态
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e8%8e%b7%e5%8f%96ws%e8%bf%9e%e6%8e%a5%e7%8a%b6%e6%80%81
     */
    getWsStatus: function () { },

    /**
     * @description 绑定游戏ID
     * @param {Number} userid QQ号
     * @param {String} gameid 游戏ID，需符合/^[a-zA-Z0-9_\s-]{4,16}$/
     * @returns {Boolean} 绑定结果
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e7%bb%91%e5%ae%9a%e6%b8%b8%e6%88%8fid
     */
    bindMember: function (userid, gameid) { },

    /**
     * @description 删除绑定记录
     * @param {Number} userid QQ号
     * @returns {Boolean} 解绑结果
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e5%88%a0%e9%99%a4%e7%bb%91%e5%ae%9a%e8%ae%b0%e5%bd%95
     */
    unbindMember: function (userid) { },

    /**
     * @description 获取指定用户QQ
     * @param {*} gameid 游戏ID
     * @returns {Number} QQ号
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e8%8e%b7%e5%8f%96%e6%8c%87%e5%ae%9a%e7%94%a8%e6%88%b7qq
     */
    getID: function (gameid) { },

    /**
     * @description 获取指定游戏ID
     * @param {Number} userid QQ号
     * @returns {String} 游戏ID
     * @see https://serein.cc/#/Function/JSDocs/Func?id=%e8%8e%b7%e5%8f%96%e6%8c%87%e5%ae%9a%e6%b8%b8%e6%88%8fid
     */
    getGameID: function (userid) { },

}

/**
 * 引入命名空间
 * @param {String} namespace
 * @returns {Object} 命名空间动态对象，具体路径见 https://docs.microsoft.com/zh-cn/dotnet/api/?view=net-6.0
 */
function importNamespace(namespace) { };

/**
 * @description Java服务器Motd对象
 */
class Motdje {
    /**
     * @description Java服务器Motd对象
     * @param {String} addr 服务器地址
     */
    constructor(addr) { };

    /**
     * @description 最大玩家数
     */
    MaxPlayer = "-";

    /**
     * @description 在线玩家数
     */
    OnlinePlayer = "-";

    /**
     * @description 服务器描述
     */
    Description = "-";

    /**
     * @description 协议
     */
    Protocol = "-";

    /**
     * @description 存档名称（仅基岩版有意义）
     */
    LevelName = "-";

    /**
     * @description 游戏模式（仅基岩版有意义）
     */
    GameMode = "-";

    /**
     * @description 图标（CQ码）（仅Java有意义）
     */
    Favicon = "-";

    /**
     * @description 延迟（ms）
     * @example var delay = new Motdje('...').Delay.TotalMilliseconds;
     * @type {Object}
     */
    Delay;

    /**
     * @description 原文
     */
    Origin = "-";

    /**
     * @description 错误消息
     */
    Exception = "";

    /**
     * @description 是否获取成功
     */
    IsSuccess = false;
}

/**
 * @description 基岩版服务器Motd对象
 */
class Motdpe {
    /**
     * @description 基岩版服务器Motd对象
     * @param {String} addr 服务器地址
     */
    constructor(addr) { };

    /**
     * @description 最大玩家数
     */
    MaxPlayer = "-";

    /**
     * @description 在线玩家数
     */
    OnlinePlayer = "-";

    /**
     * @description 服务器描述
     */
    Description = "-";

    /**
     * @description 协议
     */
    Protocol = "-";

    /**
     * @description 存档名称（仅基岩版有意义）
     */
    LevelName = "-";

    /**
     * @description 游戏模式（仅基岩版有意义）
     */
    GameMode = "-";

    /**
     * @description 图标（CQ码）（仅Java有意义）
     */
    Favicon = "-";

    /**
     * @description 延迟（ms）
     * @example var delay = new Motdje('...').Delay.TotalMilliseconds;
     * @type {Object}
     */
    Delay;

    /**
     * @description 原文
     */
    Origin = "-";

    /**
     * @description 错误消息
     */
    Exception = "";

    /**
     * @description 是否获取成功
     */
    IsSuccess = false;
}

/**
 * @description Logger
 */
class Logger {
    /**
     * @description Logger
     * @param {String} title 标题
     */
    constructor(title) { };

    /**
     * @description 信息输出
     * @param {*} content 输出内容
     */
    info(content) { };

    /**
     * @description 警告输出
     * @param {*} content 输出内容
     */
    warn(content) { };

    /**
     * @description 错误输出
     * @param {*} content 输出内容
     */
    error(content) { };

    /**
     * @description 调试输出
     * @param {*} content 输出内容
     */
    debug(content) { };
}

/**
 * @description WebSocket客户端
 */
class WebSocket {
    /**
     * @description WebSocket客户端
     * @param {String} uri ws地址
     * @param {String} namespace 当前命名空间
     */
    constructor(uri, namespace) { };

    /**
     * @description 开启事件
     */
    onopen = new Function();

    /**
     * @description 关闭事件
     */
    onclose = new Function();

    /**
     * @description 错误事件
     */
    onerror = new Function();

    /**
     * @description 消息接收事件
     */
    onmessage = new Function();

    /**
     * @description 连接
     */
    open() { };

    /**
     * @description 发送
     * @param {String} msg 消息内容
     */
    send(msg) { };

    /**
     * @description 关闭
     */
    close() { };

    /**
     * @description 释放对象（调用后释放对象的资源，且不可撤销）
     */
    dispose() { };

    /**
     * @description 连接状态
     * @enum -1=未知或无效;
     * @enum  0=正在连接;
     * @enum  1=连接成功;
     * @enum  2=正在关闭;
     * @enum  3=已关闭;
     */
    state = 0;
}
