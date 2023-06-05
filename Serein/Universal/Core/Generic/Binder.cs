﻿using Newtonsoft.Json.Linq;
using Serein.Base;
using Serein.Core.Server;
using Serein.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Serein.Core.Generic
{
    internal static class Binder
    {
        /// <summary>
        /// 游戏ID集合
        /// </summary>
        public static List<string> GameIDs
        {
            get
            {
                List<string> GameIDList = new();
                foreach (Member Item in _items)
                {
                    GameIDList.Add(Item.GameID);
                }
                return GameIDList;
            }
        }

        /// <summary>
        /// 只读的 Global.MemberDict Value副本
        /// </summary>
        private static List<Member> _items => Global.MemberDict.Values.ToList();

        /// <summary>
        /// 绑定（无群反馈）
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="value">值</param>
        /// <returns>绑定结果</returns>
        public static bool Bind(long userID, string value)
        {
            value = value.Trim();
            if (Global.MemberDict.ContainsKey(userID) || !System.Text.RegularExpressions.Regex.IsMatch(value, @"^[a-zA-Z0-9_\s-]{4,16}$") || GameIDs.Contains(value))
            {
                return false;
            }
            lock (Global.MemberDict)
            {
                Global.MemberDict.Add(userID, new Member
                {
                    ID = userID,
                    GameID = value
                });
            }
            IO.SaveMember();
            return true;
        }

        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="jsonObject">消息JSON对象</param>
        /// <param name="value">值</param>
        /// <param name="userID">用户ID</param>
        /// <param name="groupID">群聊ID</param>
        public static void Bind(JObject jsonObject, string value, long userID, long groupID)
        {
            value = value.Trim();
            if (Global.MemberDict.ContainsKey(userID))
            {
                EventTrigger.Trigger(EventType.BindingFailDueToAlreadyBinded, groupID, userID, jsonObject);
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(value, Global.Settings.Serein.Function.RegexForCheckingGameID))
            {
                EventTrigger.Trigger(EventType.BindingFailDueToInvalid, groupID, userID, jsonObject);
                return;
            }
            if (Global.Settings.Serein.Function.DisableBinderWhenServerClosed ? !ServerManager.Status : false)
            {
                EventTrigger.Trigger(EventType.BinderDisable, groupID, userID, jsonObject);
                return;
            }
            if (GameIDs.Contains(value))
            {
                EventTrigger.Trigger(EventType.BindingFailDueToOccupation, groupID, userID, jsonObject);
                return;
            }
            lock (Global.MemberDict)
            {
                Global.MemberDict.Add(userID, new()
                {
                    ID = userID,
                    Card = jsonObject["sender"]["card"].ToString(),
                    Nickname = jsonObject["sender"]["nickname"].ToString(),
                    Role = Array.IndexOf(Command.Roles, jsonObject["sender"]["role"].ToString()),
                    GameID = value
                });
            }
            IO.SaveMember();
            EventTrigger.Trigger(EventType.BindingSucceed, groupID, userID);
        }

        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="jsonObject">消息JSON对象</param>
        /// <param name="userID">用户ID</param>
        /// <param name="groupID">群聊ID</param>
        public static void UnBind(JObject jsonObject, long userID, long groupID)
        {
            if (Global.Settings.Serein.Function.DisableBinderWhenServerClosed ? !ServerManager.Status : false)
            {
                EventTrigger.Trigger(EventType.BinderDisable, groupID, userID, jsonObject);
                return;
            }
            lock (Global.MemberDict)
            {
                if (Global.MemberDict.Remove(userID))
                {
                    IO.SaveMember();
                    EventTrigger.Trigger(EventType.UnbindingSucceed, groupID, userID, jsonObject);
                }
                else
                {
                    EventTrigger.Trigger(EventType.UnbindingFail, groupID, userID, jsonObject);
                }
            }
        }

        /// <summary>
        /// 解绑ID（无群反馈）
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns>解绑结果</returns>
        public static bool UnBind(long userID)
        {
            lock (Global.MemberDict)
            {
                if (Global.MemberDict.Remove(userID))
                {
                    IO.SaveMember();
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 获取指定用户ID对应的游戏ID
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns>对应的游戏ID</returns>
        public static string GetGameID(long userID)
            => Global.MemberDict.TryGetValue(userID, out Member member) ? member.GameID : string.Empty;

        /// <summary>
        /// 获取指定游戏ID对应的用户ID
        /// </summary>
        /// <param name="gameId">游戏ID</param>
        /// <returns>对应的用户ID</returns>
        public static long GetID(string gameId)
        {
            gameId = gameId.Trim();
            foreach (Member member in _items)
            {
                if (member.GameID == gameId)
                {
                    return member.ID;
                }
            }
            return 0;
        }

        /// <summary>
        /// 更新群成员信息
        /// </summary>
        /// <param name="jsonObject">消息JSON对象</param>
        /// <param name="userID">用户ID</param>
        public static void Update(JObject jsonObject, long userID)
        {
            if (Global.MemberDict.TryGetValue(userID, out Member member))
            {
                Logger.Output(LogType.Debug, jsonObject["sender"]);
                member.Nickname = jsonObject["sender"]["nickname"].ToString();
                member.Role = Array.IndexOf(Command.Roles, jsonObject["sender"]["role"].ToString());
                member.Card = jsonObject["sender"]["card"].ToString();
            }
        }
    }
}
