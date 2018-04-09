using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using RAFT.Interfaces;
using RAFT.Models;
using static System.Console;

namespace RAFT
{
    public class Peer
    {
        private List<bool> _serversReadyToStart = new List<bool>();
        private DuplexChannelFactory<IPeer> _channelFactory;
        private ServiceHost _host = new ServiceHost(typeof(MasterService));
        private readonly int _numOfServers = int.Parse(ConfigurationManager.AppSettings["NumOfservers"]);
        private readonly Random _rand = new Random();
        private int ServerNumber { get; set; }
        private int NumOfRunnigSrvs { get; set; }
        public static PeerState MyState { get; set; }
        private int VotesReceived { get; set; }
        private int _numberOfAddToLog = 0;

        public Peer()
        {
            MasterService.Election = new Timer(ChangeToCandidate);
            MasterService.HeartBeat = new Timer(StartHeartbeat);
            MasterService.WorkTimer = new Timer(StartAdd);
        }

        public void StartServer()
        {
            CreateServer();
            if (NumOfRunnigSrvs == _numOfServers)
            {
                WriteLine("All existing servers are already running.");
                ReadKey();
                return;
            }

            WriteLine("Server " + ServerNumber + " start");
            ConnectToPeers();
            MasterService.ReadyToStart = true;
            while (true)
            {
                GetServersStatus();
                if (_serversReadyToStart.Count == MasterService.Peers.Count && _serversReadyToStart.All(x => x))
                {
                    Thread.Sleep(1000);
                    break;
                }

            }
            foreach (var peer in MyState.PeerlList)
            {
                var persister = peer.Value.ReadPersist(MyState.ThisServerInfo);
                if (persister != null)
                {
                    persister.ReadPeerState(MyState);
                    break;
                }
            }
            var electionTime = MasterService.ElectionTimeout;
            WriteLine("ElectionTimeout " + electionTime);
            MasterService.Election.Change(electionTime, Timeout.Infinite);
            ReadKey();
            _channelFactory.Close();
            _host.Close();
        }

        private void CreateServer()
        {
            var binding = new WSDualHttpBinding();
            var contract = typeof(IPeer);

            for (var i = 0; i < _numOfServers; i++)
            {
                if (_host.State != CommunicationState.Opened)
                {
                    try
                    {
                        _host = null;
                        _host = new ServiceHost(typeof(MasterService));
                        var address = new Uri("http://localhost:400" + i + "/IPeer");
                        _host.AddServiceEndpoint(contract, binding, address);
                        _host.Open();
                        ServerNumber = i;

                    }
                    catch (AddressAlreadyInUseException)
                    {
                        NumOfRunnigSrvs++;
                    }
                }
            }
            if (NumOfRunnigSrvs!=_numOfServers)
            {
                MyState = new PeerState
                {
                    Id = ServerNumber,
                    ThisServerInfo = new PeerInfo
                    {
                        Id = "S" + ServerNumber,
                        Address = "http://localhost:400" + ServerNumber + "/IPeer",
                        PersistedState = new Persister()
                    },
                    CommittedIndex = -1
                };
            }
        }

        private void ConnectToPeers()
        {
            var peerBinding = new WSDualHttpBinding
            {
                SendTimeout = new TimeSpan(0, 0, 1, 0),
                ReceiveTimeout = new TimeSpan(0, 0, 20, 0)
            };
            var instanceContext = new InstanceContext(new ClientNotifyHandler());
            _channelFactory = new DuplexChannelFactory<IPeer>(instanceContext, peerBinding);
            for (var i = 0; i < _numOfServers; i++)
            {
                if (i != ServerNumber)
                {
                    var peerInfo = new PeerInfo
                    {
                        Id = "S" + i,
                        Address = "http://localhost:400" + i + "/IPeer"
                    };
                    var address = new Uri("http://localhost:400" + i + "/IPeer");
                    MyState.PeerlList[peerInfo] = ConnectToPeer(MyState.ThisServerInfo.Id, address);
                }
            }
        }

