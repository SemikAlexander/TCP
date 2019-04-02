using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPEmulation
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(100000, 5555, 2);
            server.Run();
        }
    }
}
