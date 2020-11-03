using Pushenger.Notification.Service.Core;

namespace Pushenger.Notification.Service
{
    class Program
    {
        static void Main(string[] args)
        {            
            INotificationServiceHosting notificationServiceHosting = new NotificationServiceHosting();
            SampleNotificationService sampleNotificationService = new SampleNotificationService("a0981ee8-6fce-4407-988c-60a58c6814df", "f4b81aab-b512-4633-aaf4-29334d2ba87a",SupportedLanguage.TR);
            notificationServiceHosting.RegisterService(sampleNotificationService);
            notificationServiceHosting.Wait();
        }
    }
}
