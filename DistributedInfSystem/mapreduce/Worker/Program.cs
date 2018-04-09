using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using MapReduceInterfaces;

namespace Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.SetWindowSize(Math.Min(85, Console.LargestWindowWidth), Math.Min(15, Console.LargestWindowHeight));
                var address = new Uri("http://localhost:4010/IJobTracker");
                var binding = new WSDualHttpBinding
                {
                    MaxBufferPoolSize = 2147483647,
                    MaxReceivedMessageSize = 2147483647,
                    ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas
                    {
                        MaxDepth = 2147483647,
                        MaxStringContentLength = 2147483647,
                        MaxArrayLength = 2147483647,
                        MaxBytesPerRead = 2147483647,
                        MaxNameTableCharCount = 2147483647
                    }
                };
                var endpoint = new EndpointAddress(address);
                var instanceContext = new InstanceContext(new WorkerNotifyHandler());
                var channelFactory = new DuplexChannelFactory<IJobTracker>(instanceContext, binding);
                var workerService = channelFactory.CreateChannel(endpoint);
             
                Console.WriteLine("Choose type of worker, 1 - mapper, 2 - reducer:");

                var input = Console.ReadLine();
                string type = "";
                switch (input)
                {
                    case "1":
                        type = "mapper";
                        StartWorker(workerService, type);
                        break;
                    case "2":
                        type = "reducer";
                        StartWorker(workerService, type);
                        break;
                    default:
                        Console.WriteLine("You entered incorrect data.");
                        break;
                }
                Console.ReadKey();
                workerService.UnSubscribeWorker();
                channelFactory.Close();
            }
            catch (EndpointNotFoundException)
            {
                Console.WriteLine("Server isn't available. Please try later...");
                Console.ReadKey();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Console.ReadKey();
            }
        }

        private static void StartWorker(IJobTracker workerService, string type)
        {
            Console.WriteLine("If the server does not respond wait for a minute...");
            workerService.SubscribeWorker(type);
        }
    }
    public class WorkerNotifyHandler : IWorker
    {
        public List<KeyValuePair<string,int>> ReceiveDataForMap(DataForProcessing testData, List<FileToProcessing> files, string type)
        {
            var res = new List<KeyValuePair<string, int>>();
            string result = null;
            foreach (var file in files)
                result += '\n' + Encoding.UTF8.GetString(file.Content);
            if (type=="mapper")
                res = testData.Map(result);
            Console.WriteLine("'Map' operation has finished");
            return res;
        }

        public List<KeyValuePair<string, int>> ReceiveDataForReduce(DataForProcessing testData, List<List<KeyValuePair<string, int>>> dataAfterMap, string type)
        {
            var res = new List<KeyValuePair<string, int>>();
            if (type == "reducer")
                res = testData.Reduce(dataAfterMap);
            Console.WriteLine("'Reduce' operation has finished.");
            return res;
        }

        public void SystemMessage(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