        private IPeer ConnectToPeer(string myId, Uri addres)
        {
            var peerAddress = addres;
            var peerEndpoint = new EndpointAddress(peerAddress);
            var peerService = _channelFactory.CreateChannel(peerEndpoint);
            peerService.SubscribeClient(myId);
            return peerService;
        }

        private void GetServersStatus()
        {
            _serversReadyToStart.Clear();
            for (var i = 0; i < MyState.PeerlList.Count; i++)
            {
                _serversReadyToStart.Add(MyState.PeerlList.ElementAt(i).Value.GetServerReadyStatus());
            }
        }

        private void CheckServerState()
        {
            switch (MyState.CurrentState)
            {
                case CurrentServerState.Follower:
                    MasterService.Election.Change(MasterService.ElectionTimeout, Timeout.Infinite);
                    break;
                case CurrentServerState.Candidate:
                    StartLeaderElection();
                    break;
                case CurrentServerState.Leader:
                    WriteLine($"Server {MyState.Id} is now sending heartbeat.");
                    MasterService.HeartBeat.Change(0, 2000);
                    break;
            }
        }

        private void ChangeToCandidate(object state)
        {
            WriteLine($"Server {MyState.Id} is becoming a candidate");
            MyState.CurrentState = CurrentServerState.Candidate;
            MasterService.WorkTimer.Change(Timeout.Infinite, Timeout.Infinite);
            MasterService.Election.Change(MasterService.ElectionTimeout, Timeout.Infinite);
            MyState.VotedForId = null;
            MyState.Leader = null;
            CheckServerState();
        }

        private void ChangeToFollower()
        {
            if(MyState.CurrentState==CurrentServerState.Follower)
                return;
            WriteLine($"Server {MyState.Id} reverting to follower from {MyState.CurrentState}");
            MyState.CurrentState = CurrentServerState.Follower;
            MyState.VotedForId = null;
            MasterService.Election.Change(MasterService.ElectionTimeout, Timeout.Infinite);
            MasterService.WorkTimer.Change(Timeout.Infinite, Timeout.Infinite);
            CheckServerState();
        }

        private void StartLeaderElection()
        {
            try
            {
                MyState.CurrentTerm++;
                VotesReceived = 0;
                MyState.VotedForId = MyState.Id;
                VotesReceived++;
                MyState.ThisServerInfo.PersistedState.SavePeerState(MyState);
                for (var i = 0; i < MyState.PeerlList.Count; i++)
                {
                    RunVotingProcess(MyState.PeerlList.ElementAt(i).Key);
                }
            }
            catch (Exception exception)
            {
                WriteLine(exception.Message);
            }
        }

        public void RunVotingProcess(PeerInfo peerInfo)
        {
            var voteRequest = new RequestVote()
            {
                CandidateId = MyState.Id,
                LastLogIndex = MyState.Log.Count-1,
                LastLogTerm = MyState.Log.Select(l => l.Term).LastOrDefault(),
                Term = MyState.CurrentTerm
            };

            MyState.PeerlList[peerInfo] = ConnectToPeer(MyState.ThisServerInfo.Id, new Uri(peerInfo.Address));
            MyState.PeerlList[peerInfo].Persist(MyState.ThisServerInfo);
            var voteResponse = MyState.PeerlList[peerInfo].GetVote(voteRequest);

            if (voteResponse.Term > MyState.CurrentTerm)
            {
                MyState.CurrentTerm = voteResponse.Term;
                VotesReceived = 0;
                ChangeToFollower();

                MyState.ThisServerInfo.PersistedState.SavePeerState(MyState);
                MyState.PeerlList[peerInfo].Persist(MyState.ThisServerInfo);
            }
            else if (voteResponse.VoteGranted && MyState.CurrentState == CurrentServerState.Candidate)
            {
                VotesReceived++;
                if (VotesReceived >= (MyState.PeerlList.Count/2 + 1) &&
                    MyState.CurrentState == CurrentServerState.Candidate)
                {
                    StartLeadership();
                }
            }
        }
        private void StartLeadership()
        {
            MyState.CurrentState = CurrentServerState.Leader;
            MyState.MatchIndex.Clear();
            MyState.NextIndex.Clear();
            MasterService.WorkTimer.Change(0, 5000);
            MasterService.Election.Change(Timeout.Infinite, Timeout.Infinite);
            foreach (var node in MyState.PeerlList)
            {
                MyState.MatchIndex.Add(node.Key.Id, -1);
                MyState.NextIndex.Add(node.Key.Id, MyState.Log.Count);
            }
            CheckServerState();
        }

