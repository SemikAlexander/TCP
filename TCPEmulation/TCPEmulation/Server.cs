using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace TCPEmulation
{
    public class Server
    {
        public object obj = new object();
        int timeOut;
        public int port = 0, countClient = 0, currentClient = 0;
        Socket socket;
        public Server(int TIME_OUT, int PORT, int COUNT_CLIENT)
        {
            timeOut = TIME_OUT;
            port = PORT;
            countClient = COUNT_CLIENT;
        }
        public void Run()
        {
            IPAddress adress = IPAddress.Any;
            IPEndPoint endPoint = new IPEndPoint(adress, port);
            socket = new Socket(adress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(endPoint);
            socket.Listen(countClient);
            bool s;
            while (true)
            {
                Console.WriteLine($"Ожидаем соединение через порт {endPoint}");
                s = true;
                while (s)
                {
                    lock (obj)
                    {
                        if (currentClient >= countClient)
                            Thread.Sleep(500);
                        else
                            s = false;
                    }
                }
                Socket handler = socket.Accept();
                handler.ReceiveTimeout = timeOut;
                handler.SendTimeout = timeOut;
                currentClient++;
                new Thread(new ParameterizedThreadStart(new Client(this).Run)).Start(handler);
            }
        }
    }
    class Client
    {
        Socket socket;
        public Client(Server serverTCP)
        {
            server = serverTCP;
        }
        Server server;
        public void Run(object ob)
        {
            try
            {
                socket = (Socket)ob;
                byte[] bytes = new byte[128];
                string ms = "";
                while (true)
                {

                    do
                    {
                        int k = socket.Receive(bytes);
                        ms += Encoding.Unicode.GetString(bytes, 0, k);
                    }
                    while (socket.Available > 0);
                    Console.WriteLine($"\nПришло: {ms}\n");
                    socket.Send(Encoding.Unicode.GetBytes(ms));
                }
            }
            catch (Exception ex)
            {
                lock (server.obj)
                    server.currentClient--;
                socket.Close();
                Console.WriteLine($"Клиент отключен: {ex.Message}");
            }
        }
    }
}