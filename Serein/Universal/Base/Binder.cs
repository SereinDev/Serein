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
        /// 只读的 Global.MemberItems Value副本
        /// </summary>
        private static List<Member> Items => Global.MemberItems.Values.ToList();

        /// <summary>
        /// 绑定（无群反馈）
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <param name="Value">值</param>
        /// <returns>绑定结果</returns>
        public static bool Bind(long UserId, string Value)
        {
            Value = Value.Trim();
            if (Global.MemberItems.Keys.Contains(UserId) || !System.Text.RegularExpressions.Regex.IsMatch(Value, @"^[a-zA-Z0-9_\s-]{4,16}$") || GameIDs.Contains(Value))
                return false;
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
            Value = Value.Trim();
            if (Global.MemberItems.Keys.Contains(UserId))
                EventTrigger.Trigger(EventType.BindingFailDueToAlreadyBinded, GroupId, UserId);
            else if (!System.Text.RegularExpressions.Regex.IsMatch(Value, @"^[a-zA-Z0-9_\s-]{4,16}$"))
                EventTrigger.Trigger(EventType.BindingFailDueToInvalid, GroupId, UserId);
            else if (GameIDs.Contains(Value))
                EventTrigger.Trigger(EventType.BindingFailDueToOccupation, GroupId, UserId);
            else
            {
                Member Item = new Member()
                {
                    ID = UserId,
                    Card = JsonObject["sender"]["card"].ToString(),
                    Nickname = JsonObject["sender"]["nickname"].ToString(),
                    Role = Array.IndexOf(Command.Roles, JsonObject["sender"]["role"].ToString()),
                    GameID = Value
                };
                Dictionary<long, Member> Items = Global.MemberItems;
                Items.Add(UserId, Item);
                Global.UpdateMemberItems(Items);
                IO.SaveMember();
                EventTrigger.Trigger(EventType.BindingSucceed, GroupId, UserId);
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
                EventTrigger.Trigger(EventType.UnbindingFail, GroupId, UserId);
            else
            {
                lock (Global.MemberItems)
                {
                    Global.MemberItems.Remove(UserId);
                    IO.SaveMember();
                    EventTrigger.Trigger(EventType.UnbindingSucceed, GroupId, UserId);
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
                lock (Global.MemberItems)
                {
                    Global.MemberItems.Remove(UserId);
                    IO.SaveMember();
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
                return string.Empty;
            else
            {
                foreach (Member Item in Items)
                {
                    if (Item.ID == UserId)
                        return Item.GameID;
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
            GameID = GameID.Trim();
            if (!GameIDs.Contains(GameID))
                return 0;
            else
            {
                foreach (Member Item in Items)
                {
                    if (Item.GameID == GameID)
                        return Item.ID;
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
                Logger.Out(LogType.Debug, JsonObject["sender"]);
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
