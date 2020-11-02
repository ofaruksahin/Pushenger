namespace Pushenger.Notification.Service.Core
{
    public class NotificationService : INotificationService
    {
        string UniqueKey { get;  set; }
        string SenderKey { get;  set; }

        public NotificationService(string _UniqueKey,string _SenderKey)
        {
            UniqueKey = _UniqueKey;
            SenderKey = _SenderKey;
        }

        public void InitializeService()
        {
            
        }
    }
}
