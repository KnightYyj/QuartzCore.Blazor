using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using QuartzCore.Blazor.Server.Middlewares;
using QuartzCore.Common;
using QuartzCore.Data.Entity;
using QuartzCore.Data.Freesql;
using QuartzCore.IService;
using QuartzCore.Service;
using QuartzCore.Tasks;
using System.Linq;

namespace QuartzCore.Blazor.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFreeSqlDbContext();
            services.AddAutoMapper(typeof(AutomapperConfig));

            services.AddScoped<IAppService, AppService>();
            services.AddScoped<ITasksQzService, TasksQzService>();
            services.AddScoped<IQzRunLogService, QzRunLogService>();


            //services.AddSingleton(new JobQueue<QzRunLogEntity>());
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddTransient<Job_HttpApi_Quartz>();
            services.AddTransient<Job_Demo1_Quartz>();

            services.AddSingleton<ISchedulerCenter, SchedulerCenterServer>();


            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ITasksQzService tasksQzService, ISchedulerCenter schedulerCenter)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseQuartzJobMildd(tasksQzService, schedulerCenter);
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
