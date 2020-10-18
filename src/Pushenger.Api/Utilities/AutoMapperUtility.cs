using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Pushenger.Api.Utilities
{
    public static class AutoMapperUtility
    {
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            return services;
        }
    }
}
