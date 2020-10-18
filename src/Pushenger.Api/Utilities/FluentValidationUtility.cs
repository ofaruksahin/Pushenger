using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Pushenger.Api.Utilities
{
    public static class FluentValidationUtility
    {
        public static IMvcBuilder AddFluent(this IMvcBuilder mvc)
        {
            mvc.AddFluentValidation(fv =>
            {

            });
            return mvc;
        }
    }
}
