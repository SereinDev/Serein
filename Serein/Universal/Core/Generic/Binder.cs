using Serein.Base;
using Serein.Base.Packets;
using Serein.Core.Server;
using Serein.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Serein.Core.Generic
{
    internal static class Binder
    {
        /// <summary>
        /// 只读的 Global.MemberDict Value副本
        /// </summary>
        private static List<Member> _items => Global.MemberDict.Values.ToList();

        /// <summary>
        /// 游戏ID集合
        /// </summary>
        public static List<string> GameIDs => _items.Select((member) => member.GameID).ToList();

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
        /// <param name="message">数据包</param>
        /// <param name="value">值</param>
        public static void Bind(Message message, string? value)
        {
            if (message is null || message.Sender is null || string.IsNullOrEmpty(value))
            {
                return;
            }
            value = value!.Trim();
            if (Global.MemberDict.ContainsKey(message.UserId))
            {
                EventTrigger.Trigger(EventType.BindingFailDueToAlreadyBinded, message);
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(value, Global.Settings.Serein.Function.RegexForCheckingGameID))
            {
                EventTrigger.Trigger(EventType.BindingFailDueToInvalid, message);
                return;
            }
            if (Global.Settings.Serein.Function.DisableBinderWhenServerClosed ? !ServerManager.Status : false)
            {
                EventTrigger.Trigger(EventType.BinderDisable, message);
                return;
            }
            if (GameIDs.Contains(value))
            {
                EventTrigger.Trigger(EventType.BindingFailDueToOccupation, message);
                return;
            }
            lock (Global.MemberDict)
            {
                Global.MemberDict.Add(message.UserId, new()
                {
                    ID = message.UserId,
                    Card = message.Sender.Card ?? string.Empty,
                    Nickname = message.Sender.Nickname ?? string.Empty,
                    Role = message.Sender.RoleIndex,
                    GameID = value
                });
            }
            IO.SaveMember();
            EventTrigger.Trigger(EventType.BindingSucceed, message);
        }

        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="message">数据包</param>
        public static void UnBind(Message message)
        {
            if (Global.Settings.Serein.Function.DisableBinderWhenServerClosed ? !ServerManager.Status : false)
            {
                EventTrigger.Trigger(EventType.BinderDisable, message);
                return;
            }
            lock (Global.MemberDict)
            {
                if (Global.MemberDict.Remove(message.UserId))
                {
                    IO.SaveMember();
                    EventTrigger.Trigger(EventType.UnbindingSucceed, message);
                }
                else
                {
                    EventTrigger.Trigger(EventType.UnbindingFail, message);
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
            => Global.MemberDict.TryGetValue(userID, out Member? member) ? member.GameID : string.Empty;

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
        /// <param name="message">数据包</param>
        public static void Update(Message message)
        {
            if (message is not null && message.Sender is not null && Global.MemberDict.TryGetValue(message.UserId, out Member? member))
            {
                Logger.Output(LogType.Debug, message.Sender);
                member.Nickname = message.Sender.Nickname ?? string.Empty;
                member.Role = message.Sender.RoleIndex;
                member.Card = message.Sender.Card ?? string.Empty;
            }
        }
    }
}
