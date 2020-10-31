using AutoMapper.Configuration;
using environment.net.core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pushenger.Api.Hubs;
using StackExchange.Redis;
using System;
using System.Net;

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
            IEnvironmentManager environmentManager = EnvironmentManager.Instance;
            Microsoft.Extensions.Configuration.IConfiguration configuration = environmentManager.GetConfiguration();
            var connectionString = (string)configuration.GetValue(typeof(string), "redis_connection");
            services.AddSignalR().AddStackExchangeRedis(connectionString);
            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder MapHubs(this IApplicationBuilder app)
        {
            app.UseSignalR(routes =>
            {
                routes.MapHub<SubscriptionHub>("/subscription");
            });
            return app;
        }
    }
}
