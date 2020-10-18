using Microsoft.Extensions.DependencyInjection;
using Pushenger.Core.Interfaces;
using Pushenger.Service;

namespace Pushenger.Api.Utilities
{
    public static class DependecyInjectUtility
    {
        public static IServiceCollection AddInjection(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDbContext, DbContext>();            
            return services;
        }
    }
}
