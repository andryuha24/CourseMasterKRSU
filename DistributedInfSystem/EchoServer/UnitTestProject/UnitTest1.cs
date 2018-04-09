using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCPServer;
using TCPClient;
namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestConnection()
        {
            var port = 5555;
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            Server srv = new Server(ipAddress, port);
            Client tcpClient = new Client(ipAddress,port,false);
            Assert.IsTrue(tcpClient._client.Connected);
        }

        [TestMethod]
        public void Disconect()
        {
            var port = 4444;
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            Server srv = new Server(ipAddress, port);
            Server.Clients.Clear();
            if (Server.MessageCollection.Count != 0)
                Server.MessageCollection.ToList().ForEach(m => Server.MessageCollection.TryDequeue(out m));
            Client tcpClient = new Client(ipAddress, port, false);
            var user = srv.AddNewClient();
            Client tcpClient2 = new Client(ipAddress, port, false);
            var user2 = srv.AddNewClient();
            var msg = "Hello World!";
            Server.MessageCollection.Enqueue(new Message(msg,0));

            Server.EndUser(user);
           
            Assert.IsTrue(Server.Clients.Count == 1);
        }
  
        [TestMethod]
        public void AddClient()
        {
            var port = 3333;
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            Server srv = new Server(ipAddress, port);
            Server.Clients.Clear();
            Client tcpClient = new Client(ipAddress, port, false);
            srv.AddNewClient();
            Assert.IsTrue(Server.Clients.Count > 0);
        }

        [TestMethod]
        public void SendMsgToServer()
        {
            var port = 7777;
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            Server srv = new Server(ipAddress, port);
            Server.Clients.Clear();
            if(Server.MessageCollection.Count!=0)
                Server.MessageCollection.ToList().ForEach(m=>Server.MessageCollection.TryDequeue(out m));
            Client tcpClient = new Client(ipAddress, port, false);
            var user = srv.AddNewClient();
            var msg = "Hello World!";
            tcpClient.SendMessage(msg);
            string receivedMsg = user.GetMessage(user.Reader);
            Assert.AreEqual(msg, receivedMsg);
        }


        [TestMethod]
        public void SendMsgState()
        {
            var port = 2222;
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            Server srv = new Server(ipAddress, port);
            Server.Clients.Clear();
            if (Server.MessageCollection.Count != 0)
                Server.MessageCollection.ToList().ForEach(m => Server.MessageCollection.TryDequeue(out m));
            Client tcpClient = new Client(ipAddress, port, false);
            var user = srv.AddNewClient();
            Message msg = new Message("Hello World!",0);
            Server.MessageCollection.Enqueue(msg);
            srv.SendToAll();
            tcpClient.ReceiveMessage();
            var receivedMsg = user.GetMessage(user.Reader);
            user.MessageProcessing(receivedMsg);
            if(Server.MessageCollection.TryPeek(out msg))
                Assert.IsTrue(msg.DeliveredToAll);
            else
                Assert.Fail();
        }
    }

}
