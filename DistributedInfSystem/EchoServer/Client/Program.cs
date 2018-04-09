using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace TCPClient
{
    public class Program
    {
        static int port = 8888;
        static IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        static void Main(string[] args)
        {

            bool delay = false;
            try
            {
                Console.WriteLine("Default server settings: IP - " + ipAddress + "; Port - " + port);
                Settings();
                Client client = new Client(ipAddress, port, delay);
                client.AdditionalSettings();
                client.StartClient();
            }
            catch (SocketException)
            {
                Console.WriteLine("The server is down. Try starting the server and reopening this window.");
                Console.ReadLine(); ;
            }
        }
        private static void Settings()
        {
            Console.WriteLine("If you want to change the default connection settings type:\n" +
                              "\t1 - change port and ip-address  -  press F1\n" +
                              "\t2 - continue - press any key.");
            if (Console.ReadKey().Key == ConsoleKey.F1)
            {
                SetNewIp();
                SetNewPort();
            }
        }

        private static void SetNewPort()
        {
            var input = "";
            Console.WriteLine("New Port: ");
            while (!int.TryParse(input, out port))
                input = Console.ReadLine();
        }
        private static void SetNewIp()
        {
            var input = "";
            Console.WriteLine("New IP addres: ");
            while (!IPAddress.TryParse(input, out ipAddress))
                input = Console.ReadLine();
        }
    }
}
