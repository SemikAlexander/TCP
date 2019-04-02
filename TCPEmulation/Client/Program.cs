using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            int PORT = 0;
            Console.Write("ПОРТ: ");
            while (!int.TryParse(Console.ReadLine(), out PORT))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка ввода!");
                Console.ResetColor();
            }
            Console.Write("IP: ");
            string IP = Console.ReadLine();
            Client client = new Client(IP, PORT);
            client.Run();
        }
    }
}
