/*!
 * @Author       : Maraudern
 * @Date         : 2023-01-16 20:19:47
 * @LastEditors  : Maraudern
 * @LastEditTime : 2023-02-03 14:23:10
 * @FilePath     : \Serein\BetterWhitelist.js
 * @Description  : 更好的白名单
 */
var betterWhiteList = {
	name: "更好的白名单",
	version: "v1.3",
	author: "9Yan",
	description: "更完善的白名单方案，基于Serein成员管理，需关闭白名单相关正则",
};
serein.registerPlugin(betterWhiteList.name, betterWhiteList.version, betterWhiteList.author, betterWhiteList.description);

const File = importNamespace("System.IO").File;
const Directory = importNamespace("System.IO").Directory;
const Encoding = importNamespace("System.Text").Encoding;

const PluginPath = "plugins/BetterWhiteList/config.json"; // 插件配置
const MemberPath = "data/members.json"; // 成员管理
var whiteListPath; // 白名单

const isXboxID = /^[A-Za-z]{1}[0-9A-Za-z ]{4,14}$/; // 匹配 XboxID
const isQQNumber = /^[0-9]{5,11}$/; // 匹配 QQ号
const isCqAt = /^\[CQ:at,qq=(\d+)\]$/; // 匹配 CQ:at
const isEnterServer = /^.*?Player\sSpawned:\s(.*?)\sxuid:.*$/; // 匹配 进入服务器信息
const isPath = /([^<>/\\\|:""\*\?]+)\.\w+$/; // 匹配 路径文件名

var config, members, whiteList;

// 确定配置文件夹是否存在
if (!Directory.Exists("plugins/BetterWhiteList")) {
	Directory.CreateDirectory("plugins/BetterWhiteList");
}
// 确定配置文件是否存在
if (!File.Exists(PluginPath)) {
	writeConfig();
	serein.log("成功初始化配置文件");
} else {
	config = JSON.parse(File.ReadAllText(PluginPath));
	serein.log("成功加载配置文件");
}
// 确认版本是否正确
if (config.version != betterWhiteList.version) {
	writeConfig();
	serein.log("成功更新插件，请重新配置插件");
}

function writeConfig() {
	config = {
		version: betterWhiteList.version,
		ignoreGroup: [],
		"1//": "排除监听指定群聊（使用“,”分隔）",
		bindSelf: true,
		"2//": "是否开启(true/false) 群成员使用绑定命令的权限",
		syncWhiteList: true,
		"3//": "是否开启(true/false) 同步成员管理与服务器白名单",
		memberWhiteList: false,
		"4//": "是否开启(true/false) 将成员管理设为服务器白名单（使用llbds且关闭bds内置白名单可开启）",
		exitGroup: true,
		"5//": "是否开启(true/false) 退群后自动删除该成员管理的数据",
		editCard: true,
		"6//": "是否开启(true/false) 绑定后自动修改群名片为 XboxID （需管理员权限）",
	};
	File.WriteAllText(PluginPath, JSON.stringify(config, null, 4));
	return;
}

/**
 * @description: 判断是否在面板管理权限列表
 * @param {Number} userID QQ号
 * @return {Boolean} 是为true，否为false
 */
function isPermission(userID) {
	return Boolean(JSON.parse(serein.getSettings()).Bot.PermissionList.indexOf(userID) + 1);
}

/**
 * @description: 判断是否在面板监听群列表
 * @param {Number} groupID QQ群号
 * @return {Boolean} 是为true，否为false
 */
function isListenerGroup(groupID) {
	return Boolean(JSON.parse(serein.getSettings()).Bot.GroupList.indexOf(groupID) + 1);
}

/**
 * @description: 判断是否在排除监听群列表
 * @param {Number} groupID QQ群号
 * @return {Boolean} 是为true，否为false
 */
function isIgnoreGroup(groupID) {
	return Boolean(config.ignoreGroup.indexOf(groupID) + 1);
}

/**
 * @description: 判断是否在成员管理列表
 * @param {String} xboxID 游戏ID
 * @return {Number} 是为数组下标，否为-1
 */
function isMember(xboxID) {
	members = JSON.parse(File.ReadAllText(MemberPath, Encoding.UTF8));
	let data = -1;
	for (let i = 0; i < members.data.length; i++) {
		if (xboxID === members.data[i].GameID) {
			data = i;
			break;
		}
	}
	return data;
}

