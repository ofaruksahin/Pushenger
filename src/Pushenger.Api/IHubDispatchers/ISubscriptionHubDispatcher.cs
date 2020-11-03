using Pushenger.Core.Entities;
using System.Threading.Tasks;

namespace Pushenger.Api.IHubDispatchers
{
    public interface ISubscriptionHubDispatcher 
    {
        Task SendNotification(Notification notification);
    }
}
