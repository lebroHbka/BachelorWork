using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sensor
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

        public static IEnumerable<decimal> GetRearData()
        {
            var fileName = ConfigurationManager.AppSettings.GetValues("fileName")[0];
            return ParsData(AppDomain.CurrentDomain.BaseDirectory + $"App_Data\\{fileName}");
        }
    }

    public class SensorDataPoint
    {
        public decimal Value { get; set; }
        public long TimeTicks { get; set; }
    }

    class Sensor
    {
        static void Main(string[] args)
        {
            var date = new DateTime(2018, 3, 22, 0, 0, 0);
            var rawData = Parser.GetRearData();
            var dataSend = rawData.Select((u, counter) => new SensorDataPoint
            {
                TimeTicks = date.AddMinutes(5 * counter).Ticks,
                Value = u
            });

            var action = new Action(() =>
            {
                var url = ConfigurationManager.AppSettings.GetValues("serviceUrl")[0];
                var delay = Int32.Parse(ConfigurationManager.AppSettings.GetValues("sendDelay")[0]);

                foreach (var d in dataSend)
                {
                    using (var webClient = new WebClient())
                    {
                        webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

                        IEnumerable<SensorDataPoint> dat = new List<SensorDataPoint> { d };
                        
                        var rez = webClient.UploadString(url, "POST", JsonConvert.SerializeObject(dat));
                        if (!Boolean.Parse(rez))
                        {
                            Console.WriteLine("Send FAILS");
                        }
                    }
                    System.Threading.Thread.Sleep(delay);
                }
            });

            var task = new Task(action);

            task.Start();

            Console.WriteLine("RUNNING...");

            task.Wait();

            Console.WriteLine("FINISH");
        }
    }
}
