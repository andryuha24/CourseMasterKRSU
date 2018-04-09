using System.ServiceModel;

namespace RAFT.Interfaces
{
    public interface IClient
    {
        [OperationContract(IsOneWay = true)]
        void SystemMessage(string msg);
    }
}
