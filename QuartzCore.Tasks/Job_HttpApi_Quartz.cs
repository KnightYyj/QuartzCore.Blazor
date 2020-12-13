 using Quartz;
using QuartzCore.Common.Helper;
using QuartzCore.Data.Entity;
using QuartzCore.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.Tasks
{
    public class Job_HttpApi_Quartz : JobBase, IJob
    {
        IQzRunLogService _qzRunLogService;
        ITasksQzService _tasksQzServices;
        public Job_HttpApi_Quartz(ITasksQzService tasksQzServices, IQzRunLogService qzRunLogService) : base(qzRunLogService)
        {
            _tasksQzServices = tasksQzServices;
        }



        public async Task Execute(IJobExecutionContext context)
        {
            DateTime dateTime = DateTime.Now;
            var jobKey = context.JobDetail.Key;
            var jobId = jobKey.Name;
            var model = await _tasksQzServices.GetAsync(jobId.ObjToInt());
            var executeLog = await ExecuteJob(context, async () => await Run(context, jobId.ObjToInt()));
        }
        public async Task<string> Run(IJobExecutionContext context, int jobid)
        {
            var msg = "";
            if (jobid > 0)
            {
                var model = await _tasksQzServices.GetAsync(jobid);
                if (model != null)
                {
                    if (model.IsApiUrl)
                    {
                        if (model.MethodType == 1) //model.MethodType?.ToUpper() == "GET"
                        {
                            var rep = await HttpUtil.HttpGetAsync(model.ApiUrl);
                            msg = rep;
                        }
                        else
                        {
                            var postData = JsonHelper.ToJson(model.RequestValue);
                            var rep = await HttpUtil.HttpPostAsync(model.ApiUrl, postData, null, 30000);
                            msg = rep;
                        }
                    }
                    model.RunTimes += 1;
                    await _tasksQzServices.UpdateAsync(model);
                }
            }
            return msg;
        }

    }
}
