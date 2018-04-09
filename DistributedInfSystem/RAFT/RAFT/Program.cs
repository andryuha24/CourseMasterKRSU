using System;
using static System.Console;

namespace RAFT
{
    class Program
    {
        static void Main(string[] args)
        {
            Peer peer = new Peer();
            SetWindowSize(Math.Min(85, LargestWindowWidth), Math.Min(15, LargestWindowHeight));                
            peer.StartServer();
            ReadLine();
        }
    }
}
