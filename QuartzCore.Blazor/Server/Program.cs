using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuartzCore.Common;
using QuartzCore.Data.Freesql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzCore.Blazor.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            var builder = new ConfigurationBuilder()
   .SetBasePath(Directory.GetCurrentDirectory());
#if DEBUG
            Global.Config =
                 builder
                .AddJsonFile("appsettings.Development.json")
                .AddEnvironmentVariables()
                .Build();
#else
            Global.Config = builder.AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();
#endif
           
            var host = CreateHostBuilder(args).Build();

            var sp = host.Services;
            var res = sp.CreateScope().ServiceProvider.GetService<FreeSqlContext>() ;

            host.Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseUrls("http://*:5001");
                });
    }
}
