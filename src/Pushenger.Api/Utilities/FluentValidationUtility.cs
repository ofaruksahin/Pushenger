using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Pushenger.Api.Dto.Request.Company;
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
            });
            return mvc;
        }
    }
}