        public void StartHeartbeat(object state)
        {
            try
            {
                for (var i = 0; i < MyState.PeerlList.Count; i++)
                {
                    SendHeartBeat(MyState.PeerlList.ElementAt(i).Key);
                }
            }
            catch (Exception exception)
            {
                WriteLine(exception.Message);
            }
        }

        public void SendHeartBeat(PeerInfo peerInfo)
        {
            var nextIndex = MyState.NextIndex[peerInfo.Id];
            var logEntries = new List<LogItem>();
            if (nextIndex < MyState.Log.Count)
            {
                var entriesCount = Math.Min(50, MyState.Log.Count - nextIndex);
                logEntries = MyState.Log.GetRange(nextIndex, entriesCount);
            }

            var appendRequest = new AppendEntriesRequest
            {
                Leader = MyState.ThisServerInfo,
                Term = MyState.CurrentTerm,
                LeaderCommitIndex = MyState.CommittedIndex,
                PrevLogIndex = nextIndex > 0 ? nextIndex - 1 : -1,
                PrevLogTerm = nextIndex > 0 ? MyState.Log[nextIndex - 1].Term : -1,
                entries = logEntries
            };

            MyState.PeerlList[peerInfo] = ConnectToPeer(MyState.ThisServerInfo.Id, new Uri(peerInfo.Address));
            var response = MyState.PeerlList[peerInfo].AppendEntry(appendRequest);
            if (response.Term > MyState.CurrentTerm)
            {
                WriteLine($"Server {MyState.Id} is reverting to follower from {MyState.CurrentState}");
                MyState.CurrentTerm = response.Term;
                ChangeToFollower();
                MyState.ThisServerInfo.PersistedState.SavePeerState(MyState);

                MyState.PeerlList[peerInfo].Persist(MyState.ThisServerInfo);

                MasterService.HeartBeat.Change(Timeout.Infinite, Timeout.Infinite);
            }
            var node = response.Id;
            if (response.Success)
            {
                MyState.MatchIndex[node] = appendRequest.PrevLogIndex+logEntries.Count;
                MyState.NextIndex[node] = MyState.MatchIndex[node]+1;
                if (!MyState.Log.Any())
                    return;
               
                for (var i = MyState.Log.Count-1; i > MyState.CommittedIndex; i--)
                {
                    if (MyState.Log[i].Term == MyState.CurrentTerm &&
                        MyState.MatchIndex.Values.Count(mi => mi >= i) > MyState.PeerlList.Count/2)
                    {
                        MyState.CommittedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                if (MyState.NextIndex[node] > 0)
                    MyState.NextIndex[node]--;
            }
        }

        private void StartAdd(object state)
        {
            try
            {
                if (_numberOfAddToLog < 5)
                {
                    MyState.Log.Add(new LogItem { Term = MyState.CurrentTerm, Value = _rand.Next(0, 100) });
                    MyState.ThisServerInfo.PersistedState.SavePeerState(MyState);
                    WriteLine(MyState.Log.Count);
                    _numberOfAddToLog++;
                    for (var i = 0; i < MyState.PeerlList.Count; i++)
                    {
                        var address = new Uri(MyState.PeerlList.ElementAt(i).Key.Address);
                        MyState.PeerlList[MyState.PeerlList.ElementAt(i).Key] = ConnectToPeer(MyState.ThisServerInfo.Id, address);
                        MyState.PeerlList.ElementAt(i).Value.Persist(MyState.ThisServerInfo);
                    }

                }
                else
                    MasterService.WorkTimer.Change(new Random().Next(1000, 2000), Timeout.Infinite);
            }
            catch (Exception exception)
            {
               WriteLine(exception.Message);
            }
        }

    }
    public class ClientNotifyHandler : IClient
    {
        public void SystemMessage(string msg)
        {
            WriteLine(msg);
        }
    }
}
