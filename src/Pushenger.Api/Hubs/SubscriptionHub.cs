using Microsoft.AspNetCore.SignalR;
using Pushenger.Api.Dto.Request.Hubs.Subscription;
using Pushenger.Core.Entities;
using Pushenger.Core.Enums;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using Pushenger.Core.Utilities.Result;
using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Abone olma işlemi
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            SubscriptionOnConnected onConnectedQueries = Query.Map<SubscriptionOnConnected>();
            SubscriptionOnConnectedValidator onConnectedValidator = new SubscriptionOnConnectedValidator();
            if (!onConnectedValidator.Validate(onConnectedQueries).IsValid)
            {
                HttpContext.Abort(); return base.OnConnectedAsync();
            }

            IDataResult<Project> projectExists = unitOfWork.ProjectRepository.GetProjectWithUniqueKey(onConnectedQueries.ProjectKey);

            if (!projectExists.Success)
            {
                HttpContext.Abort(); return base.OnConnectedAsync();
            }

            Project project = projectExists.Data;

            Topic subscriptionTopic = null;

            if (String.IsNullOrEmpty(onConnectedQueries.OldConnectionId) || onConnectedQueries.OldConnectionId == "null") // Yeni bir subscription işlemi
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
                    {
                        HttpContext.Abort(); return base.OnConnectedAsync();
                    }

                    Topic topic = defaultTopicExists.Data;

                    subscription.TopicId = topic.Id;
                    subscriptionTopic = topic;
                }
                else //Another Topic
                {
                    IDataResult<Topic> topicExists = unitOfWork.TopicRepository.GetTopicWithUniqueKey(onConnectedQueries.TopicKey);
                    if (!topicExists.Success)
                    {
                        HttpContext.Abort(); return base.OnConnectedAsync();
                    }

                    Topic topic = topicExists.Data;

                    if (project.Id != topic.ProjectId)
                    {
                        HttpContext.Abort(); return base.OnConnectedAsync();
                    }

                    subscription.TopicId = topic.Id;
                    subscriptionTopic = topic;
                }

                subscription.ConnectionId = ConnectionId;
                subscription.Os = onConnectedQueries.Os;
                subscription.OsVersion = onConnectedQueries.OsVersion;
                subscription.App = onConnectedQueries.App;
                subscription.AppVersion = onConnectedQueries.AppVersion;

                IResult isSubscribed = unitOfWork.SubscriptionRepository.Insert(subscription);
                if (!isSubscribed.Success)
                {
                    HttpContext.Abort(); return base.OnConnectedAsync();
                }

                unitOfWork.Commit();
            }
            else // Daha önce subscribe olmuş birisi
            {
                IDataResult<Subscription> subscriptionExists = unitOfWork.SubscriptionRepository.GetSubscriptionWithConnectionId(onConnectedQueries.OldConnectionId);
                if (!subscriptionExists.Success)
                {
                    HttpContext.Abort(); return base.OnConnectedAsync();
                }
                Subscription subscription = subscriptionExists.Data;
                subscription.OldConnectionId = subscription.ConnectionId;
                subscription.ConnectionId = ConnectionId;

                IResult subscriptionUpdated = unitOfWork.SubscriptionRepository.Update(subscription);
                if (!subscriptionUpdated.Success)
                {
                    HttpContext.Abort(); return base.OnConnectedAsync();
                }

                IDataResult<Topic> topicExists = unitOfWork.TopicRepository.Get(subscription.TopicId);
                if (!topicExists.Success)
                {
                    HttpContext.Abort(); return base.OnConnectedAsync();
                }

                subscriptionTopic = topicExists.Data;
                unitOfWork.Commit();
            }

            Groups.AddToGroupAsync(ConnectionId, subscriptionTopic.UniqueKey);
            Clients.Caller.SendAsync("subscribed", ConnectionId);
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// Bildirimi teslim alma işlemi
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task OnReceive(OnReceiveDto dto)
        {
            IDataResult<Subscription> subscriptionExists = unitOfWork.SubscriptionRepository.GetSubscriptionConnectionIdAndOldConnectionId(ConnectionId);
            if (!subscriptionExists.Success)
                return;
            IDataResult<Notification> notificationExists = unitOfWork.NotificationRepository.GetNotificationRedis(dto.UniqueKey);
            if (!notificationExists.Success)
                return;
            Subscription subscription = subscriptionExists.Data;
            Notification notification = notificationExists.Data;
            NotificationDetail notificationDetail = new NotificationDetail() 
            { 
                NotificationId = notification.Id,
                SubscriptionId = subscription.Id,
                ReceiveDate = DateTime.Now
            };
            unitOfWork.NotificationRepository.InsertNotificationDetailRedis(notificationDetail);
            unitOfWork.Commit();
        }

        /// <summary>
        /// Eski bildirimlerini almak için kullanılır.
        /// </summary>
        /// <returns></returns>
        public async Task GetOldNotification()
        {
            IDataResult<Subscription> subscriptionExists = unitOfWork.SubscriptionRepository.GetSubscriptionConnectionIdAndOldConnectionId(ConnectionId);
            if (!subscriptionExists.Success)
                return;
            IDataResult<List<Notification>> notificationsExists = unitOfWork.NotificationRepository.GetNotificationsRedis();
            if (!notificationsExists.Success)
                return;
            Subscription subscription = subscriptionExists.Data;
            List<Notification> notifications = notificationsExists.Data;
            notifications.RemoveAll(x => x.TopicId != subscription.TopicId && String.IsNullOrEmpty(x.To));
            notifications.RemoveAll(x => !String.IsNullOrEmpty(x.To) && (x.To != subscription.ConnectionId && x.To == subscription.OldConnectionId));            
            if (!notifications.Any())
                return;
            List<Notification> userNotifications = new List<Notification>();
            foreach (var notification in notifications)
            {
                IDataResult<List<NotificationDetail>> notificationDetailsExists = unitOfWork.NotificationRepository.GetNotificationDetailsRedis(notification.Id);                
                List<NotificationDetail> notificationDetails = notificationDetailsExists.Data;                
                if (notificationDetails == null || !notificationDetails.Any(x => x.SubscriptionId == subscription.Id))
                    userNotifications.Add(notification);
            }
            if (userNotifications.Any())
               await Clients.Caller.SendAsync("getOldNotifications", userNotifications);
        }

        public async Task UnSubscribe()
        {
            IDataResult<Subscription> subscriptionExists = unitOfWork.SubscriptionRepository.GetSubscriptionConnectionIdAndOldConnectionId(ConnectionId);
            if (!subscriptionExists.Success)
                return;
            Subscription subscription = subscriptionExists.Data;
            IDataResult<Topic> topicExists = unitOfWork.TopicRepository.Get(subscription.TopicId);
            if (!topicExists.Success)
                return;
            Topic topic = topicExists.Data;
            subscription.Status = enumRecordStatus.InActive;
            IResult unSubscribed = unitOfWork.SubscriptionRepository.Update(subscription);
            if (!unSubscribed.Success)
                return;
            unitOfWork.Commit();
            await Groups.RemoveFromGroupAsync(ConnectionId, topic.UniqueKey);
            await Clients.Caller.SendAsync("unSubscribed", topic.UniqueKey);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
