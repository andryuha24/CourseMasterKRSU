using System;
using System.ServiceModel;
using MapReduceInterfaces;

namespace MasterServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(Math.Min(85, Console.LargestWindowWidth), Math.Min(15, Console.LargestWindowHeight));
            var addressClient = new Uri("http://localhost:4000/IDistrCalcService");
            var bindingClient = new WSDualHttpBinding
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
            var contractClient = typeof(IDistrCalcService);

            var addressWorker = new Uri("http://localhost:4010/IJobTracker");
            var bindingWorker = new WSDualHttpBinding
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
            var contractWorker = typeof(IJobTracker);
           
            var host = new ServiceHost(typeof(MasterService));

            host.AddServiceEndpoint(contractClient, bindingClient, addressClient);
            host.AddServiceEndpoint(contractWorker, bindingWorker, addressWorker);

            host.Open();
            
            Console.WriteLine("Server is running.");
            Console.WriteLine("Press the Enter key to exit the program...");
            Console.ReadKey();
            host.Close();
        }
    }
}
