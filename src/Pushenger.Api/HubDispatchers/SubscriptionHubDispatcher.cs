using Microsoft.AspNetCore.SignalR;
using Pushenger.Api.Dto.Request.Hubs.Subscription;
using Pushenger.Api.Hubs;
using Pushenger.Api.IHubDispatchers;
using Pushenger.Core.Entities;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities.Result;
using System.Threading.Tasks;

namespace Pushenger.Api.HubDispatchers
{
    /// <summary>
    /// Bildirim gönderme servisi
    /// </summary>
    public class SubscriptionHubDispatcher : ISubscriptionHubDispatcher
    {
        readonly IHubContext<SubscriptionHub> hubContext;
        readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="_hubContext"></param>
        /// <param name="_unitOfWork"></param>
        public SubscriptionHubDispatcher(
            IHubContext<SubscriptionHub> _hubContext,
            IUnitOfWork _unitOfWork
            )
        {
            hubContext = _hubContext;
            unitOfWork = _unitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        public async Task SendNotification(Notification notification)
        {
            NotificationViewModel viewModel = new NotificationViewModel() 
            {
                UniqueKey = notification.UniqueKey,
                Title = notification.Title,
                Body = notification.Body,
                Badge = notification.Badge,
                Image = notification.Image
            };
            if (notification.TopicId > 0)
            {
                IDataResult<Topic> topicExists = unitOfWork.TopicRepository.Get(notification.TopicId);
                if (!topicExists.Success)
                    return;
                Topic topic = topicExists.Data;
                await hubContext.Clients.Group(topic.UniqueKey).SendAsync("onmessage", viewModel);
            }
            else
            {
                await hubContext.Clients.Client(notification.To).SendAsync("onmessage", viewModel);
            }
        }
    }
}
