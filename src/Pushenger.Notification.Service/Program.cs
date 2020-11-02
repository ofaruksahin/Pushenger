namespace Pushenger.Notification.Service
{
    class Program
    {
        static void Main(string[] args)
        {            
            INotificationServiceHosting notificationServiceHosting = new NotificationServiceHosting();            
            notificationServiceHosting.Wait();
        }
    }
}
