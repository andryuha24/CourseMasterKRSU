using System.Runtime.Serialization;

namespace RAFT.Models
{
    [DataContract]
    public class PeerInfo
    {
        [DataMember]
        public string Id { get; set; }

        public string Address { get; set; }

        [DataMember]
        public Persister PersistedState { get; set; }
    }
}
