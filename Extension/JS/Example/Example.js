// 注册插件，注册成功后会在插件栏显示具体内容
// 若不注册则只显示插件名
serein.registerPlugin("示例文件", "v1.1", "Zaitonn", "示例文件");

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
serein.registerCommand("example", example);
function example(cmd) {
    serein.log("你输入了注册的命令：" + cmd);
}

/**
 * 注册监听器
 * https://serein.cc/Javascript.html#%E8%AE%BE%E7%BD%AE%E7%9B%91%E5%90%AC%E5%99%A8
 *
 * 返回：Boolean
 * 
 * 以下情况无法触发
 *  处理函数参数不正确
*/
serein.setListener("onServerOutput", onServerOutput)
function onServerOutput(line) {
    serein.log("服务器控制台输出：" + line);
}

serein.setListener("onSereinStart", onSereinStart);
function onSereinStart() {
    serein.log("Serein启动");
}

serein.setListener("onSereinClose", onSereinClose);
function onSereinClose() {
    serein.log("Serein关闭");
}

serein.setListener("onServerStart", onServerStart);
function onServerStart() {
    serein.log("服务器启动");
}

serein.setListener("onServerStop", onServerStop);
function onServerStop() {
    serein.log("服务器关闭");
}

serein.setListener("onServerSendCommand", onServerSendCommand);
function onServerSendCommand(command) {
    serein.log("你输入了指令：" + command);
}

serein.setListener("onGroupIncrease", onGroupIncrease);
function onGroupIncrease(group, user) {
    serein.log("监听群群成员增加 触发群：" + group + "  QQ：" + user);
}

serein.setListener("onGroupDecrease", onGroupDecrease);
function onGroupDecrease(group, user) {
    serein.log("监听群群成员减少 触发群：" + group + "  QQ：" + user);
}

serein.setListener("onGroupPoke", onGroupPoke);
function onGroupPoke(group, user) {
    serein.log("监听群群成员戳一戳当前账号 触发群：" + group + "  QQ：" + user);
}

serein.setListener("onReceiveGroupMessage", onReceiveGroupMessage);
function onReceiveGroupMessage(group, user, msg, shownname) {
    serein.log("监听群聊消息 " + group + "  " + shownname + "（" + user + "）：" + msg);
}

serein.setListener("onReceivePrivateMessage", onReceivePrivateMessage);
function onReceivePrivateMessage(user, msg, nickname) {
    serein.log("监听私聊消息 " + nickname + "（" + user + "）：" + msg);
}

serein.setListener("onReceivePacket", onReceivePacket);
function onReceivePacket(json) {
    serein.log("收到数据包：" + json)
}


/**
 * 发送数据包
 * 数据包格式参考：
 *  - go-cqhttp https://docs.go-cqhttp.org/api
 *  - 其他类型机器人 https://github.com/botuniverse/onebot-11/blob/master/api/public.md
 * 
 * 返回：Boolean
 *
 * Tips：发送复杂数据包时建议构造字典后生成JSON
*/
serein.sendPacket("{\"action\": \"send_private_msg\",\"params\": {\"user_id\": \"10001\",\"message\": \"111\"}}")


/**
 * --------------------------分割线--------------------------
 * 
 * 以下为进阶教程
 * 可能需要一些js基础和C#基础
 * 
 * 由于JS引擎（Jint https://github.com/sebastienros/jint）的特性，你可以在js中插入NET对象/类
*/

// 导入Encoding类
var Encoding = importNamespace("System.Text").Encoding;
var UTF8Encoding = importNamespace("System.Text").UTF8Encoding;
// Encoding.UTF8 → UTF8-BOM
// new UTF8Encoding(false) → UTF8
// 
// Tips：使用UTF8-BOM也没啥大不了，只要读写的编码一致即可

// 导入SearchOption类
// https://docs.microsoft.com/zh-cn/dotnet/api/system.io.searchoption?view=net-6.0
var SearchOption = importNamespace("System.IO").SearchOption;
// SearchOption.AllDirectories → 在搜索操作中包括当前目录和所有它的子目录。 此选项在搜索中包括重解析点，比如安装的驱动器和符号链接
// SearchOption.TopDirectoryOnly → 仅在搜索操作中包括当前目录

// 导入File类
// https://docs.microsoft.com/zh-cn/dotnet/api/system.io.file?view=net-6.0
var File = importNamespace("System.IO").File;

// 创建一个新文件，向其中写入内容，然后关闭文件。 如果目标文件已存在，则覆盖该文件
File.WriteAllText(
    "1.txt", // 路径
    "一些覆盖的文本", // 文本
    Encoding.UTF8 // 编码（可省略，默认为Encoding.UTF8）
);

// 将指定的字符串追加到文件中，如果文件还不存在则创建该文件
File.AppendAllText(
    "1.txt", // 路径
    "一些追加的文本",
    Encoding.UTF8 // 编码
);

