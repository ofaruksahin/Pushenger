using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pushenger.Api.Utilities;

namespace Pushenger.Api
{
    /// <summary>
    /// Startup Class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration Object
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Service Configuration
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            var mvcBuilder = services.AddControllers(options=> {
                options.EnableEndpointRouting = false;
            }).SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });            

            mvcBuilder.AddFluent();
            services.AddCors();

            services.AddCulture();
            services.AddInjection();
            services.AddMapper();
            services.EnableSignalR();
        }


        /// <summary>
        /// Configure Application
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(x => x
              .AllowAnyOrigin()
              .AllowAnyMethod()
             .AllowAnyHeader());

            app.UseCulture();
            app.UseMvc();
            app.MapHubs();
        }
    }
}
