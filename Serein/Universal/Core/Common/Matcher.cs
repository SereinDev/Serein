using Serein.Base.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Serein.Core.Common
{
    internal static class Matcher
    {
        /// <summary>
        /// 处理来自控制台的消息
        /// </summary>
        /// <param name="line">控制台的消息</param>
        public static void Process(string line)
        {
            lock (Global.RegexList)
            {
                foreach (Base.Regex regex in Global.RegexList)
                {
                    if (
                        string.IsNullOrEmpty(regex.Expression)
                        || regex.Area != 1
                        || !System.Text.RegularExpressions.Regex.IsMatch(line, regex.Expression)
                    )
                    {
                        continue;
                    }
                    Task.Run(
                        () =>
                            Command.Run(
                                Base.CommandOrigin.Console,
                                regex.Command,
                                System.Text.RegularExpressions.Regex.Match(line, regex.Expression)
                            )
                    );
                }
            }
        }

        /// <summary>
        /// 匹配消息
        /// </summary>
        /// <param name="messagePacket">数据包</param>
        /// <param name="isSelfMessage">是否为自身消息</param>
        public static void MatchMsg(Message messagePacket, bool isSelfMessage)
        {
            lock (Global.RegexList)
            {
                foreach (Base.Regex regex in Global.RegexList)
                {
                    if (
                        string.IsNullOrEmpty(messagePacket.RawMessage)
                        || string.IsNullOrEmpty(regex.Expression) // 表达式为空
                        || regex.Area <= 1 // 禁用或控制台
                        || !(isSelfMessage ^ regex.Area != 4) // 自身消息与定义域矛盾
                        || !System.Text.RegularExpressions.Regex.IsMatch(
                            messagePacket.RawMessage,
                            regex.Expression
                        ) // 不匹配
                        || regex.Area == 2 && regex.Ignored.ToList().Contains(messagePacket.GroupId)
                        || regex.Area == 3 && regex.Ignored.ToList().Contains(messagePacket.UserId) // 忽略
                    )
                    {
                        continue;
                    }

                    if (!IsAdmin(messagePacket) && regex.IsAdmin && !isSelfMessage)
                    {
                        switch (regex.Area)
                        {
                            case 2:
                                EventTrigger.Trigger(
                                    Base.EventType.PermissionDeniedFromGroupMsg,
                                    messagePacket!
                                );
                                break;

                            case 3:
                                EventTrigger.Trigger(
                                    Base.EventType.PermissionDeniedFromPrivateMsg,
                                    messagePacket!
                                );
                                break;
                        }
                        continue;
                    }

                    if (
                        (regex.Area == 4 || regex.Area == 2) && messagePacket.MessageType == "group"
                        || (regex.Area == 4 || regex.Area == 3)
                            && messagePacket.MessageType == "private"
                    )
                    {
                        Command.Run(
                            Base.CommandOrigin.Msg,
                            regex.Command,
                            System.Text.RegularExpressions.Regex.Match(
                                messagePacket.RawMessage,
                                regex.Expression
                            ),
                            messagePacket,
                            false
                        );
                    }
                }
            }

            if (messagePacket.MessageType == "group")
            {
                UpdateGroupCache(messagePacket);
            }
        }

        /// <summary>
        /// 判断是否为管理
        /// </summary>
        /// <param name="messagePacket">数据包</param>
        /// <returns>是否为管理</returns>
        private static bool IsAdmin(Message messagePacket) =>
            Global.Settings.Bot.PermissionList.Contains(messagePacket.UserId)
            || Global.Settings.Bot.GivePermissionToAllAdmin
                && messagePacket.MessageType == "group"
                && messagePacket.Sender!.RoleIndex < 2;

        /// <summary>
        /// 更新群组缓存
        /// </summary>
        /// <param name="messagePacket">数据包</param>
        private static void UpdateGroupCache(Message messagePacket)
        {
            lock (Global.GroupCache)
            {
                if (!Global.GroupCache.ContainsKey(messagePacket.GroupId))
                {
                    Global.GroupCache.Add(
                        messagePacket.GroupId,
                        new Dictionary<long, Base.Member>()
                    );
                }
                if (!Global.GroupCache[messagePacket.GroupId].ContainsKey(messagePacket.UserId))
                {
                    Global.GroupCache[messagePacket.GroupId].Add(
                        messagePacket.UserId,
                        new Base.Member()
                    );
                }
                Base.Member member = Global.GroupCache[messagePacket.GroupId][messagePacket.UserId];
                member.ID = messagePacket.UserId;
                member.Nickname = messagePacket.Sender!.Nickname ?? string.Empty;
                member.Card = messagePacket.Sender.Card ?? string.Empty;
                member.Role = messagePacket.Sender.RoleIndex;
                member.GameID = Binder.GetGameID(messagePacket.UserId);

                Global.GroupCache[messagePacket.GroupId][messagePacket.UserId] = member;
            }
        }
    }
}
