using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Pushenger.Api.Utilities
{
    /// <summary>
    /// SignalR
    /// </summary>
    public static class HubMappingUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection EnableSignalR(this IServiceCollection services)
        {
            services.AddSignalR().AddStackExchangeRedis("");
            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder MapHubs(this IApplicationBuilder app)
        {
            return app;
        }
    }
}
