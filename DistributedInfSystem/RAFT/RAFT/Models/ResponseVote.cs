using System.Runtime.Serialization;

namespace RAFT.Models
{
    [DataContract]
    public class ResponseVote
    {
        [DataMember]
        public int Term { get; set; }
        [DataMember]
        public bool VoteGranted { get; set; }

        public ResponseVote(int term, bool voteGranted)
        {
            Term = term;
            VoteGranted = voteGranted;
        }
    }
}