// 打开一个文本文件，将文件中的所有文本读取到一个字符串中，然后关闭此文件
// 返回：String
var fileReadAllText = File.ReadAllText(
    "1.txt", // 路径
    Encoding.UTF8 // 编码（可省略，默认为Encoding.UTF8）
);

// 确定指定的文件是否存在
// 返回：Boolean
var fileExists = File.Exists("1.txt");

// 将现有文件复制到新文件
// File.Copy("1.txt","2.txt");
File.Copy("1.txt", "2.txt", true); // 允许覆盖同名文件

// 删除指定的文件
File.Delete("2.txt");

// 将指定文件移到新位置，提供要指定新文件名的选项
File.Move("1.txt", "2.txt");


// 导入Directory类
// https://docs.microsoft.com/zh-cn/dotnet/api/system.io.directory?view=net-6.0
var Directory = importNamespace("System.IO").Directory;

// 在指定路径中创建所有目录
Directory.CreateDirectory("./path");

// 确定给定路径是否引用磁盘上的现有目录
// 返回：Boolean
var directoryExists = Directory.Exists("./path");

// 将文件或目录及其内容移到新位置
Directory.Move("./path", "./path2");

// 从指定路径删除空目录
Directory.Delete("./path2");

// 返回指定目录中文件的名称（包括其路径）
// 返回：String[]
var files = Directory.GetFiles("./");
var files = Directory.GetFiles("./", "*.js"); // 指定类型
var files = Directory.GetFiles("./", SearchOption.TopDirectoryOnly); // 仅搜索当前目录，不包括子目录

// 返回满足指定条件的子目录的名称
// 返回：String[]
var directories = Directory.GetDirectories("./"); // 重载同上

// 检索指定路径的父目录，包括绝对路径和相对路径
// 返回：String
var parentDirectory = Directory.GetParent("./");


// 导入Process类
// https://docs.microsoft.com/zh-cn/dotnet/api/system.diagnostics.process?view=net-6.0
var Process = importNamespace("System.Diagnostics").Process;

// 启动进程资源并将其与 Process 组件关联。
// Process.Start("Explorer.exe");
Process.Start("cmd.exe");

// Process.Start("https://github.com/Zaitonn/Serein");
// 不推荐这种方式打开网页，因为在NET 6下使用这种方法会导致Serein崩溃，替代方法见下文(Line 248)

// 创建新的 Process 组件，并将其与您指定的现有进程资源关联
// var process = Process.GetProcessById(1);

// 强制终止基础进程。
// process.kill()；
// process.kill(true); // 终止进程同时终止子进程

// Process对象属性
// https://docs.microsoft.com/zh-cn/dotnet/api/system.diagnostics.process?view=net-6.0#properties
// 由于属性过多，此处仅举出几个例子，其余属性请戳上方链接查看

// var hasExited = process.HasExited; // 获取指示关联进程是否已终止的值(Boolean)
// var pid = process.Id // 获取关联进程的唯一标识符(Number)

// 导入ProcessStartInfo类
// https://docs.microsoft.com/zh-cn/dotnet/api/system.diagnostics.process?view=net-6.0
var ProcessStartInfo = importNamespace("System.Diagnostics").ProcessStartInfo;

var psi = new ProcessStartInfo(
    "notepad.exe",
    "" // 启动参数（可为空）
);

// ProcessStartInfo
// https://docs.microsoft.com/zh-cn/dotnet/api/system.diagnostics.processstartinfo.argumentlist?view=net-6.0
// 由于属性过多，此处仅举出几个例子，其余属性请戳上方链接查看

psi.UseShellExecute = false; // 获取或设置指示是否使用操作系统 shell 启动进程的值
// 若使用psi启动务必将UseShellExecute设置为false，否则容易出现一些奇奇怪怪的问题
psi.CreateNoWindow = false; // 获取或设置指示是否在新窗口中启动该进程的值
psi.WorkingDirectory = serein.path // 获取或设置要启动的进程的工作目录

// 提供指定启动进程时使用的一组值启动进程
Process.Start(psi);


// 导入WebRequest类
// https://docs.microsoft.com/zh-cn/dotnet/api/system.net.webrequest?view=net-6.0
var WebRequest = importNamespace("System.Net").WebRequest;

// 导入StreamReader类
// https://docs.microsoft.com/zh-cn/dotnet/api/system.io.streamreader?view=net-6.0
var StreamReader = importNamespace("System.IO").StreamReader;

var request = WebRequest.Create("https://v1.hitokoto.cn"); // 创建网络请求
request.Method = "GET"; // 设置要在此请求中使用的协议方法

var response = new StreamReader(
    request
        .GetResponse() // 获取响应
        .GetResponseStream() // 获取响应的流
).ReadToEnd(); // 读取响应流

// 输出响应数据到控制台
serein.log(response);