using System.Collections.Generic;
using RAFT.Interfaces;

namespace RAFT.Models
{
    public class PeerState
    {
        public int Id { get; set; }
        public int CurrentTerm { get; set; }
        public int? VotedForId { get; set; }
        public List<LogItem> Log { get; set; } = new List<LogItem>();
        public PeerInfo ThisServerInfo { get; set; }
        public PeerInfo Leader { get; set; }
        public Dictionary<string, Persister> PersistedStates { get; set; } = new Dictionary<string, Persister>();

        public int CommittedIndex { get; set; }
        public int LastAppliedIndex { get; set; }

        public Dictionary<string, int> MatchIndex { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> NextIndex { get; set; } = new Dictionary<string, int>();

        public Dictionary<PeerInfo,IPeer> PeerlList { get; set; } = new Dictionary<PeerInfo,IPeer>();
        public CurrentServerState CurrentState { get; set; } = CurrentServerState.Follower;


    }
}