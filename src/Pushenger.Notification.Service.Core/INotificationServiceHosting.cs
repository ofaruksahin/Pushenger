using Pushenger.Notification.Service.Core;

namespace Pushenger.Notification.Service
{
    public interface INotificationServiceHosting
    {
        void RegisterService<NotificationService>(NotificationService service) where NotificationService : INotificationService;
        void Wait();
    }
}
