using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serein.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

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
                List<long> list = new List<long>();
                foreach (MemberItem Item in MemberItems)
                {
                    list.Add(Item.ID);
                }
                return list;
            }
        }

        /// <summary>
        /// 游戏ID集合
        /// </summary>
        public static List<string> GameIDs
        {
            get
            {
                List<string> list = new List<string>();
                foreach (MemberItem Item in MemberItems)
                {
                    list.Add(Item.GameID);
                }
                return list;
            }
        }

        /// <summary>
        /// 只读的 Global.MemberItems 副本
        /// </summary>
        private static List<MemberItem> MemberItems
        {
            get
            {
                List<MemberItem> TempList = new List<MemberItem>();
                Global.MemberItems.ForEach(i => TempList.Add(i));
                return TempList;
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public static void Load()
        {
            if (!Directory.Exists(Global.Path + "\\data"))
            {
                Directory.CreateDirectory(Global.Path + "\\data");
            }
            if (File.Exists($"{Global.Path}\\data\\members.json"))
            {
                StreamReader Reader = new StreamReader(
                    File.Open(
                        $"{Global.Path}\\data\\members.json",
                        FileMode.Open
                    ),
                    Encoding.UTF8);
                string Text = Reader.ReadToEnd();
                if (!string.IsNullOrEmpty(Text))
                {
                    try
                    {
                        JObject JsonObject = (JObject)JsonConvert.DeserializeObject(Text);
                        if (JsonObject["type"].ToString().ToUpper() != "MEMBERS")
                        {
                            return;
                        }
                        Global.UpdateMemberItems(((JArray)JsonObject["data"]).ToObject<List<MemberItem>>());
                    }
                    catch { }
                }
                Reader.Close();
            }
            List<MemberItem> memberItems = MemberItems;
            memberItems.Sort(
                (Item1, Item2) =>
                {
                    return Item1.ID > Item2.ID ? 1 : -1;
                }
                );
            Global.UpdateMemberItems(memberItems);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public static void Save()
        {
            List<MemberItem> memberItems = MemberItems;
            memberItems.Sort(
                (Item1, Item2) =>
                {
                    return Item1.ID > Item2.ID ? 1 : -1;
                }
                );
            Global.UpdateMemberItems(memberItems);
            if (!Directory.Exists(Global.Path + "\\data"))
            {
                Directory.CreateDirectory(Global.Path + "\\data");
            }
            StreamWriter MembersWriter = new StreamWriter(
                File.Open(
                    $"{Global.Path}\\data\\members.json",
                    FileMode.Create,
                    FileAccess.Write
                    ),
                Encoding.UTF8
                );
            JObject ListJObject = new JObject();
            JArray ListJArray = new JArray();
            foreach (MemberItem Item in MemberItems)
            {
                ListJArray.Add(JObject.FromObject(Item));
            }
            ListJObject.Add("type", "MEMBERS");
            ListJObject.Add("comment", "非必要请不要直接修改文件，语法错误可能导致数据丢失");
            ListJObject.Add("data", ListJArray);
            MembersWriter.Write(ListJObject.ToString());
            MembersWriter.Flush();
            MembersWriter.Close();
        }

        /// <summary>
        /// 绑定（无群反馈）
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <param name="Value">值</param>
        /// <returns>绑定结果</returns>
        public static bool Bind(long UserId, string Value)
        {
            if (IDs.Contains(UserId) || !Regex.IsMatch(Value, @"^[a-zA-Z0-9_\s-]{4,16}$") || GameIDs.Contains(Value))
                return false;
            else
            {
                MemberItem Item = new MemberItem()
                {
                    ID = UserId,
                    GameID = Value
                };
                List<MemberItem> memberItems = MemberItems;
                memberItems.Add(Item);
                Global.UpdateMemberItems(memberItems);
                Save();
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
            else if (!Regex.IsMatch(Value, @"^[a-zA-Z0-9_\s-]{4,16}$"))
            {
                EventTrigger.Trigger("Bind_Invalid", GroupId, UserId);
            }
            else if (GameIDs.Contains(Value))
            {
                EventTrigger.Trigger("Bind_Occupied", GroupId, UserId);
            }
            else
            {
                MemberItem Item = new MemberItem()
                {
                    ID = UserId,
                    Card = JsonObject["sender"]["card"].ToString(),
                    Nickname = JsonObject["sender"]["nickname"].ToString(),
                    Role = Array.IndexOf(Command.Roles, JsonObject["sender"]["role"].ToString()),
                    GameID = Value
                };
                List<MemberItem> memberItems = MemberItems;
                memberItems.Add(Item);
                Global.UpdateMemberItems(memberItems);
                Save();
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
                List<MemberItem> memberItems = MemberItems;
                foreach (MemberItem Item in memberItems)
                {
                    if (Item.ID == UserId && memberItems.Remove(Item))
                    {
                        Global.UpdateMemberItems(memberItems);
                        Save();
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
                List<MemberItem> memberItems = MemberItems;
                foreach (MemberItem Item in memberItems)
                {
                    if (Item.ID == UserId && memberItems.Remove(Item))
                    {
                        Global.UpdateMemberItems(memberItems);
                        Save();
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
                foreach (MemberItem Item in MemberItems)
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
                foreach (MemberItem Item in MemberItems)
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
            if (IDs.Contains(UserId))
            {
                List<MemberItem> memberItems = MemberItems;
                foreach (MemberItem Item in MemberItems)
                {
                    if (Item.ID == UserId && memberItems.Remove(Item))
                    {
                        Item.Role = Array.IndexOf(Command.Roles, JsonObject["sender"]["role"].ToString());
                        Item.Nickname = JsonObject["sender"]["nickname"].ToString();
                        Item.Card = JsonObject["sender"]["card"].ToString();
                        memberItems.Add(Item);
                        Global.UpdateMemberItems(memberItems);
                        Save();
                        break;
                    }
                }
            }
        }
    }
}
