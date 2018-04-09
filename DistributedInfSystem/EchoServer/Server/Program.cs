using System;
using System.Net;

namespace TCPServer
{
    class Program
    {
        static int port = 8888;
        static IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        static void Main(string[] args)
        {
            Console.WriteLine("Default server settings: IP - " + ipAddress + "; Port - " + port);
            Settings();
            Server simpleChatServer = new Server(ipAddress, port);
            simpleChatServer.StartServer();
        }
        private static void Settings()
        {
            Console.WriteLine("If you want to change the default connection settings type:\n" +
                              "\t1 - change port and ip-address  -  press F1\n" +
                              "\t2 - continue - press any key");
            if (Console.ReadKey().Key == ConsoleKey.F1)
            {
                var input = "";
                Console.WriteLine("New IP addres: ");
                while (!IPAddress.TryParse(input, out ipAddress))
                    input = Console.ReadLine();
                Console.WriteLine("New Port: ");
                while (!int.TryParse(input, out port))
                    input = Console.ReadLine();
            }
        }
       
    }
}
