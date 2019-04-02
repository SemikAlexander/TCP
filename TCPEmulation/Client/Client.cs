using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    public class Client
    {
        IPAddress ipAddr;
        IPEndPoint ipEndPoint;
        Socket senderSocket;
        string IP; int PORT;
        public Client(string ip, int port)
        {
            IP = ip;
            PORT = port;
            ipAddr = IPAddress.Parse(ip);
            ipEndPoint = new IPEndPoint(ipAddr, port);
        }
        public void Run()
        {
            byte[] bytes = new byte[1024];
            senderSocket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                senderSocket.Connect(ipEndPoint);
                senderSocket.ReceiveTimeout = 10000;
                senderSocket.SendTimeout = 10000;
                Console.WriteLine($"Сокет соединяется с {senderSocket.RemoteEndPoint.ToString()}...");
                while (true)
                {
                    Console.Write("Введите сообщение: ");
                    string message = Console.ReadLine();
                    byte[] msg = Encoding.Unicode.GetBytes(message);
                    string data = "";
                    senderSocket.Send(msg);
                    do
                    {
                        int bytesRec = senderSocket.Receive(bytes);
                        data += Encoding.Unicode.GetString(bytes, 0, bytesRec);
                    }
                    while (senderSocket.Available > 0);
                    Console.WriteLine(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                senderSocket.Close();

            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}