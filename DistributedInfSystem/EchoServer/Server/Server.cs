using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCPServer
{
    public class Server
    {
        public static List<User> Clients = new List<User>();
        public static ConcurrentQueue<Message> MessageCollection = new ConcurrentQueue<Message>();
        public static int SizeofBuffer = 100;
        public TcpListener ServerListener;
        private TcpClient _client;
        private int _port;
        private IPAddress _ipAddress;
        public static FileStream FileStream;
        private static Random _random = new Random();

        public Server(IPAddress ip, int port)
        {
            _ipAddress = ip;
            _port = port;
            Console.WriteLine("Server settings: IP - "+_ipAddress+"; Port - "+ _port+"\n" +
                              "Server starting...");
            ServerListener = new TcpListener(_ipAddress, _port);
            ServerListener.Start();
        }

        public static void EndUser(User user)
        {
            if (!Clients.Contains(user))
                return;
            Clients.Remove(user);
            user.Client.Close();
            if (Clients.Count == 0 && MessageCollection.Count > 0)
                MessageCollection.ToList().ForEach(msg => MessageCollection.TryDequeue(out msg));
            Message message;
            if (MessageCollection.TryPeek(out message))
            {
                if (message.NumberOfWhoRead < message.NumOfMsgSentTo)
                    message.DeliveredToAll = true;
            }
        }

        public void StartServer()
        {
            try
            {
                FileStream = new FileStream("ServerLog" + _random.Next(500) + ".txt", FileMode.Create);
                Thread sendThread = new Thread(SendMessages) {IsBackground = true};
                sendThread.Start();
                while (true)
                {
                    var usr = AddNewClient();
                    usr.StartClient();
                }
            }
            catch{}
            finally { FileStream.Dispose(); }
        }

        public User AddNewClient()
        {
            _client = ServerListener.AcceptTcpClient();
            User r = new User(_client);
            Clients.Add(r);
            return r;
        }

        private void SendMessages()
        {
            while (true)
            {
                SendToAll();
            }
        }

        public void SendToAll()
        {
            Message msg;
            if (MessageCollection.TryPeek(out msg))
            {
                if (msg.Sent && msg.DeliveredToAll)
                {
                    MessageCollection.TryDequeue(out msg);
                }
                if (msg != null && !msg.Sent)
                {
                    for (int i = 0; i < Clients.Count; i++)
                    {
                        StreamWriter streamWriter = new StreamWriter(Clients.ElementAt(i).ClientStream, Encoding.Default);
                        streamWriter.WriteLine("Server: {0} [{1}]", msg.Content, DateTime.Now);
                        streamWriter.Flush();
                        msg.NumOfMsgSentTo++;
                    }
                    msg.Sent = true;
                }
            }
        }
    }
}