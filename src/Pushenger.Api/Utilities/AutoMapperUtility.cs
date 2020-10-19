using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Pushenger.Api.Utilities
{
    /// <summary>
    /// AutoMapper entegrasyonu
    /// </summary>
    public static class AutoMapperUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            return services;
        }
    }
}
