using System.Runtime.Serialization;

namespace RAFT.Models
{
    [DataContract]
    public class LogItem
    {
        [DataMember]
        public int Term { get; set; }
        [DataMember]
        public object Value { get; set; }
    }
}