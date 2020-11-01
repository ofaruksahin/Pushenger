using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Pushenger.Api.Dto.Request.Company;
using Pushenger.Api.Dto.Request.Hubs.Subscription;
using Pushenger.Api.Dto.Request.Project;
using Pushenger.Api.Dto.Request.ProjectUser;
using Pushenger.Api.Dto.Request.Topic;
using Pushenger.Api.Dto.Request.User;

namespace Pushenger.Api.Utilities
{
    /// <summary>
    /// Fluent Validation entegrasyonu
    /// </summary>
    public static class FluentValidationUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mvc"></param>
        /// <returns></returns>
        public static IMvcBuilder AddFluent(this IMvcBuilder mvc)
        {
            mvc.AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<InsertCompanyRequestValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<UpdateUserRequestValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<UpdateCompanyRequestValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<InsertUserRequestValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<UpdateUserTypeRequestValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<InsertProjectRequestValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<InsertTopicRequestValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<UpdateTopicRequestValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<InsertProjectUserRequestDTO>();
                fv.RegisterValidatorsFromAssemblyContaining<SubscriptionOnConnectedValidator>();
            });
            return mvc;
        }
    }
}
