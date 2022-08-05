using Fleck;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Timers;

namespace WebConsole
{
    internal static class WebSocket
    {
        private class Socket
        {
            public IWebSocketConnection WebSocketConnection;
            public string WebSocketId;
        }
        private static IDictionary<string, Socket> Sockets = new Dictionary<string, Socket>();

        private static List<string> VerifidKeys = new List<string>();
        private static WebSocketServer server;
        public static void Start()
        {
            FleckLog.LogAction = (Level, Message, e) =>
            {
                switch ((int)Level)
                {
                    case 0:
                        //Console.WriteLine($"\x1b[95m[Debug]\x1b[0m{Message} {e}");
                        break;
                    case 1:
                        Console.WriteLine($"\x1b[96m[Info]\x1b[0m{Message} {e}");
                        break;
                    case 2:
                        Console.WriteLine($"\x1b[33m[Warn]{Message} {e}\x1b[0m");
                        break;
                    case 3:
                        Console.WriteLine($"\x1b[91m[Error]{Message} {e}\x1b[0m");
                        break;
                }
            };
            server = new WebSocketServer("ws://0.0.0.0:30000")
            {
                RestartAfterListenError = true
            };
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.Title = $"WebConsole - Serein ({Sockets.Count})";
                    string ClientUrl = socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort;
                    string GUID = Guid.NewGuid().ToString().Replace("-", string.Empty);
                    Console.WriteLine($"\x1b[36m[+]\x1b[0m<{ClientUrl}> guid:{GUID}, md5:{GetMD5(GUID + (Program.Args.Count > 0 ? Program.Args[0] : "pwd"))}");
                    socket.Send(JsonConvert.SerializeObject(new Packet("request", "verify", GUID, "host")));
                    Sockets.Add(
                        ClientUrl,
                        new Socket()
                        {
                            WebSocketId = GUID,
                            WebSocketConnection = socket
                        }
                        );
                    string Key = socket.ConnectionInfo.Headers.TryGetValue("Sec-WebSocket-Key", out Key) ? Key : "";
                    Timer VerifyTimer = new Timer(5000)
                    {
                        AutoReset = false,
                    };
                    VerifyTimer.Start();
                    VerifyTimer.Elapsed += (sender, e) =>
                    {
                        if (!VerifidKeys.Contains(Key))
                        {
                            socket.Send(JsonConvert.SerializeObject(new Packet("notice", "verify_timeout", "验证超时", "host")));
                            socket.Close();
                        }
                        VerifyTimer.Dispose();
                    };
                };
                socket.OnClose = () =>
                {
                    Console.Title = $"WebConsole - Serein ({Sockets.Count})";
                    string ClientUrl = socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort;
                    Console.WriteLine($"\x1b[93m[-]\x1b[0m<{ClientUrl}>");
                    Sockets.Remove(ClientUrl);
                    VerifidKeys.Remove(socket.ConnectionInfo.Headers.TryGetValue("Sec-WebSocket-Key", out string Key) ? Key : "");
                };
                socket.OnMessage = message =>
                {
                    string ClientUrl = socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort;
                    Console.WriteLine($"\x1b[92m[v]\x1b[0m<{ClientUrl}> {message}");
                    if (Sockets.Keys.Contains(ClientUrl))
                    {
                        string Key = socket.ConnectionInfo.Headers.TryGetValue("Sec-WebSocket-Key", out Key) ? Key : "";
                        Packet packet = new Packet();
                        try
                        {
                            packet = JsonConvert.DeserializeObject<Packet>(message);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"\x1b[91m[Error]序列化数据包时出现错误(From:{ClientUrl}):{e}\x1b[0m");
                        }
                        switch (packet.Type)
                        {
                            case "reply":
                                if (!VerifidKeys.Contains(Key) && packet.SubType == "verify")
                                {
                                    if (
                                        !string.IsNullOrEmpty(Key) &&
                                        packet.Data == GetMD5(
                                            Sockets[ClientUrl].WebSocketId +
                                            (Program.Args.Count > 0 ? Program.Args[0] : "pwd")
                                        )
                                    )
                                    {
                                        socket.Send(JsonConvert.SerializeObject(new Packet("notice", "verify_success", "验证成功", "host")));
                                        VerifidKeys.Add(Key);
                                    }
                                    else
                                    {
                                        socket.Send(JsonConvert.SerializeObject(new Packet("notice", "verify_failed", "验证失败", "host")));
                                        socket.Close();
                                    }
                                }
                                break;
                        }
                    }
                };
            });
        }

        public static string GetMD5(string myString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(myString);
            byte[] targetData = md5.ComputeHash(fromData);
            string Result = string.Empty;
            for (int i = 0; i < targetData.Length; i++)
            {
                Result += targetData[i].ToString("x2");
            }
            return Result;
        }
    }
}
