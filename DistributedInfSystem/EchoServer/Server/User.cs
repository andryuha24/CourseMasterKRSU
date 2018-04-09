using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCPServer
{
    public class User
    {
        public TcpClient Client;
        public NetworkStream ClientStream;
        public StreamReader Reader;

        public User(TcpClient clientSocket)
        {
            Client = clientSocket;
            ClientStream = Client.GetStream();
            Reader = new StreamReader(ClientStream, Encoding.Default);
        }
        public void StartClient()
        {
            Thread receiveThread = new Thread(ReceiveMessage);
            receiveThread.Start();
            receiveThread.IsBackground = true;
        }
        private void ReceiveMessage()
        {
            try
            {
                while (Client.Connected)
                {
                    var getMessage = GetMessage(Reader);
                    MessageProcessing(getMessage);
                    Console.WriteLine("Log: {0} [{1}]", getMessage, DateTime.Now);
                    StreamWriter writer = new StreamWriter(Server.FileStream);
                    writer.WriteLine("Log: {0} [{1}]", getMessage, DateTime.Now);
                    writer.Flush();
                }
            }
            catch(IOException)
            {
                Server.EndUser(this);
            }
        }

        public void MessageProcessing(string getMessage)
        {
            if (getMessage != "ok!")
            {
                if (Server.MessageCollection.Count != Server.SizeofBuffer)
                    Server.MessageCollection.Enqueue(new Message(getMessage, 0));
            }
            else
            {
                Message msg;
                if (Server.MessageCollection.TryPeek(out msg))
                {
                    msg.NumberOfWhoRead++;
                    if (msg.NumberOfWhoRead == msg.NumOfMsgSentTo)
                        msg.DeliveredToAll = true;
                }
            }
        }

        public string GetMessage(StreamReader reader)
        {
            var getMessage = Reader.ReadLine();
            return getMessage;
        }
    }
}
