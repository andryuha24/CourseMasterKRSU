using System.ServiceModel;

namespace MapReduceInterfaces
{
    [ServiceContract(CallbackContract = typeof(IWorker))]
    public interface IJobTracker
    {
        [OperationContract]
        void SubscribeWorker(string type);

        [OperationContract]
        void UnSubscribeWorker();
    }
}
