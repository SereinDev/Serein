using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Motd
{
    class Program
    {
        static Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        static void Main(string[] args)
        {
            client.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 19130));
            Thread t = new Thread(sendMsg);
            t.Start();
            Thread t2 = new Thread(ReciveMsg);
            t2.Start();
            Console.WriteLine("客户端已经开启");
        }

        static void sendMsg()
        {
            EndPoint point = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 19132);
            while (true)
            {
                string msg = Console.ReadLine();
                string Text = "01000000000097529500FFFF00FEFEFEFEFDFDFDFD1234567811ECD14F6AA190EB";
                byte[] sendBytes = new byte[Text.Length / 2];
                for (int i = 0; i < Text.Length / 2; i++)
                {
                    sendBytes[i] = Convert.ToByte(Text.Substring(i * 2, 2), 16);
                }
                client.SendTo(sendBytes, point);
            }


        }
        static void ReciveMsg()
        {
            while (true)
            {
                EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
                byte[] buffer = new byte[1024];
                int length = client.ReceiveFrom(buffer, ref point);//接收数据报
                string message = Encoding.UTF8.GetString(buffer, 0, length);
                Console.WriteLine(point.ToString() + message);
            }
        }

    }
}