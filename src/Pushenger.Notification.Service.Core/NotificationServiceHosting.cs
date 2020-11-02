using Pushenger.Notification.Service.Core;
using System.Threading;

namespace Pushenger.Notification.Service
{
    public class NotificationServiceHosting : INotificationServiceHosting
    {
        public void RegisterService<NotificationService>(NotificationService service) 
            where NotificationService : INotificationService
        {
           
        }

        public void Wait()
        {
            while (true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
