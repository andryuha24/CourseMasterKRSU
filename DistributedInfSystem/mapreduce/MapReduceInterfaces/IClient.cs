using System.Collections.Generic;
using System.ServiceModel;

namespace MapReduceInterfaces
{
    public interface IClient
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveData(List<KeyValuePair<string,int>> result);

        [OperationContract(IsOneWay = true)]
        void SystemMessage(string msg);
    }
}
