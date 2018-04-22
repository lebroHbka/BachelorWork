using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using RandomCutForest;
using System.Collections.Concurrent;

namespace SensorService
{
    public class Service : IService
    {
        static string connectionString;
        static int treeCount;
        static double samplingRatio;

        object key = new object();

        static string dateTimeFormat = "yyyy-MM-dd HH:mm:ss.fffffff";
        static ConcurrentDictionary<int, Forest> forestDict = new ConcurrentDictionary<int, Forest>();
        static ConcurrentDictionary<int, DataMaker> dataMakerDict = new ConcurrentDictionary<int, DataMaker>();


        static Service()
        {
            connectionString = Properties.Settings.Default.ConnectionString;
            treeCount = Properties.Settings.Default.treeCount;
            samplingRatio = Properties.Settings.Default.samplingRatio;
            DataMaker.Shingle = Properties.Settings.Default.shingle;
        }

        public bool AddNewSensore(string id, IEnumerable<decimal> feedData)
        {
            var intId = Int32.Parse(id);
            if (forestDict.ContainsKey(intId))
            {
                return false;
            }
            else
            {
                try
                {
                    var f = new Forest(treeCount, samplingRatio);
                    var m = new DataMaker();

                    f.BuildForest(m.NormalizePoints(feedData).ToList());

                    forestDict[intId] = f;
                    dataMakerDict[intId] = m;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public bool SendData(string id, IEnumerable<SensorDataPoint> data)
        {
            var intId = Int32.Parse(id);
            if (!forestDict.ContainsKey(intId))
            {
                return false;
            }

            var anomalyList = new List<decimal>();
            var rawPoints = data.Select(u => u.Value);
            var normalizedPoints = dataMakerDict[intId].NormalizePoints(rawPoints);

            foreach (var p in normalizedPoints)
            {
                anomalyList.Add(forestDict[intId].PointAnomaly(p));
            }
            lock (key)
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var tr = connection.BeginTransaction();

                    var command = connection.CreateCommand();
                    command.Transaction = tr;
                    try
                    {
                        int i = 0;
                        foreach (var d in data)
                        {
                            var value = d.Value.ToString().Replace(',', '.');
                            var anomaly = anomalyList[i++].ToString().Replace(',', '.');
                            var time = new DateTime(d.TimeTicks).ToString(dateTimeFormat);

                            command.CommandText = $"insert SensorsData values ({id}, {value}, {anomaly}, '{time}')";
                            command.ExecuteNonQuery();
                        }
                        tr.Commit();
                    }
                    catch (Exception e)
                    {
                        tr.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }

        public bool DeleteSensor(string id)
        {
            var intId = Int32.Parse(id);
            if(forestDict.ContainsKey(intId) && dataMakerDict.ContainsKey(intId))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand($"delete from SensorsData where sensor_id={intId}", connection);
                    command.ExecuteNonQuery();
                }

                forestDict.TryRemove(intId, out Forest s);
                dataMakerDict.TryRemove(intId, out DataMaker d);
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<SensorInfo> GetResultsAfter(string sensorId, long time)
        {
            var t = new DateTime(time).ToString(dateTimeFormat);
            var command = $"select * from SensorsData where sensor_id = {sensorId} and time >= '{t}'";

            var dataSet = new DataSet();
            using (var adapter = new SqlDataAdapter(command, connectionString))
            {
                adapter.Fill(dataSet);
            }

            return from c in dataSet.Tables[0].AsEnumerable()
                   orderby c["time"]
                   select new SensorInfo
                   {
                       SensorId = (int)c["sensor_id"],
                       Value = (decimal)c["value"],
                       Anomaly = (decimal)c["anomaly"],
                       TimeTicks = ((DateTime)c["time"]).Ticks
                   };
        }

        public IEnumerable<int> GetSensorsIdList()
        {
            return forestDict.Keys;
        }

    }
}
