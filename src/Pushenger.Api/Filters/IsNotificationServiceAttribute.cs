using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using Pushenger.Api.Dto.Request.Notification;
using Pushenger.Api.Models;
using Pushenger.Core.Entities;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using Pushenger.Core.Utilities.Result;
using Pushenger.Service;
using System;
using System.Net;

namespace Pushenger.Api.Filters
{
    public class IsNotificationServiceAttribute : Attribute, IActionFilter
    {

        IStringLocalizer localizer;        

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            IsNotificationServiceModel notificationServiceModel = context.HttpContext.Request.Headers.Map<IsNotificationServiceModel>();
            localizer = (IStringLocalizer<BaseResource>)context.HttpContext.RequestServices.GetService(typeof(IStringLocalizer<BaseResource>));
            IsNotificationServiceModelValidator validator = new IsNotificationServiceModelValidator();
            if (!validator.Validate(notificationServiceModel).IsValid)
                UnAuthorized(context);

            using (IUnitOfWork unitOfWork = new UnitOfWork())
            {
                IDataResult<Project> projectExists = unitOfWork.ProjectRepository.GetProjectWithUniqueKey(notificationServiceModel.UniqueKey, notificationServiceModel.SenderKey);
                if (!projectExists.Success)
                    UnAuthorized(context);
            }
        }

        private async void UnAuthorized(ActionExecutingContext context)
        {
            BaseResult<bool> baseResult = new BaseResult<bool>();
            baseResult.Message = localizer[Constant.UnAuthorized];
            baseResult.StatusCode = HttpStatusCode.Unauthorized;
            context.Result = new UnauthorizedObjectResult(baseResult);
        }
    }
}
