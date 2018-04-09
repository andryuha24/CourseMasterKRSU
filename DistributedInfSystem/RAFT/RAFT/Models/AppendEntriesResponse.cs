using System.Runtime.Serialization;

namespace RAFT.Models
{
    [DataContract]
    public class AppendEntriesResponse
    {
        [DataMember]
        public int Term { get; set; }
        [DataMember]
        public bool Success { get; set; }
        [DataMember]
        public string Id { get; set; }

        public AppendEntriesResponse(int term, bool succes, string id)
        {
            Term = term;
            Success = succes;
            Id = id;
        }
    }
}
