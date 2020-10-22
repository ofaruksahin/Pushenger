using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using Pushenger.Api.Models;
using Pushenger.Core.Enums;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using Pushenger.Service;
using System;
using System.Net;

namespace Pushenger.Api.Filters
{
    public class IsOwnerAttribute : Attribute, IActionFilter
    {
        IStringLocalizer localizer;
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            using (IUnitOfWork unitOfWork = new UnitOfWork())
            {
                localizer = (IStringLocalizer<BaseResource>)context.HttpContext.RequestServices.GetService(typeof(IStringLocalizer<BaseResource>));
                int userId = JWTManager.GetUserId(context.HttpContext, unitOfWork);
                Core.Entities.User user = JWTManager.GetUser(userId, unitOfWork);
                if (user.UserTypeId != enumUserType.Owner)
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
