using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Configuration;

namespace CreateNewSensor
{
    static class Parser
    {
        static IEnumerable<decimal> ParsData(string file)
        {
            var points = new List<decimal>();
            using (StreamReader sr = new StreamReader(file))
            {
                while (!sr.EndOfStream)
                {
                    var str = sr.ReadLine();
                    if (str != "")
                    {
                        var s = str.Replace('.', ',');
                        points.Add(decimal.Parse(s));
                    }
                }
            }
            return points;
        }

        public static IEnumerable<decimal> GetFeedData(string id)
        {
            return ParsData(AppDomain.CurrentDomain.BaseDirectory + $"App_Data\\feedData_sensor_{id}.csv");
        }
    }

    class CreateNewSensor
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Input sensor id:");
            var sensorId = Console.ReadLine();

            IEnumerable<decimal> data;
            try
            {
                data = Parser.GetFeedData(sensorId);
            }
            catch(FileNotFoundException)
            {
                Console.WriteLine("No feed data for this sensor");
                return;
            }

            Console.WriteLine("-------------------------------------");

            using (var webClient = new WebClient())
            {
                var url = ConfigurationManager.AppSettings.GetValues("serviceUrl")[0] + sensorId;
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                var rez = webClient.UploadString(url, "POST", JsonConvert.SerializeObject(data));

                if(rez == "true")
                    Console.WriteLine("Sensor added successful");
                else
                    Console.WriteLine($"Sensor with id {sensorId} already exists");
            }

        }
    }
}
