using System.Runtime.Serialization;

namespace RAFT.Models
{
    [DataContract]
    public class RequestVote
    {
        [DataMember]
        public int Term { get; set; }
        [DataMember]
        public int CandidateId { get; set; }
        [DataMember]
        public int LastLogIndex { get; set; }
        [DataMember]
        public int LastLogTerm { get; set; }
        //[DataMember]
        //public Persister PersistedState { get; set; }
    }
}
