using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Pushenger.Api.Dto.Request.Notification;
using Pushenger.Api.Dto.Response.Notification;
using Pushenger.Api.Filters;
using Pushenger.Api.IHubDispatchers;
using Pushenger.Core.Entities;
using Pushenger.Core.Enums;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using Pushenger.Core.Utilities.Result;
using System;

namespace Pushenger.Api.Controllers
{
    /// <summary>
    /// Bildirim göndermek için kullanılır.
    /// </summary>
    public class NotificationController : BaseController
    {
        readonly IStringLocalizer<NotificationResource> localizer;
        readonly IStringLocalizer<ProjectResource> projectLocalizer;
        readonly IStringLocalizer<TopicResource> topicLocalizer;
        readonly IStringLocalizer<SubscriptionResource> subscriptionLocalizer;
        readonly ISubscriptionHubDispatcher subscriptionDispatcher;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="_unitOfWork"></param>
        /// <param name="_mapper"></param>
        /// <param name="_baseLocalizer"></param>
        /// <param name="_localizer"></param>
        /// <param name="_projectLocalizer"></param>
        /// <param name="_topicLocalizer"></param>
        /// <param name="_subscriptionLocalizer"></param>
        public NotificationController(
            IUnitOfWork _unitOfWork,
            IMapper _mapper,
            IStringLocalizer<BaseResource> _baseLocalizer,
            IStringLocalizer<NotificationResource> _localizer,
            IStringLocalizer<ProjectResource> _projectLocalizer,
            IStringLocalizer<TopicResource> _topicLocalizer,
            IStringLocalizer<SubscriptionResource> _subscriptionLocalizer,
            ISubscriptionHubDispatcher _subscriptionHubDispatcher
            )
            : base(_unitOfWork, _mapper, _baseLocalizer)
        {
            localizer = _localizer;
            projectLocalizer = _projectLocalizer;
            topicLocalizer = _topicLocalizer;
            subscriptionLocalizer = _subscriptionLocalizer;
            subscriptionDispatcher = _subscriptionHubDispatcher;
        }

        [NonAction]
        public object GetProject<T>(T response)
        {
            IsNotificationServiceModel notificationServiceModel = HttpContext.Request.Headers.Map<IsNotificationServiceModel>();
            IsNotificationServiceModelValidator validator = new IsNotificationServiceModelValidator();
            if (!validator.Validate(notificationServiceModel).IsValid)
                return NotFound(response);
            IDataResult<Project> projectExists = unitOfWork.ProjectRepository.GetProjectWithUniqueKey(notificationServiceModel.UniqueKey, notificationServiceModel.SenderKey);
            if (!projectExists.Success)
                return NotFound(response, projectLocalizer[projectExists.Message]);
            return projectExists.Data;
        }

        /// <summary>
        /// Bildirim güncellemek için kullanılır.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("send")]
        [IsNotificationService]
        public IActionResult Send([FromBody] SendNotificationRequestDTO dto)
        {
            SendNotificationResponse response = new SendNotificationResponse();
            object projectExists = GetProject(response);
            if (projectExists.GetType().BaseType == typeof(ObjectResult))
                return (IActionResult)projectExists;
            Project project = (Project)projectExists;
            Notification notification = new Notification()
            {
                CompanyId = project.CompanyId,
                ProjectId = project.Id,
                UniqueKey = Guid.NewGuid().ToString(),
                Title = dto.Title,
                Body = dto.Body,
                Badge = dto.Badge,
                Image = dto.Image,
                To = dto.To,
                ExpiryDate = DateTime.Now.AddDays(2),
                CreationDate = DateTime.Now,
                CreatorId = 0,
                Status = enumRecordStatus.Active
            };
            if (!String.IsNullOrEmpty(dto.TopicKey)) // Topic bildirim gönderme
            {
                IDataResult<Topic> topicExists = unitOfWork.TopicRepository.GetTopicWithUniqueKey(dto.TopicKey);
                if (!topicExists.Success)
                    return NotFound(response, topicLocalizer[topicExists.Message]);
                Topic topic = topicExists.Data;
                notification.TopicId = topic.Id;
            }
            else // Tekli kişiye bildirim gönderme
            {
                IDataResult<Subscription> subscriptionExists = unitOfWork.SubscriptionRepository.GetSubscriptionConnectionIdAndOldConnectionId(dto.To);
                if (!subscriptionExists.Success)
                    return NotFound(response,subscriptionLocalizer[subscriptionExists.Message]);                
                Subscription subscription = subscriptionExists.Data;
                if (subscription.ProjectId != project.Id)
                    return NotFound(response, subscriptionLocalizer[Constant.SubscriptionMessages.SubscriptionNotFound]);
                notification.To = subscription.ConnectionId;
            }
            IResult notificationInserted = unitOfWork.NotificationRepository.Insert(notification);
            if (!notificationInserted.Success)
                return NotFound(response,localizer[notificationInserted.Message]);
            unitOfWork.Commit();
            subscriptionDispatcher.SendNotification(notification);
            response.NotificationUniqueKey = notification.UniqueKey;
            return Ok(response);
        }
    }
}
