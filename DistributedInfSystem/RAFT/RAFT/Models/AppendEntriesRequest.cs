using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RAFT.Models
{
    [DataContract]
    public class AppendEntriesRequest
    {
        [DataMember]
        public int Term { get; set; }
        [DataMember]
        public PeerInfo Leader { get; set; }
        [DataMember]
        public int PrevLogIndex { get; set; }
        [DataMember]
        public int PrevLogTerm { get; set; }
        [DataMember]
        public List<LogItem> entries { get; set; }
        [DataMember]
        public int LeaderCommitIndex { get; set; }
        //[DataMember]
        //public Persister PersistedState { get; set; }
    }
}
