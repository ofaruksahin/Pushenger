using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using Pushenger.Api.Dto.Request.Notification;
using Pushenger.Api.Models;
using Pushenger.Core.Utilities;
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

            //TODO: Proje kontrolüne devam etmek için kullanılır.
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
