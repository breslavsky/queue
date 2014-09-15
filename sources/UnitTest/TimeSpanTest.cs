using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;

namespace UnitTest
{
    [TestClass]
    public class TimeSpanTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var timeSpan = new TimeSpan(1, 1, 1, 1, 510);

            var x = string.Format("{0:hh\\:mm\\:ss}", timeSpan);

            timeSpan = TimeSpan.FromMilliseconds(12);

            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(TimeSpan));
            dataContractJsonSerializer.WriteObject(stream, timeSpan);
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            string output = reader.ReadToEnd();
            Console.WriteLine(output);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var timeSpan1 = new TimeSpan(1, 1, 1, 1, 510);
            var timeSpan2 = new TimeSpan(1, 1, 1, 1, 110);

            var timeSpan3 = timeSpan1.Subtract(timeSpan2).Duration();
            var x = timeSpan3.Ticks;
        }

        [TestMethod]
        public void TestMethod3()
        {
            var clientInterval = TimeSpan.FromMinutes(30);
            var intervalTime = new TimeSpan(10, 0, 0);
            var timeIntervalRounding = TimeSpan.FromMinutes(5);
            var intersection = TimeSpan.FromMinutes(0);

            var rounding = new TimeSpan(intervalTime.Ticks % timeIntervalRounding.Ticks);
            Trace.WriteLine(rounding.ToString());

            intervalTime = intervalTime.Add(rounding <= intersection ? -rounding : timeIntervalRounding.Subtract(rounding));

            Trace.WriteLine(intervalTime.ToString());
        }
    }
}