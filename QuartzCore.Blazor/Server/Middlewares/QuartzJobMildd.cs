using Microsoft.AspNetCore.Builder;
using QuartzCore.IService;
using QuartzCore.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzCore.Blazor.Server.Middlewares
{
    /// <summary>
    /// Quartz 启动服务
    /// </summary>
    public static class QuartzJobMildd
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(QuartzJobMildd));
        public static void UseQuartzJobMildd(this IApplicationBuilder app, ITasksQzService tasksQzService, ISchedulerCenter schedulerCenter)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            try
            {
                    var allQzServices = tasksQzService.GetAsync().Result;
                    foreach (var item in allQzServices)
                    {
                        if (item.IsStart)
                        {
                            var ResuleModel = schedulerCenter.AddScheduleJobAsync(item).Result;
                            if (ResuleModel.success)
                            {
                                Console.WriteLine($"QuartzNetJob{item.Name}启动成功！");
                            }
                            else
                            {
                                Console.WriteLine($"QuartzNetJob{item.Name}启动失败！错误信息：{ResuleModel.msg}");
                            }
                        }
                    }
            }
            catch (Exception e)
            {
                //log.Error($"An error was reported when starting the job service.\n{e.Message}");
                throw;
            }
        }
    }
}
