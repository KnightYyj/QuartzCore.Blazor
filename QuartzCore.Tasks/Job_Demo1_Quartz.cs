using Quartz;
using QuartzCore.Common.Helper;
using QuartzCore.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.Tasks
{
    public class Job_Demo1_Quartz : JobBase, IJob
    {
        IQzRunLogService _qzRunLogService;
        ITasksQzService _tasksQzServices;
        public Job_Demo1_Quartz(ITasksQzService tasksQzServices, IQzRunLogService qzRunLogService) : base(qzRunLogService)
        {
            _tasksQzServices = tasksQzServices;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync("开始" + 1);
            var jobKey = context.JobDetail.Key;
            var jobId = jobKey.Name;
            var executeLog = await ExecuteJob(context, async () => await Run(context, jobId.ObjToInt()));

            // 也可以通过数据库配置，获取传递过来的参数
            JobDataMap data = context.JobDetail.JobDataMap;
        }
        public async Task<string> Run(IJobExecutionContext context, int jobid)
        {
            if (jobid > 0)
            {
                var model = await _tasksQzServices.GetAsync(jobid);
                if (model != null)
                {
                    model.RunTimes += 1;
                    await _tasksQzServices.UpdateAsync(model);
                }
            }
            await Console.Out.WriteLineAsync("你好!我是测试Demo1");
            return "你好!我是测试Demo1";
        }
    }

}
