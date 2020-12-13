using Quartz;
using QuartzCore.Common;
using QuartzCore.Common.Helper;
using QuartzCore.Data.Entity;
using QuartzCore.IService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.Tasks
{
    public class JobBase
    {
        IQzRunLogService _qzRunLogService;
         
        public JobBase(IQzRunLogService qzRunLogService )
        {
            _qzRunLogService = qzRunLogService;
          
        }

        /// <summary>
        /// 执行指定任务
        /// </summary>
        /// <param name="context"></param>
        /// <param name="action"></param>
        public async Task<string> ExecuteJob(IJobExecutionContext context, Func<Task<string>> func)
        {
            QzRunLogEntity qzlog = new QzRunLogEntity();
            qzlog.TasksQzId = context.JobDetail.Key.Name.ObjToInt();
            qzlog.AppId = context.JobDetail.Key.Group;
            qzlog.LogTime = DateTime.Now;
            qzlog.LogType = SysLogType.Normal;

            string jobHistory = $"【{DateTime.Now}】执行任务【Id：{context.JobDetail.Key.Name}，组别：{context.JobDetail.Key.Group}】";
            try
            {
                var s = context.Trigger.Key.Name;
                //记录Job时间
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var msg = await func();//执行任务
                stopwatch.Stop();
                jobHistory += $"，【执行成功】，完成时间：{stopwatch.Elapsed.TotalMilliseconds.ToString("00")}毫秒";
                //WriteLog(context.Trigger.Key.Name.Replace("-", ""), $"{context.Trigger.Key.Name}定时任务运行一切OK", "任务结束");
                qzlog.LogText =
                        $" 响应结果:[{msg}]";
                qzlog.Milliseconds = $"{stopwatch.Elapsed.TotalMilliseconds.ToString("00")}毫秒";
            }
            catch (Exception ex)
            {
                JobExecutionException e2 = new JobExecutionException(ex);
                //true  是立即重新执行任务 
                e2.RefireImmediately = true;
                //WriteErrorLog(context.Trigger.Key.Name.Replace("-", ""), $"{context.Trigger.Key.Name}任务运行异常", ex);
                jobHistory += $"，【执行失败】，异常日志：{ex.Message}";
                qzlog.LogText =
                     $"【执行失败】，异常日志：{ex.Message}";
                qzlog.LogType = SysLogType.Warn;
            }
            await _qzRunLogService.AddAsync(qzlog);
    
             Console.Out.WriteLine(jobHistory);
            return jobHistory;
        }
    }



}
