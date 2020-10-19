using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace Pushenger.Api.Utilities
{
    /// <summary>
    /// Localization entegrasyonu
    /// </summary>
    public static class CultureUtility
    {        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCulture(this IServiceCollection services)
        {
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "";
            });
            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCulture(this IApplicationBuilder app)
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("tr")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            return app;
        }
    }
}
