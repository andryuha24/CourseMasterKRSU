using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using RAFT.Interfaces;
using RAFT.Models;
using static System.Console;


namespace RAFT
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class MasterService:IPeer
    {
        public static readonly Dictionary<string,IClient> Peers = new Dictionary<string, IClient>();
        private  readonly object _lockObject = new object();
        public static bool ReadyToStart;
        private static readonly Random _rand = new Random();
        public static Timer HeartBeat { get; set; }
        public static Timer Election { get; set; }
        public static Timer WorkTimer { get; set; }
        public static int ElectionTimeout { get { return _rand.Next(5000, 10000); } }
        public void SubscribeClient(string userName)
        {
            lock (_lockObject)
            {
                var connection = OperationContext.Current.GetCallbackChannel<IClient>();
                if (!Peers.ContainsKey(userName))
                {
                    Peers[userName] = connection;
                    connection.SystemMessage("Welcome " + userName + " to our " + Peer.MyState.Id + " server!" + DateTime.Now);
                }

            }
        }

        public void UnSubscribeClient(string userName)
        {
            lock (_lockObject)
            {
                var connection = OperationContext.Current.GetCallbackChannel<IClient>();
                Peers.Remove(userName);
            }
        }


        public bool GetServerReadyStatus()
        {
            lock (_lockObject)
            {
                return ReadyToStart;
            }
         
        }

        public ResponseVote GetVote(RequestVote requestVote)
        {
            lock (_lockObject)
            {
                Peer.MyState.ThisServerInfo.PersistedState.SavePeerState(Peer.MyState);
                if (requestVote.Term > Peer.MyState.CurrentTerm || (Peer.MyState.CurrentState == CurrentServerState.Candidate
                 && Peer.MyState.CurrentTerm <= requestVote.Term))
                {
                    WriteLine($"Server {Peer.MyState.Id} is reverting to follower from {Peer.MyState.CurrentState}");
                    Peer.MyState.CurrentTerm = requestVote.Term;
                    Peer.MyState.CurrentState = CurrentServerState.Follower;
                }
                var voteGranted = GrantVote(requestVote);
                if (!voteGranted) return new ResponseVote(Peer.MyState.CurrentTerm, false);
                Peer.MyState.VotedForId = requestVote.CandidateId;
                var electTime = ElectionTimeout;
                Election.Change(electTime, Timeout.Infinite);
            }
            return new ResponseVote(Peer.MyState.CurrentTerm, true);
        }

        private bool GrantVote(RequestVote requestVote)
        {
            if (requestVote.Term < Peer.MyState.CurrentTerm)
                return false;
            return (Peer.MyState.VotedForId == null || Peer.MyState.VotedForId == requestVote.CandidateId) && 
                    IsLogOlderOrEqual(requestVote.LastLogIndex, requestVote.LastLogTerm);
        }

        private bool IsLogOlderOrEqual(long lastLogIndex, long lastLogTerm)
        {
            if (Peer.MyState.Log.Count == 0)
                return true;
            var lastEntry = Peer.MyState.Log[Peer.MyState.Log.Count - 1];
            return lastEntry.Term < lastLogTerm || (lastEntry.Term == lastLogTerm && lastLogIndex >= Peer.MyState.Log.Count - 1);
        }
        

        public AppendEntriesResponse AppendEntry(AppendEntriesRequest requestVote)
        {
            var electTime = ElectionTimeout;
            Election.Change(electTime, Timeout.Infinite);
            lock (_lockObject)
            {
                Peer.MyState.ThisServerInfo.PersistedState.SavePeerState(Peer.MyState);
                foreach (var peer in Peer.MyState.PeerlList)
                {
                    peer.Value.Persist(Peer.MyState.ThisServerInfo);
                }

                var differentStateOrLeader = Peer.MyState.CurrentState != CurrentServerState.Follower ||
                                                Peer.MyState.Leader != null && Peer.MyState.Leader.Id != requestVote.Leader.Id;
                if (requestVote.Term > Peer.MyState.CurrentTerm || Peer.MyState.CurrentTerm <= requestVote.Term && differentStateOrLeader)
                {
                    WriteLine($"Server {Peer.MyState.Id} is reverting to follower from {Peer.MyState.CurrentState}");
                    Peer.MyState.CurrentTerm = requestVote.Term;
                    Peer.MyState.CurrentState = CurrentServerState.Follower;
                    Peer.MyState.Leader = requestVote.Leader;
                }
                var success = GrantSuccess(requestVote);

                return new AppendEntriesResponse(Peer.MyState.CurrentTerm, success, Peer.MyState.ThisServerInfo.Id);
            }

        }
        private bool GrantSuccess(AppendEntriesRequest requestVote)
        {
            try
            {
                if (requestVote.Term < Peer.MyState.CurrentTerm)
                {
                    return false;
                }

                if (!EntryTermMatches(requestVote))
                    return false;

                DeleteConflictEntries(requestVote.PrevLogIndex);

                if (requestVote.entries != null)
                {
                    Peer.MyState.Log.AddRange(requestVote.entries);
                }

                if (requestVote.LeaderCommitIndex > Peer.MyState.CommittedIndex)
                {
                    Peer.MyState.CommittedIndex = Math.Min(requestVote.LeaderCommitIndex, Peer.MyState.Log.Count - 1);
                    WriteLine(Peer.MyState.CommittedIndex);
                }
            }
            catch (Exception exception)
            {
                WriteLine(exception.Message);
            }
            return true;
        }

        private bool EntryTermMatches(AppendEntriesRequest requestVote)
        {
            if (requestVote.PrevLogIndex < 0 && Peer.MyState.Log.Count == 0)
                return requestVote.PrevLogIndex == -1;

            if (requestVote.PrevLogIndex >= Peer.MyState.Log.Count)
                return false;

            return requestVote.PrevLogIndex == -1 || Peer.MyState.Log[requestVote.PrevLogIndex].Term == requestVote.PrevLogTerm;
        }
        private void DeleteConflictEntries(int prevLogIndex)
        {
            var index = prevLogIndex + 1;
            if (index >= Peer.MyState.Log.Count)
                return;
            Peer.MyState.Log.RemoveRange(index, Peer.MyState.Log.Count - index);
        }

        public void Persist(PeerInfo peer)
        {
            lock (_lockObject)
            {
                Peer.MyState.PersistedStates[peer.Id] = peer.PersistedState;
            }

        }

        public Persister ReadPersist(PeerInfo peer)
        {
            lock (_lockObject)
            {
                return Peer.MyState.PersistedStates.FirstOrDefault(p => p.Key == peer.Id).Value;
            }
        }
    }
}
