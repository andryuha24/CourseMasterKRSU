namespace TCPServer
{
    public class Message
    {
        public readonly string Content;
        public int NumberOfWhoRead { get; set; }
        public bool DeliveredToAll;
        public int NumOfMsgSentTo { get; set; }
        public bool Sent;
        public Message(string msg, int numOfWhoRead)
        {
            NumberOfWhoRead = numOfWhoRead;
            Content = msg;
            DeliveredToAll = false;
            Sent = false;
            NumOfMsgSentTo = 0;
        }
    }
}