/**
 * @description: 添加白名单
 * @param {Number} groupID QQ群号
 * @param {Number} userID QQ号
 * @param {String} xboxID 游戏ID
 * @return {Boolean} 成功为true，否则为false
 */
function whiteListAdd(groupID, userID, xboxID) {
	if (config.syncWhiteList) serein.sendCmd(`whitelist add "${xboxID}"`);
	if (config.editCard) editCard(groupID, userID, xboxID);
	return serein.bindMember(userID, xboxID);
}

/**
 * @description: 删除白名单
 * @param {Number} userID QQ号
 * @param {String} xboxID 游戏ID
 * @return {Boolean} 成功为true，否则为false
 */
function whiteListRemove(userID, xboxID) {
	if (config.syncWhiteList) serein.sendCmd(`whitelist remove "${xboxID}"`);
	return serein.unbindMember(userID); // 1.3.3 版本 unbindMember 返回值有误
}

/**
 * @description: 修改群昵称
 * @param {Number} groupID QQ群号
 * @param {Number} userID QQ号
 * @param {String} card 群昵称
 * @return {*}
 */
function editCard(groupID, userID, card) {
	serein.sendPacket(
		JSON.stringify({
			action: "set_group_card",
			params: {
				group_id: groupID,
				user_id: userID,
				card: card,
			},
		})
	);
}

/**
 * @description: 同步白名单
 * @return {*}
 */
function syncWhiteList(groupID) {
	if (!config.syncWhiteList) return;
	serein.sendGroup(groupID, "正在同步白名单...");

	var errorWhiteList = [];
	var errorNumber = [];

	whiteListPath = JSON.parse(serein.getSettings()).Server.Path.replace(isPath, "allowlist.json");
	if (!File.Exists(whiteListPath)) whiteListPath = whiteListPath.replace(isPath, "whitelist.json");
	whiteList = JSON.parse(File.ReadAllText(whiteListPath));
	members = JSON.parse(File.ReadAllText(MemberPath, Encoding.UTF8));

	var oldIds = whiteList.map((item) => item.name);
	var newIds = members.data.filter((item) => !oldIds.includes(item.GameID));
	newIds.forEach((item) => {
		errorWhiteList.push(item.GameID);
		serein.sendCmd(`whitelist add "${item.GameID}"`);
	});
	if (errorWhiteList.length) {
		let str = errorWhiteList.join(",");
		serein.sendGroup(groupID, "添加白名单：\n" + str);
	}

	var oldIds = members.data.map((item) => item.GameID);
	var newIds = whiteList.filter((item) => !oldIds.includes(item.name));
	newIds.forEach((item) => {
		errorNumber.push(item.name);
		serein.sendCmd(`whitelist remove "${item.name}"`);
	});
	if (errorNumber.length) {
		let str = errorNumber.join(",");
		serein.sendGroup(groupID, "删除白名单：\n" + str);
	}

	if (!errorWhiteList.length && !errorNumber.length) serein.sendGroup(groupID, "没有需要同步的白名单");

	return;
}

serein.setListener("onServerStart", () => {
	for (let i = 0; i < JSON.parse(serein.getSettings()).Bot.GroupList.length; i++) {
		if (!isIgnoreGroup(JSON.parse(serein.getSettings()).Bot.GroupList[i])) {
			var groupID = JSON.parse(serein.getSettings()).Bot.GroupList[i];
			break;
		}
	}
	syncWhiteList(groupID);
});

