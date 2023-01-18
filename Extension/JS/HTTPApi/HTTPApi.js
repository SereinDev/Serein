var HttpListener = importNamespace('System.Net').HttpListener;
var Encoding = importNamespace('System.Text').Encoding;
var IO = importNamespace('System.IO');
var File = IO.File;
var Directory = IO.Directory;

const logger = new Logger("HTTPApi");
serein.registerPlugin("HTTPApi", "1.0", "Zaitonn", "HTTP控制");

if (!HttpListener.IsSupported)
    throw new Error("当前系统不支持");

let listener = new HttpListener();
let config = {
    auth: [""],
    port: 2222
};


if (Directory.Exists('plugins/HTTPApi') && File.Exists('plugins/HTTPApi/config.json')) {
    try {
        config = JSON.parse(File.ReadAllText('plugins/HTTPApi/config.json'));
    } catch (e) {
        throw new Error(`读取配置文件时出现问题：${e}`);
    }
    if (config.auth.length > 0) {
        listener.Prefixes.Add(`http://127.0.0.1:${config.port}/serein/`);
        listener.Start();
        start();
    }
    else
        throw new Error("AuthKey为空，请修改后重试");
} else {
    Directory.CreateDirectory('plugins/HTTPApi');
    File.WriteAllText('plugins/HTTPApi/config.json', JSON.stringify(config, null, 4));
    logger.error("配置文件不存在，已重新创建");
    logger.error("请使用文本编辑器打开 plugins/HTTPApi/config.json 修改相应配置");
}

serein.setListener("onSereinClose", close);
serein.setListener("onPluginsReload", close);

/**
 * @description 关闭监听
 */
function close() {
    if (listener.IsListening)
        listener.Close();
    listener.Stop();
    listener.Dispose();
}

/**
 * @description 开始获取请求
 */
function start() {
    setTimeout(() => {
        while (listener.IsListening) {
            let context = listener.GetContext();
            context.Response.StatusCode = handleResponse(context.Request.RawUrl);
            context.Response.Close();
        }
    }, 100);
}

/**
 * @description 获取当前URL参数
 * @param {string} parameter 参数
 * @param {string|null} name 参数名
 * @returns 参数
 */
function getParameter(parameter, name) {
    let reg = new RegExp("(^|&)" + name + "=([^&]+?)(&|$)", "i");
    let r = parameter.match(reg);
    if (r != null)
        return decodeURIComponent(r[2]);
    return null;
}

/**
 * @description 处理回复
 * @param {string} url url
 * @returns 状态码
 * 100=已阅
 * 200=成功 
 * 400=无效操作
 * 401=验证失败
 * 406=执行失败
 */
function handleResponse(url) {
    url = url.substring(1);
    logger.info(`Recieve: ${url}`);
    if (url.lastIndexOf('/') > 0 || url.indexOf('?') < 0 || url.indexOf('?') != url.lastIndexOf('?')) // 拒绝二级目录的请求和无参数请求
        return 400;
    let parameter = url.split('?')[1];
    if (config.auth.indexOf(getParameter(parameter, "auth")) < 0) {
        logger.warn(`验证失败:${getParameter(parameter, "auth")}`);
        return 401;
    } else
        switch (getParameter(parameter, "operation")) {
            case "sendcmd":
                let command = getParameter(parameter, "command");
                if (!command || !serein.getServerStatus())
                    return 406;
                else {
                    serein.sendCmd(command);
                    return 200;
                }
            case "startserver":
                return serein.startServer() ? 200 : 406;
            case "stopserver":
                return serein.stopServer() ? 200 : 406;
            case "killserver":
                return serein.killServer() ? 200 : 406;
            case "runcommand":
                serein.runCommand(getParameter(parameter, "command"));
                return 200;
            case "sendprivate":
                if (getParameter(url, "target") &&
                    getParameter(url, "msg") &&
                    serein.sendPrivate(Number(getParameter(url, "targrt")), getParameter(url, "msg")))
                    return 200;
                else
                    return 406;
            case "sendgroup":
                if (getParameter(url, "target") &&
                    getParameter(url, "msg") &&
                    serein.sendGroup(Number(getParameter(url, "targrt")), getParameter(url, "msg")))
                    return 200;
                else
                    return 406;
            case "sendpacket":
                if (getParameter(url, "packet") && serein.sendPacket(getParameter(url, "packet")))
                    return 200;
                else
                    return 406;
            case "bindmember":
                if (getParameter(url, "userid") &&
                    getParameter(url, "gameid") &&
                    serein.bindMember(Number(getParameter(url, "userid")), getParameter(url, "gameid")))
                    return 200;
                else
                    return 406;
            case "unbindmember":
                if (
                    getParameter(url, "userid") &&
                    serein.unbindMember(Number(getParameter(url, "userid"))))
                    return 200;
                else
                    return 406;
            case null:
            case undefined:
                return 201;
            default:
                logger.warn("未知的操作");
                return 400;
        }
}
