using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Serein
{
    internal class Motd
    {
        public static MotdpeItem GetPe(MotdpeItem Item)
        {
            try
            {
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, -5000);
                client.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0));
                string[] Guids = Guid.NewGuid().ToString().ToUpper().Split('-');
                int TickCount = Environment.TickCount;
                string Text = $"01{TickCount.ToString("X").PadLeft(16, '0')}00FFFF00FEFEFEFEFDFDFDFD12345678{Guids[2]}{Guids[4]}";
                byte[] sendBytes = new byte[Text.Length / 2];
                for (int i = 0; i < Text.Length / 2; i++)
                {
                    sendBytes[i] = Convert.ToByte(Text.Substring(i * 2, 2), 16);
                }
                DateTime StartTime = DateTime.Now;
                client.SendTo(sendBytes, new IPEndPoint(IPAddress.Parse(Item.ip), Item.Port));
                EndPoint point = new IPEndPoint(IPAddress.Any, 0);
                byte[] buffer = new byte[1024];
                int length = client.ReceiveFrom(buffer, ref point);
                string Message = length > 35 ?
                    Encoding.UTF8.GetString(buffer, 35, length - 35) :
                    Encoding.UTF8.GetString(buffer, 0, length);
                client.Close();
                if (length > 35)
                {
                    Item.Serialization(Message);
                    Item.Span = DateTime.Now - StartTime;
                }
            }
            catch { }
            return Item;
        }
    }
}
