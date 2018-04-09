using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace MapReduceInterfaces
{
    [ServiceContract(CallbackContract = typeof(IClient))]
    public interface IDistrCalcService
    {
        [OperationContract]
        void SubscribeClient(string userName);

        [OperationContract]
        void UnSubscribeClient();

        [OperationContract]
        void SendData(DataForProcessing data);
    }

    public class FileToProcessing
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
    }

    [DataContract]
    public class DataForProcessing
    {
        [DataMember]
        public List<FileToProcessing> FileList { get; set; }
        public string ForWhom { get; set; }

        public List<KeyValuePair<string, int>> Map(string text)
        {
            var words = text.Split(new[] {" ",",", "\r\n", "\n", "?", ".", ":", "-","!", ";"}, StringSplitOptions.RemoveEmptyEntries);
            return words.Select(word => new KeyValuePair<string, int>(word, 1)).ToList();
        }

        public List<KeyValuePair<string, int>>  Reduce(List<List<KeyValuePair<string,int>>> inputList)
        {
            var result = new List<KeyValuePair<string,int>>();
            foreach (var list in inputList)
            {
                result.Add(new KeyValuePair<string, int>(list[0].Key, list.Sum(x => x.Value)));
            }
            return result;
        }
    }
}
