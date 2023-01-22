using Newtonsoft.Json.Linq;
using Serein.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Serein.Base
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
                foreach (Member Item in Items)
                {
                    GameIDList.Add(Item.GameID);
                }
                return GameIDList;
            }
        }

        /// <summary>
        /// 只读的 Global.MemberDict Value副本
        /// </summary>
        private static List<Member> Items => Global.MemberDict.Values.ToList();

        /// <summary>
        /// 绑定（无群反馈）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="value">值</param>
        /// <returns>绑定结果</returns>
        public static bool Bind(long userId, string value)
        {
            value = value.Trim();
            if (Global.MemberDict.ContainsKey(userId) || !System.Text.RegularExpressions.Regex.IsMatch(value, @"^[a-zA-Z0-9_\s-]{4,16}$") || GameIDs.Contains(value))
            {
                return false;
            }
            else
            {
                Global.MemberDict.Add(userId, new Member
                {
                    ID = userId,
                    GameID = value
                });
                IO.SaveMember();
                return true;
            }
        }

        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="jsonObject">消息JSON对象</param>
        /// <param name="value">值</param>
        /// <param name="userId">用户ID</param>
        /// <param name="groupId">群聊ID</param>
        public static void Bind(JObject jsonObject, string value, long userId, long groupId = -1)
        {
            value = value.Trim();
            if (Global.MemberDict.ContainsKey(userId))
            {
                EventTrigger.Trigger(EventType.BindingFailDueToAlreadyBinded, groupId, userId);
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^[a-zA-Z0-9_\s-]{4,16}$"))
            {
                EventTrigger.Trigger(EventType.BindingFailDueToInvalid, groupId, userId);
            }
            else if (GameIDs.Contains(value))
            {
                EventTrigger.Trigger(EventType.BindingFailDueToOccupation, groupId, userId);
            }
            else
            {
                Member member = new()
                {
                    ID = userId,
                    Card = jsonObject["sender"]["card"].ToString(),
                    Nickname = jsonObject["sender"]["nickname"].ToString(),
                    Role = Array.IndexOf(Command.Roles, jsonObject["sender"]["role"].ToString()),
                    GameID = value
                };
                lock (Global.MemberDict)
                {
                    Global.MemberDict.Add(userId, member);
                }
                IO.SaveMember();
                EventTrigger.Trigger(EventType.BindingSucceed, groupId, userId);
            }
        }

        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="groupId">群聊ID</param>
        public static void UnBind(long userId, long groupId)
        {
            lock (Global.MemberDict)
            {
                if (Global.MemberDict.Remove(userId))
                {
                    IO.SaveMember();
                    EventTrigger.Trigger(EventType.UnbindingSucceed, groupId, userId);
                }
                else
                {
                    EventTrigger.Trigger(EventType.UnbindingFail, groupId, userId);
                }
            }
        }

        /// <summary>
        /// 解绑ID（无群反馈）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>解绑结果</returns>
        public static bool UnBind(long userId)
        {
            lock (Global.MemberDict)
            {
                if (Global.MemberDict.Remove(userId))
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
        /// <param name="userId">用户ID</param>
        /// <returns>对应的游戏ID</returns>
        public static string GetGameID(long userId)
            => Global.MemberDict.TryGetValue(userId, out Member member) ? member.GameID : string.Empty;

        /// <summary>
        /// 获取指定游戏ID对应的用户ID
        /// </summary>
        /// <param name="gameId">游戏ID</param>
        /// <returns>对应的用户ID</returns>
        public static long GetID(string gameId)
        {
            gameId = gameId.Trim();
            if (!GameIDs.Contains(gameId))
            {
                return 0;
            }
            else
            {
                foreach (Member member in Items)
                {
                    if (member.GameID == gameId)
                    {
                        return member.ID;
                    }
                }
                return 0;
            }
        }

        /// <summary>
        /// 更新群成员信息
        /// </summary>
        /// <param name="jsonObject">消息JSON对象</param>
        /// <param name="userId">用户ID</param>
        public static void Update(JObject jsonObject, long userId)
        {
            if (Global.MemberDict.ContainsKey(userId))
            {
                Logger.Output(LogType.Debug, jsonObject["sender"]);
                lock (Global.MemberDict)
                {
                    Global.MemberDict[userId].Nickname = jsonObject["sender"]["nickname"].ToString();
                    Global.MemberDict[userId].Role = Array.IndexOf(Command.Roles, jsonObject["sender"]["role"].ToString());
                    Global.MemberDict[userId].Card = jsonObject["sender"]["card"].ToString();
                }
            }
        }
    }
}