serein.setListener("onReceiveGroupMessage", (groupID, userID, msg, shownName) => {
	if (!isListenerGroup(groupID) || isIgnoreGroup(groupID)) return;

	let command = msg.split(" ").filter((item) => item && item.trim());
	let keyWord = command[0];
	command.splice(0, 1);

	switch (keyWord.toLowerCase()) {
		case "添加白名单":
		case "wladd":
		case "whitelistadd":
			// 判断是否有管理员权限
			if (!isPermission(userID)) {
				serein.sendGroup(groupID, "您没有使用<" + keyWord + ">的权限！");
				return;
			}

			// 判断分割数组是否有值
			if (!command.length) {
				serein.sendGroup(groupID, "语法错误，请发送：\n" + keyWord + " <QQ号>(@成员) <XboxID>");
				return;
			}

			// 判断是否为 QQ 号
			var qqNumber = command[0].replace(isCqAt, "$1");
			command.splice(0, 1);
			if (!isQQNumber.test(qqNumber)) {
				serein.sendGroup(groupID, "意外的：>>" + qqNumber + "<< \n应当为：<QQ号>(@成员)");
				return;
			}

			// 判断 text 是否符合 xboxID 规范
			var text = command.join(" ");
			if (!isXboxID.test(text)) {
				serein.sendGroup(groupID, "意外的：>>" + text + "<<\n应当为：<XboxID>");
				return;
			}

			// 判断成员管理中是否存在相同 xboxID
			var index = isMember(text);
			if (index + 1) {
				serein.sendGroup(groupID, `绑定失败，存在相同 XboxID\n${text}（${members.data[index].ID}）`);
				return;
			}

			// 判断 该玩家是否存在绑定 xboxID
			var xboxID = serein.getGameID(qqNumber);
			if (xboxID) {
				whiteListRemove(qqNumber, xboxID);
				if (whiteListAdd(groupID, qqNumber, text)) {
					serein.sendGroup(groupID, `已存在数据：\n${xboxID}(${qqNumber})\n成功修改为：\n${text}(${qqNumber})`);
				}
				return;
			}

			if (whiteListAdd(groupID, qqNumber, text)) {
				serein.sendGroup(groupID, `绑定成功：${text}（${qqNumber}）`);
			}
			return;

		case "删除白名单":
		case "wldel":
		case "whitelistdelete":
			// 判断是否有管理员权限
			if (!isPermission(userID)) {
				serein.sendGroup(groupID, "您没有使用<" + keyWord + ">的权限！");
				return;
			}

			// 判断分割数组是否有值
			if (!command.length) {
				serein.sendGroup(groupID, "语法错误，请发送：\n" + keyWord + " <QQ号>(@成员) <XboxID>");
				return;
			}

			// 判断是否为 QQ 号
			var qqNumber = command[0].replace(isCqAt, "$1");
			if (!isQQNumber.test(qqNumber)) {
				serein.sendGroup(groupID, "意外的：>>" + qqNumber + "<< \n应当为：<QQ号>(@成员)");
				return;
			}

			// 判断 该玩家是否存在绑定 xboxID
			var xboxID = serein.getGameID(qqNumber);
			if (!xboxID) {
				serein.sendGroup(groupID, "没有绑定白名单！");
				return;
			}

			whiteListRemove(qqNumber, xboxID);
			//if (whiteListRemove(qqNumber, xboxID))
			serein.sendGroup(groupID, `成功删除：${xboxID}（${qqNumber}）`);
			//}
			return;

		case "白名单列表":
		case "wllist":
		case "whitelist":
			// 判断是否有管理员权限
			if (!isPermission(userID)) {
				serein.sendGroup(groupID, "您没有使用<" + keyWord + ">的权限！");
				return;
			}

			whiteListPath = JSON.parse(serein.getSettings()).Server.Path.replace(isPath, "allowlist.json");
			if (!File.Exists(whiteListPath)) whiteListPath = whiteListPath.replace(isPath, "whitelist.json");
			whiteList = JSON.parse(File.ReadAllText(whiteListPath));
			members = JSON.parse(File.ReadAllText(MemberPath, Encoding.UTF8));

			Array = [];
			for (let i = 0; i < members.data.length; i++) {
				let isCorrect = "❗";
				for (let j = 0; j < whiteList.length; j++) {
					if (members.data[i].GameID === whiteList[j].name) isCorrect = "✔";
				}
				let isName = members.data[i].Card ? members.data[i].Card : members.data[i].Nickname ? members.data[i].Nickname : members.data[i].ID;
				Array.push({
					type: "node",
					data: {
						name: "『" + i + "』" + isName,
						uin: members.data[i].ID,
						content: "成员管理数据：\n" + members.data[i].GameID + "(" + members.data[i].ID + ")\n服务器白名单：" + isCorrect,
					},
				});
			}

			//发送合并消息数据包
			while (Array.length > 90) {
				serein.sendPacket(
					'{"action": "send_group_forward_msg","params": {"group_id": "' + groupID + '","messages": ' + JSON.stringify(Array.splice(0, 90)) + "}}"
				);
			}
			serein.sendPacket('{"action": "send_group_forward_msg","params": {"group_id": "' + groupID + '","messages": ' + JSON.stringify(Array) + "}}");
			return;

		case "同步白名单":
		case "syncwl":
		case "syncwhitelist":
			// 判断是否有管理员权限
			if (!isPermission(userID)) {
				serein.sendGroup(groupID, "您没有使用<" + keyWord + ">的权限！");
				return;
			}
			syncWhiteList(groupID);
			return;

		case "绑定":
		case "bind":
			// 判断是否有管理员权限 或 配置是否开启 bindSelf
			if (!isPermission(userID) && !config.bindSelf) {
				serein.sendGroup(groupID, "您没有使用<" + keyWord + ">的权限！");
				return;
			}

			// 判断分割数组是否有值
			if (!command.length) {
				serein.sendGroup(groupID, "语法错误，请发送：\n" + keyWord + " <XboxID>");
				return;
			}

			// 判断 text 是否符合 xboxID 规范
			var text = command.join(" ");
			if (!isXboxID.test(text)) {
				serein.sendGroup(groupID, "意外的：>>" + text + "<<\n应当为：<XboxID>");
				return;
			}

			// 判断成员管理中是否存在相同 xboxID
			var index = isMember(text);
			if (index + 1) {
				serein.sendGroup(groupID, `绑定失败，存在相同 XboxID\n${text}（${members.data[index].ID}）`);
				return;
			}

			// 判断 该玩家是否存在绑定 xboxID
			var xboxID = serein.getGameID(userID);
			if (xboxID) {
				whiteListRemove(userID, xboxID);
				if (whiteListAdd(groupID, userID, text)) {
					serein.sendGroup(groupID, `已存在数据：\n${xboxID}(${userID})\n成功修改为：\n${text}(${userID})`);
				}
				return;
			}

			if (whiteListAdd(groupID, userID, text)) {
				serein.sendGroup(groupID, `绑定成功：${text}（${userID}）`);
			}
			return;

		case "解绑":
		case "unbind":
			// 判断是否有管理员权限 或 配置是否开启 bindSelf
			if (!isPermission(userID) && !config.bindSelf) {
				serein.sendGroup(groupID, "您没有使用<" + keyWord + ">的权限！");
				return;
			}

			// 判断 该玩家是否存在绑定 xboxID
			var xboxID = serein.getGameID(userID);
			if (!xboxID) {
				serein.sendGroup(groupID, "您没有绑定白名单！");
				return;
			}

			whiteListRemove(userID, xboxID);
			//if (whiteListRemove(userID, xboxID))
			serein.sendGroup(groupID, `成功解绑：${xboxID}（${userID}）`);
			//}
			return;
			break;
	}
});

