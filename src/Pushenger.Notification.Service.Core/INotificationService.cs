using System.Threading.Tasks;

namespace Pushenger.Notification.Service.Core
{
    public interface INotificationService
    {
        Task<SendNotificationResponse> SendNotification(SendNotificationModel sendNotificationModel);
    }
}
