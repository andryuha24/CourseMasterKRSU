using System.Collections.Generic;

namespace RAFT.Models
{
    public class Persister
    {
        private int CurrentTerm { get; set; }
        private int? VotedForId { get; set; }
        private List<LogItem> Log { get; set; }

        public void ReadPeerState(PeerState peerState)
        {
            peerState.Log = Log;
            peerState.CurrentTerm = CurrentTerm;
            peerState.VotedForId = VotedForId;
        }

        public void SavePeerState(PeerState peerState)
        {
            Log = peerState.Log; 
            CurrentTerm = peerState.CurrentTerm;
            VotedForId = peerState.VotedForId;
        }
    }
}
