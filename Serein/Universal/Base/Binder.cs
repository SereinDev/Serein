using Newtonsoft.Json.Linq;
using Serein.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Serein.Base
{
    internal class Binder
    {
        /// <summary>
        /// 游戏ID集合
        /// </summary>
        public static List<string> GameIDs
        {
            get
            {
                List<string> GameIDList = new List<string>();
                foreach (Member Item in Items)
                {
                    GameIDList.Add(Item.GameID);
                }
                return GameIDList;
            }
        }

        /// <summary>
        /// 只读的 Global.MemberItems 副本
        /// </summary>
        private static List<Member> Items
        {
            get
            {
                return Global.MemberItems.Values.ToList();
            }
        }

        /// <summary>
        /// 绑定（无群反馈）
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <param name="Value">值</param>
        /// <returns>绑定结果</returns>
        public static bool Bind(long UserId, string Value)
        {
            if (Global.MemberItems.Keys.Contains(UserId) || !System.Text.RegularExpressions.Regex.IsMatch(Value, @"^[a-zA-Z0-9_\s-]{4,16}$") || GameIDs.Contains(Value))
            {
                return false;
            }
            else
            {
                Global.MemberItems.Add(UserId, new Member()
                {
                    ID = UserId,
                    GameID = Value
                });
                IO.SaveMember();
                return true;
            }
        }

        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="JsonObject">消息JSON对象</param>
        /// <param name="Value">值</param>
        /// <param name="UserId">用户ID</param>
        /// <param name="GroupId">群聊ID</param>
        public static void Bind(JObject JsonObject, string Value, long UserId, long GroupId = -1)
        {
            if (Global.MemberItems.Keys.Contains(UserId))
            {
                EventTrigger.Trigger("Bind_Already", GroupId, UserId);
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(Value, @"^[a-zA-Z0-9_\s-]{4,16}$"))
            {
                EventTrigger.Trigger("Bind_Invalid", GroupId, UserId);
            }
            else if (GameIDs.Contains(Value.Trim()))
            {
                EventTrigger.Trigger("Bind_Occupied", GroupId, UserId);
            }
            else
            {
                Member Item = new Member()
                {
                    ID = UserId,
                    Card = JsonObject["sender"]["card"].ToString(),
                    Nickname = JsonObject["sender"]["nickname"].ToString(),
                    Role = Array.IndexOf(Command.Roles, JsonObject["sender"]["role"].ToString()),
                    GameID = Value.Trim()
                };
                Dictionary<long, Member> Items = Global.MemberItems;
                Items.Add(UserId, Item);
                Global.UpdateMemberItems(Items);
                IO.SaveMember();
                EventTrigger.Trigger("Bind_Success", GroupId, UserId);
            }
        }

        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <param name="GroupId">群聊ID</param>
        public static void UnBind(long UserId, long GroupId = -1)
        {
            if (!Global.MemberItems.Keys.Contains(UserId))
            {
                EventTrigger.Trigger("Unbind_Failure", GroupId, UserId);
            }
            else
            {
                lock (Global.MemberItems)
                {
                    Global.MemberItems.Remove(UserId);
                    IO.SaveMember();
                    EventTrigger.Trigger("Unbind_Success", GroupId, UserId);
                }
            }
        }

        /// <summary>
        /// 解绑ID（无群反馈）
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns>解绑结果</returns>
        public static bool UnBind(long UserId)
        {
            if (Global.MemberItems.Keys.Contains(UserId))
            {
                lock (Global.MemberItems)
                {
                    Global.MemberItems.Remove(UserId);
                    IO.SaveMember();
                }
            }
            return Global.MemberItems.Keys.Contains(UserId);
        }

        /// <summary>
        /// 获取指定用户ID对应的游戏ID
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns>对应的游戏ID</returns>
        public static string GetGameID(long UserId)
        {
            if (!Global.MemberItems.Keys.Contains(UserId))
            {
                return string.Empty;
            }
            else
            {
                foreach (Member Item in Items)
                {
                    if (Item.ID == UserId)
                    {
                        return Item.GameID;
                    }
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取指定游戏ID对应的用户ID
        /// </summary>
        /// <param name="GameID">游戏ID</param>
        /// <returns>对应的用户ID</returns>
        public static long GetID(string GameID)
        {
            if (!GameIDs.Contains(GameID))
            {
                return 0;
            }
            else
            {
                foreach (Member Item in Items)
                {
                    if (Item.GameID == GameID)
                    {
                        return Item.ID;
                    }
                }
                return 0;
            }
        }

        /// <summary>
        /// 更新群成员信息
        /// </summary>
        /// <param name="JsonObject">消息JSON对象</param>
        /// <param name="UserId">用户ID</param>
        public static void Update(JObject JsonObject, long UserId)
        {
            if (Global.MemberItems.Keys.Contains(UserId))
            {
                lock (Global.MemberItems)
                {
                    Global.MemberItems[UserId].Nickname = JsonObject["sender"]["nickname"].ToString();
                    Global.MemberItems[UserId].Role = Array.IndexOf(Command.Roles, JsonObject["sender"]["role"].ToString());
                    Global.MemberItems[UserId].Card = JsonObject["sender"]["card"].ToString();
                }
            }
        }
    }
}