serein.setListener("onGroupDecrease", (groupID, userID) => {
	if (!config.exitGroup || !isListenerGroup(groupID) || isIgnoreGroup(groupID)) return;

	serein.sendGroup(groupID, "群成员 " + userID + " 退群了，尝试删除白名单");

	// 判断 该玩家是否存在绑定 xboxID
	var xboxID = serein.getGameID(userID);
	if (!xboxID) {
		serein.sendGroup(groupID, "群成员" + userID + "没有绑定白名单！");
		return;
	}

	whiteListRemove(userID, xboxID);
	//if (whiteListRemove(qqNumber, xboxID))
	serein.sendGroup(groupID, `成功删除：${xboxID}（${userID}）`);
	//}
	return;
});

serein.setListener("onServerOutput", (text) => {
	if (!config.memberWhiteList || !isEnterServer.test(text)) return;

	let xboxID = text.replace(isEnterServer, "$1");
	if (isMember(xboxID) + 1) return;

	for (let i = 0; i < JSON.parse(serein.getSettings()).Bot.PermissionList.length; i++) {
		if (!isIgnoreGroup(JSON.parse(serein.getSettings()).Bot.PermissionList[i])) {
			var PermissionID = JSON.parse(serein.getSettings()).Bot.PermissionList[i];
			break;
		}
	}

	serein.sendPrivate(PermissionID, "玩家 " + xboxID + " 没有白名单，尝试进入");
	setTimeout(() => {
		serein.sendCmd('kick "' + xboxID + '" You do not have a whitelist!');
	}, 1000);
});
