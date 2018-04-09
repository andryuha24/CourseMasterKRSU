using System.ServiceModel;
using RAFT.Models;


namespace RAFT.Interfaces
{
    [ServiceContract(CallbackContract = typeof(IClient))]
    public interface IPeer
    {
        [OperationContract]
        void SubscribeClient(string userName);

        [OperationContract]
        void UnSubscribeClient(string userName);

        [OperationContract]
        bool GetServerReadyStatus();

        [OperationContract]
        ResponseVote GetVote(RequestVote requestVote);

        [OperationContract]
        AppendEntriesResponse AppendEntry(AppendEntriesRequest requestVote);

        [OperationContract (IsOneWay = true)]
        void Persist(PeerInfo peer);

        [OperationContract]
        Persister ReadPersist(PeerInfo peer);
    }
}
