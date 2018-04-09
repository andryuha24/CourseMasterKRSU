using System.Runtime.Serialization;

namespace RAFT.Models
{
    [DataContract]
    public enum CurrentServerState
    {
        [EnumMember]
        Follower,
        [EnumMember]
        Candidate,
        [EnumMember]
        Leader
    }
}