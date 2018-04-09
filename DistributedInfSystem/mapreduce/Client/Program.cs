using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using MapReduceInterfaces;

namespace Client
{
    class Program
    { 
        public static string Name { get; set; }
        private static DuplexChannelFactory<IDistrCalcService> _channelFactory;
        private static IDistrCalcService _clientService;
        private static string Path { get; set; }
        static void Main(string[] args)
        {
            try
            {
                Console.SetWindowSize(Math.Min(85, Console.LargestWindowWidth),Math.Min(15, Console.LargestWindowHeight));
                //Path = ConfigurationManager.AppSettings["pathFolder"];
                var address = new Uri("http://localhost:4000/IDistrCalcService");
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
                    },
                    SendTimeout = new TimeSpan(0, 0, 1, 0),
                    ReceiveTimeout = new TimeSpan(0, 0, 20, 0)
                }; 
                var endpoint = new EndpointAddress(address);
                var instanceContext = new InstanceContext(new ClientNotifyHandler());
                _channelFactory = new DuplexChannelFactory<IDistrCalcService>(instanceContext, binding);
                _clientService = _channelFactory.CreateChannel(endpoint);
                
                Console.WriteLine("Input files path:");
                Path = Console.ReadLine();
                var files = LoadFile(Path);
                if (files == null) throw new ArgumentNullException(nameof(files));
                Console.WriteLine("Input your name:");
                Name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(Name))
                {
                    Console.WriteLine("If the server does not respond wait for a minute...");
                    _clientService.SubscribeClient(Name);
                    var data = new DataForProcessing { FileList = files };
                    _clientService.SendData(data);
                }
                else
                {
                    Console.WriteLine("Incorrect input name.");
                }
                Console.WriteLine("Press the Enter key to exit the program.");
                Console.ReadKey();
                _clientService.UnSubscribeClient();
                _channelFactory.Close();
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("You did not input a valid path");
                Console.WriteLine("Press the Enter key to exit the program.");
                Console.ReadKey();
            }
            catch (EndpointNotFoundException)
            {
                Console.WriteLine("Server isn't available. Please try later...");
                Console.ReadKey();
            }
            catch (Exception exception) { Console.WriteLine(exception.Message); Console.ReadKey(); }
        }
        private static List<FileToProcessing> LoadFile(string path)
        {
            var listOfFiles = new List<FileToProcessing>();
            var dinfo = new DirectoryInfo(path);
            var files = dinfo.GetFiles("*.txt").ToList();
            foreach (var file in files)
            {
                var content = File.ReadAllBytes(file.FullName);
                listOfFiles.Add(new FileToProcessing() { Content = content, FileName = file.Name });
            }
            return listOfFiles;
        }
    }
    public class ClientNotifyHandler : IClient
    {
        private static readonly Random _random = new Random();
        private FileStream _fileStream;
        public void ReceiveData(List<KeyValuePair<string, int>> result)
        {
            try
            {
                _fileStream = new FileStream("Client_"+ Program.Name+"_ResultLog" + _random.Next(500) + ".txt", FileMode.Create);
                var writer = new StreamWriter(_fileStream);
                foreach (var word in result)
                {
                    Console.WriteLine(word);
                    writer.WriteLine(word);
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { _fileStream.Dispose();}
        }

        public void SystemMessage(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
