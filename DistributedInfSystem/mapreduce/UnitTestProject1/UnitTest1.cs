using System;
using System.Collections.Generic;
using MapReduceInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMap()
        {
            var data = new DataForProcessing();
            var result = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("Hello",1),
                new KeyValuePair<string, int>("world",1),
                new KeyValuePair<string, int>("Hello",1),
                new KeyValuePair<string, int>("my",1),
                new KeyValuePair<string, int>("friend",1)
            };
            var actual = data.Map("Hello world! Hello my friend.");
            CollectionAssert.AreEqual(result,actual);
        }

        [TestMethod]
        public void TestReduce()
        {
            var data = new DataForProcessing();
            var dataForReduce = new List<List<KeyValuePair<string, int>>>
            {
                new List<KeyValuePair<string, int>>
                {
                    new KeyValuePair<string, int>("Hello", 1),
                    new KeyValuePair<string, int>("Hello", 1)
                },
                new List<KeyValuePair<string, int>>
                {
                   new KeyValuePair<string, int>("world", 1)
                },
                new List<KeyValuePair<string, int>>
                {
                    new KeyValuePair<string, int>("my", 1)
                },
                new List<KeyValuePair<string, int>>
                {
                    new KeyValuePair<string, int>("friend", 1)
                }
            };
            var result = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("Hello",2),
                new KeyValuePair<string, int>("world",1),
                new KeyValuePair<string, int>("my",1),
                new KeyValuePair<string, int>("friend",1)
            };

            var actual = data.Reduce(dataForReduce);
            CollectionAssert.AreEqual(result, actual);
        }
    }
}
