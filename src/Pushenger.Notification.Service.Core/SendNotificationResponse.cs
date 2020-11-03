using System.Net;

namespace Pushenger.Notification.Service.Core
{
    public class SendNotificationResponse
    {
        public DataObject Data { get; set; }
        public class DataObject
        {

        }

        public string Message { get; set; }
        public HttpStatusCode StatusCode{ get; set; }
    }
}
