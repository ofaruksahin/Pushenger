using Microsoft.AspNetCore.SignalR;
using Pushenger.Api.Dto.Request.Hubs.Subscription;
using Pushenger.Core.Entities;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using Pushenger.Core.Utilities.Result;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Pushenger.Api.Hubs
{
    /// <summary>
    /// Bildirim sistemine abone olmak için kullanılır
    /// </summary>
    public class SubscriptionHub : BaseHub
    {
        public SubscriptionHub(IUnitOfWork _unitOfWork)
            : base(_unitOfWork)
        {
        }

        public override Task OnConnectedAsync()
        {
            SubscriptionOnConnected onConnectedQueries = Query.Map<SubscriptionOnConnected>();
            SubscriptionOnConnectedValidator onConnectedValidator = new SubscriptionOnConnectedValidator();
            if (!onConnectedValidator.Validate(onConnectedQueries).IsValid)
                HttpContext.Abort();

            IDataResult<Project> projectExists = unitOfWork.ProjectRepository.GetProjectWithUniqueKey(onConnectedQueries.ProjectKey);

            if (!projectExists.Success)
                HttpContext.Abort();

            Project project = projectExists.Data;

            if (String.IsNullOrEmpty(onConnectedQueries.OldConnectionId)) // Yeni bir subscription işlemi
            {
                Subscription subscription = new Subscription()
                {
                    CompanyId = project.CompanyId,
                    ProjectId = project.Id
                };

                if (String.IsNullOrEmpty(onConnectedQueries.TopicKey)) // Default Topic
                {
                    IDataResult<Topic> defaultTopicExists = unitOfWork.TopicRepository.GetDefaultTopic(project.Id);
                    if (!defaultTopicExists.Success)
                        HttpContext.Abort();

                    Topic topic = defaultTopicExists.Data;

                    subscription.TopicId = topic.Id;
                }
                else //Another Topic
                {
                    IDataResult<Topic> topicExists = unitOfWork.TopicRepository.GetTopicWithUniqueKey(onConnectedQueries.TopicKey);
                    if (!topicExists.Success)
                        HttpContext.Abort();

                    Topic topic = topicExists.Data;

                    if (project.Id != topic.ProjectId)
                        HttpContext.Abort();

                    subscription.TopicId = topic.Id;
                }

                subscription.ConnectionId = ConnectionId;
                subscription.Os = onConnectedQueries.Os;
                subscription.OsVersion = onConnectedQueries.OsVersion;
                subscription.App = onConnectedQueries.App;
                subscription.AppVersion = onConnectedQueries.AppVersion;

                IResult isSubscribed = unitOfWork.SubscriptionRepository.Insert(subscription);
                if (!isSubscribed.Success)
                    HttpContext.Abort();

                unitOfWork.Commit();
            }
            else // Daha önce subscribe olmuş birisi
            {

            }


            Clients.Caller.SendAsync("subscribed", ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
