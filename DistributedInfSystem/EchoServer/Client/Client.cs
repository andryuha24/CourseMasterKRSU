using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCPClient
{
    public class Client
    {
        public TcpClient _client;
        private NetworkStream _clientStream;
        private StreamReader _reader;
        private StreamWriter _writer;
        private int _port;
        private IPAddress _ipAddress;
        private bool _delay;
        private List<string> _messagesFromFile;
        private readonly FileStream _fileStream;
        private static Random _random = new Random();
        public Client(IPAddress ip, int port, bool delay)
        {
            _ipAddress = ip;
            _port = port;
            _delay = delay;
            Console.WriteLine("Default connection settings: IP - " + _ipAddress + "; Port - " + _port);
            _client = new TcpClient();
            _client.Connect(_ipAddress, _port);
            _clientStream = _client.GetStream();
            _reader = new StreamReader(_clientStream, Encoding.Default);
            _writer = new StreamWriter(_clientStream, Encoding.Default);
            _fileStream = new FileStream("ClientLog" +_random.Next(500) + ".txt", FileMode.Create);
        }

        public void AdditionalSettings()
        {
            Console.WriteLine("If you want to add delay to client (slow-reading)\n" +
                              "then press F2, else press any key.");
            if (Console.ReadKey().Key == ConsoleKey.F2)
                _delay = true;
            Console.WriteLine("If you want to add an input txt file with text press F3, else press any key.");
            if (Console.ReadKey().Key != ConsoleKey.F3) return;
            Console.WriteLine("Input file path:");
            var input = Console.ReadLine();
            try
            {
                if (input != null) _messagesFromFile = File.ReadAllLines(input).ToList();
            }
            catch
            {
                Console.WriteLine("You did not input a valid path");
            }
        }

        public void StartClient()
        {
            Console.WriteLine("Connected to server!");
            Thread sendThread = new Thread(Send);
            sendThread.Start();
            Thread receiveThread = new Thread(Recieve);
            receiveThread.Start();
            receiveThread.IsBackground = true;
        }

        private void Send()
        {
            try
            {
                while (true)
                    if ((_messagesFromFile != null) && (_messagesFromFile.Count != 0))
                    {
                        foreach (var str in _messagesFromFile)
                        {
                            _writer.WriteLine(str);
                            _writer.Flush();
                        }
                        _messagesFromFile.Clear();
                    }
                    else
                    {
                        var input = Console.ReadLine();
                        SendMessage(input);
                        StreamWriter writer = new StreamWriter(_fileStream);
                        writer.WriteLine(input);
                        writer.Flush();
                    }
            }
            catch{}
            finally {_fileStream.Dispose();}
        }

        public void SendMessage(string input)
        {
            _writer.WriteLine(input);
            _writer.Flush();
        }

        private void Recieve()
        {
            try
            {
                while (_client.Connected)
                {
                    if (_delay) Thread.Sleep(10000);
                    string recievedMsg = ReceiveMessage();
                    StreamWriter writer = new StreamWriter(_fileStream);
                    writer.WriteLine(recievedMsg);
                    writer.Flush();
                }
            }
            catch
            {
                Console.WriteLine("Connection to the server was interrupted.\n" +
                                  "Please, press ENTER...");
                Console.ReadLine();
                Environment.Exit(0);
            }
            finally { _fileStream.Dispose(); }
        }

        public string ReceiveMessage()
        {
            var recievedMsg = _reader.ReadLine();
            Console.WriteLine(recievedMsg);
            _writer.WriteLine("ok!");
            _writer.Flush();
            return recievedMsg;
        }
    }
}