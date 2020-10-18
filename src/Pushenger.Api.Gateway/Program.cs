using environment.net.core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace Pushenger.Api.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    EnvironmentManager environmentManager = EnvironmentManager.Instance;

                    config.SetBasePath(Directory.GetCurrentDirectory())
                    .AddEnvironmentVariables();

                    if (environmentManager.IsDevelopment())
                        config.AddJsonFile("ocelot.development.json");
                    else if (environmentManager.IsStaging())
                        config.AddJsonFile("ocelot.staging.json");
                    else if (environmentManager.IsProduction())
                        config.AddJsonFile("ocelot.production.json");
                    else
                        throw new ArgumentNullException("Undefined Environment Variable");
                })            
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
