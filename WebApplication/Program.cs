using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace WebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureLogging((context, loggingBuilder) =>
            {
                //loggingBuilder.AddFilter("System", LogLevel.Warning);
                //loggingBuilder.AddFilter("Microsoft", LogLevel.Warning);

                //log4net 
                //var path = context.HostingEnvironment.ContentRootPath;
                //loggingBuilder.AddLog4Net($"{path}/Config/log4net.config");//ÅäÖÃÎÄ¼þ


                //serilog
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("Config/serilog.json")
                    .Build();
                var logger = new LoggerConfiguration()
                   .ReadFrom.Configuration(configuration)
                   .CreateLogger();

                loggingBuilder.AddSerilog(logger, dispose: true);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>().ConfigureKestrel(serverOptions =>
                {
                    serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromHours(20);
                    serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromHours(20);
                });
            });
    }
}
