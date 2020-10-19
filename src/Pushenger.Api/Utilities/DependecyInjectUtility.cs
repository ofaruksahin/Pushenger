using Microsoft.Extensions.DependencyInjection;
using Pushenger.Core.Interfaces;
using Pushenger.Service;
using Pushenger.Service.Repositories;

namespace Pushenger.Api.Utilities
{
    /// <summary>
    /// DI katmanı
    /// </summary>
    public static class DependecyInjectUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddInjection(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDbContext, DbContext>();            
            services.AddScoped<ICompanyRepository, CompanyRepository>();            
            services.AddScoped<IUserRepository, UserRepository>();            
            return services;
        }
    }
}
