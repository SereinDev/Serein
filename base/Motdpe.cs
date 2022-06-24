using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Serein
{
    internal class Motdpe
    {
        public string ip { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 19132;
        public string MaxPlayer { get; set; } = "-";
        public string OnlinePlayer { get; set; } = "-";
        public string Description { get; set; } = "-";
        public string Protocol { get; set; } = "-";
        public string Version { get; set; } = "-";
        public string LevelName { get; set; } = "-";
        public string GameMode { get; set; } = "-";
        public TimeSpan Span { get; set; } = TimeSpan.Zero;
        public string Original { get; set; } = "-";
        public Motdpe(string newip = "127.0.0.1", int newPort = 19132)
        {
            ip = newip;
            Port = newPort;
            int length = 0;
            string Data = string.Empty;
            DateTime StartTime = DateTime.Now;
            try
            {
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000);
                client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 5000);
                client.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0));
                string[] Guids = Guid.NewGuid().ToString().ToUpper().Split('-');
                int TickCount = Environment.TickCount;
                string Text = $"01{TickCount.ToString("X").PadLeft(16, '0')}00FFFF00FEFEFEFEFDFDFDFD12345678{Guids[2]}{Guids[4]}";
                byte[] sendBytes = new byte[Text.Length / 2];
                for (int i = 0; i < Text.Length / 2; i++)
                {
                    sendBytes[i] = Convert.ToByte(Text.Substring(i * 2, 2), 16);
                }
                StartTime = DateTime.Now;
                client.SendTo(sendBytes, new IPEndPoint(IPAddress.Parse(ip), Port));
                EndPoint point = new IPEndPoint(IPAddress.Any, 0);
                byte[] buffer = new byte[1024];
                length = client.ReceiveFrom(buffer, ref point);
                Data = length > 35 ?
                    Encoding.UTF8.GetString(buffer, 35, length - 35) :
                    Encoding.UTF8.GetString(buffer, 0, length);
                client.Close();
            }
            catch { return; }
            if (length > 35)
            {
                Span = DateTime.Now - StartTime;
                Original = Data;
                string[] Datas = Data.Split(';');
                if (Datas.Length >= 12)
                {
                    Description = Regex.Replace(Datas[1], "§.", string.Empty);
                    Protocol = Datas[2];
                    Version = Datas[3];
                    OnlinePlayer = Datas[4];
                    MaxPlayer = Datas[5];
                    LevelName = Datas[7];
                    GameMode = Datas[8];
                }
            }
        }
    }
}
