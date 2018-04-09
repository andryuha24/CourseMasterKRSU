using System.Collections.Generic;
using System.ServiceModel;

namespace MapReduceInterfaces
{
    public interface IWorker
    {
        [OperationContract(IsOneWay = false)]
        List<KeyValuePair<string, int>> ReceiveDataForMap(DataForProcessing data, List<FileToProcessing> files, string type);

        [OperationContract(IsOneWay = false)]
        List<KeyValuePair<string, int>> ReceiveDataForReduce(DataForProcessing testData, List<List<KeyValuePair<string, int>>> data, string type);

        [OperationContract(IsOneWay = true)]
        void SystemMessage(string msg);
    }
}
