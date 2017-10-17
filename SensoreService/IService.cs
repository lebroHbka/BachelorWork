using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace SensorService
{
    [ServiceContract]
    public interface IService
    {

        [OperationContract]
        [WebInvoke(Method = "PUT",
                   UriTemplate = "/sensors/add/{id}",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Bare)]
        bool AddNewSensore(string id, IEnumerable<decimal> feedData);

        [OperationContract]
        [WebInvoke(Method = "POST",
                   UriTemplate = "/sensors/{id}",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Bare)]
        bool SendData(string id, IEnumerable<SensorDataPoint> data);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
                   UriTemplate = "/sensors/delete/{id}",
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Bare)]
        bool DeleteSensor(string id);


        [OperationContract]
        [WebGet(UriTemplate = "/results/{sensorId}?aftertime={time}",
                ResponseFormat = WebMessageFormat.Json,
                BodyStyle = WebMessageBodyStyle.Bare)]
        IEnumerable<SensorInfo> GetResultsAfter(string sensorId, long time);

        [OperationContract]
        [WebGet(UriTemplate = "/sensors/list",
                ResponseFormat = WebMessageFormat.Json,
                BodyStyle = WebMessageBodyStyle.Bare)]
        IEnumerable<int> GetSensorsIdList();
    }

    [DataContract]
    public class SensorInfo
    {
        [DataMember]
        public int SensorId { get; set; }

        [DataMember]
        public decimal Value { get; set; }

        [DataMember]
        public decimal Anomaly { get; set; }

        [DataMember]
        public long TimeTicks { get; set; }
    }

    [DataContract]
    public class SensorDataPoint
    {
        [DataMember]
        public decimal Value { get; set; }

        [DataMember]
        public long TimeTicks { get; set; }
    }

}
