namespace Pushenger.Notification.Service.Core
{
    public class SendNotificationModel
    { 
        public string Title { get; set; } 
        public string Body { get; set; }   
        public string Badge { get; set; }     
        public string Image { get; set; }    
        public string TopicKey { get; set; }        
        public string To { get; set; }
    }
}
