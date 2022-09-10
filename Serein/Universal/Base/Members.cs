using Newtonsoft.Json.Linq;
using Serein.Items;
using System;
using System.Collections.Generic;

namespace Serein.Base
{
    internal class Members
    {
        /// <summary>
        /// 用户ID集合
        /// </summary>
        public static List<long> IDs
        {
            get
            {
                List<long> IDList = new List<long>();
                foreach (Member Item in Items)
                {
                    IDList.Add(Item.ID);
                }
                return IDList;
            }
        }

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
                List<Member> TempList = new List<Member>();
                Global.MemberItems.ForEach(i => TempList.Add(i));
                return TempList;
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
            if (IDs.Contains(UserId) || !System.Text.RegularExpressions.Regex.IsMatch(Value, @"^[a-zA-Z0-9_\s-]{4,16}$") || GameIDs.Contains(Value))
                return false;
            else
            {
                Member Item = new Member()
                {
                    ID = UserId,
                    GameID = Value
                };
                List<Member> Items = Members.Items;
                Items.Add(Item);
                Global.UpdateMemberItems(Items);
                Loader.SaveMember();
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
            if (IDs.Contains(UserId))
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
                List<Member> Items = Members.Items;
                Items.Add(Item);
                Global.UpdateMemberItems(Items);
                Loader.SaveMember();
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
            if (!IDs.Contains(UserId))
            {
                EventTrigger.Trigger("Unbind_Failure", GroupId, UserId);
            }
            else
            {
                List<Member> Items = Members.Items;
                foreach (Member Item in Items)
                {
                    if (Item.ID == UserId && Items.Remove(Item))
                    {
                        Global.UpdateMemberItems(Items);
                        Loader.SaveMember();
                        EventTrigger.Trigger("Unbind_Success", GroupId, UserId);
                        break;
                    }
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
            if (IDs.Contains(UserId))
            {
                List<Member> Items = Members.Items;
                foreach (Member Item in Items)
                {
                    if (Item.ID == UserId && Items.Remove(Item))
                    {
                        Global.UpdateMemberItems(Items);
                        Loader.SaveMember();
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 获取指定用户ID对应的游戏ID
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns>对应的游戏ID</returns>
        public static string GetGameID(long UserId)
        {
            if (!IDs.Contains(UserId))
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
            /*
            if (IDs.Contains(UserId))
            {
                List<MemberItem> Items = Items;
                foreach (MemberItem Item in Items)
                {
                    if (Item.ID == UserId && Items.Remove(Item))
                    {
                        Item.Role = Array.IndexOf(Command.Roles, JsonObject["sender"]["role"].ToString());
                        Item.Nickname = JsonObject["sender"]["nickname"].ToString();
                        Item.Card = JsonObject["sender"]["card"].ToString();
                        Items.Add(Item);
                        Global.UpdateMemberItems(Items);
                        Loader.SaveMember();
                        break;
                    }
                }
            }*/
        }
    }
}
