using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using MapReduceInterfaces;

namespace MasterServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public  class MasterService : IDistrCalcService, IJobTracker
    {
        public readonly Dictionary<IClient, string> Clients = new Dictionary<IClient, string>();
        public readonly Dictionary<IWorker, string> Workers = new Dictionary<IWorker, string>();
        private static readonly object LockObject = new object();
        public void SubscribeClient(string userName)
        {
            var connection = OperationContext.Current.GetCallbackChannel<IClient>();
            Clients[connection] = userName;
            connection.SystemMessage("Welcome to our server!");
            Thread.Sleep(5000);
        }

        public void UnSubscribeClient()
        {
            var connection = OperationContext.Current.GetCallbackChannel<IClient>();
            Clients.Remove(connection);
        }

        public void SubscribeWorker(string type)
        {
            var connection = OperationContext.Current.GetCallbackChannel<IWorker>();
            Workers[connection] = type;
            connection.SystemMessage("Successfully joined!");
        }

        public void UnSubscribeWorker()
        {
            var connection = OperationContext.Current.GetCallbackChannel<IWorker>();
            Workers.Remove(connection);
        }

        public void SendData(DataForProcessing data)
        {
            lock (LockObject)
            {
                var connection = OperationContext.Current.GetCallbackChannel<IClient>();
                var mapResult = new List<KeyValuePair<string, int>>();
                var reduceResult = new List<KeyValuePair<string, int>>();
                var numMappers = Workers.Count(x => x.Value.Contains("mapper"));
                var numReducers = Workers.Count(x => x.Value.Contains("reducer"));

                if (numMappers > 0 && numReducers > 0)
                {
                    var dataForMappers = SplitDataForChunks(data.FileList, numMappers).ToList();
                    SendDataForMappers(data, mapResult, numMappers, dataForMappers);
                    var groupedCustomerList = mapResult.GroupBy(u => u.Key).Select(grp => grp.ToList()).ToList();
                    var dataForReducers = SplitDataForChunks(groupedCustomerList, numMappers).ToList();
                    SendDataForReducers(data, reduceResult, dataForReducers);
                    connection.ReceiveData(reduceResult);
                    Thread.Sleep(5000);
                }
                else
                {
                    connection.SystemMessage("Server doesn't have enough workers for for calculations.\nPlease try again later");
                }
             
            }
        }

        private void SendDataForReducers(DataForProcessing data, List<KeyValuePair<string, int>> reduceResult, List<List<List<KeyValuePair<string, int>>>> dataForReducers)
        {
            foreach (var worker in Workers)
            {
                if (worker.Value != "reducer") continue;
                foreach (var list in dataForReducers)
                {
                    reduceResult.AddRange(worker.Key.ReceiveDataForReduce(data, list, worker.Value));
                }
            }
        }

        private void SendDataForMappers(DataForProcessing data, List<KeyValuePair<string, int>> mapResult, int numMappers, List<List<FileToProcessing>> dataForMappers)
        {
            for (var i = 0; i < Workers.Count; i++)
            {
                if (Workers.ElementAt(i).Value != "mapper") continue;
                foreach (var list in dataForMappers)
                {
                    mapResult.AddRange(Workers.ElementAt(i).Key.ReceiveDataForMap(data, list, Workers.ElementAt(i).Value));
                }
                if (dataForMappers.Count < numMappers) i++;
            }
        }

        public IEnumerable<List<T>> SplitDataForChunks<T>(List<T> list, int numWorkers)
        {
            var j = 0;
            var splits = from item in list
                          group item by j++ % numWorkers into part
                          select part.AsEnumerable().ToList();
            return splits;
        }
    }
   
}
