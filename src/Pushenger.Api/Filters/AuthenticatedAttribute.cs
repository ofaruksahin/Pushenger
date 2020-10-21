using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using Pushenger.Api.Dto.Response.User;
using Pushenger.Api.Models;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using Pushenger.Core.Utilities.Result;
using Pushenger.Service;
using System;
using System.Linq;
using System.Net;

namespace Pushenger.Api.Filters
{
    public class AuthenticatedAttribute : Attribute, IActionFilter
    {
        IUnitOfWork unitOfWork;
        IStringLocalizer<BaseResource> localizer;

        public AuthenticatedAttribute()
        {
            unitOfWork = new UnitOfWork();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.Filters.Any(x => x.GetType() == typeof(AllowAnonymousFilter))) 
            {
                localizer = (IStringLocalizer<BaseResource>)context.HttpContext.RequestServices.GetService(typeof(IStringLocalizer<BaseResource>));
                var token = JWTManager.GetToken(context.HttpContext);
                if (String.IsNullOrWhiteSpace(token))
                    UnAuthorized(context);
                IDataResult<Core.Entities.User> existUser = unitOfWork.UserRepository.CheckToken(token);
                if (!existUser.Success)
                    UnAuthorized(context);
            }
        }

        private async void UnAuthorized(ActionExecutingContext context)
        {
            BaseResult<LoginResponse> baseResult = new BaseResult<LoginResponse>();
            baseResult.Message = localizer[Constant.TokenNotFound];
            baseResult.StatusCode = HttpStatusCode.Unauthorized;
            context.Result = new UnauthorizedObjectResult(baseResult);
        }
    }   
}
