using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using QuartzCore.Blazor.Shared;
using QuartzCore.Data.Entity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuartzCore.Tasks
{
    /// <summary>
    /// 任务调度管理中心
    /// </summary>
    public class SchedulerCenterServer : ISchedulerCenter
    {
        private IScheduler _scheduler;

        private readonly IJobFactory _iocjobFactory;
        private readonly ISchedulerFactory _schedulerFactory;
        public SchedulerCenterServer(IJobFactory jobFactory, ISchedulerFactory schedulerFactory)
        {
            _iocjobFactory = jobFactory ?? throw new ArgumentNullException(nameof(jobFactory));
            _schedulerFactory = schedulerFactory;
            _scheduler = GetSchedulerAsync().Result;

        }
        private async Task<IScheduler> GetSchedulerAsync()
        {
            if (_scheduler != null)
                return this._scheduler;
            else
            {
                // 从Factory中获取Scheduler实例
                return _scheduler = await _schedulerFactory.GetScheduler();
            }
        }

        #region _scheduler层级
        /// <summary>
        /// 开启任务调度
        /// </summary>
        /// <returns></returns>
        public async Task<MessageModel<string>> StartScheduleAsync()
        {
            var result = new MessageModel<string>();
            try
            {
                this._scheduler.JobFactory = this._iocjobFactory;
                if (!this._scheduler.IsStarted)
                {
                    //等待任务运行完成
                    await this._scheduler.Start();
                    await Console.Out.WriteLineAsync("任务调度开启！");
                    result.success = true;
                    result.msg = $"任务调度开启成功";
                    return result;
                }
                else
                {
                    result.success = false;
                    result.msg = $"任务调度已经开启";
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 停止任务调度
        /// </summary>
        /// <returns></returns>
        public async Task<MessageModel<string>> StopScheduleAsync()
        {
            var result = new MessageModel<string>();
            try
            {
                if (!this._scheduler.IsShutdown)
                {
                    await Task.Delay(30);
                    //等待任务运行完成
                    await this._scheduler.Shutdown();
                    await Console.Out.WriteLineAsync("任务调度停止！");
                    result.success = true;
                    result.msg = $"任务调度停止成功";
                    return result;
                }
                else
                {
                    result.success = false;
                    result.msg = $"任务调度已经停止";
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        /// <summary>
        /// 添加一个计划任务（映射程序集指定IJob实现类）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tasksQz"></param>
        /// <returns></returns>
        public async Task<MessageModel<string>> AddScheduleJobAsync(TasksQzEntity tasksQz)
        {
            var result = new MessageModel<string>();

            if (tasksQz != null)
            {
                try
                {
                    JobKey jobKey = new JobKey(tasksQz.Id.ToString(), tasksQz.AppId);
                    if (await _scheduler.CheckExists(jobKey))
                    {

                        result.success = false;
                        result.msg = $"该任务计划已经在执行:【{tasksQz.Name}】,请勿重复启动！";
                        return result;
                    }

                    #region 通过反射获取程序集类型和类   

                    Assembly assembly = Assembly.Load(new AssemblyName(tasksQz.AssemblyName));
                    Type jobType = assembly.GetType(tasksQz.AssemblyName + "." + tasksQz.ClassName);

                    #endregion
                    //判断任务调度是否开启
                    if (!_scheduler.IsStarted)
                    {
                        await StartScheduleAsync();
                    }
                    #region 泛型传递
                    //传入反射出来的执行程序集
                    //IJobDetail job = new JobDetailImpl(tasksQz.Id.ToString(), tasksQz.AppId, jobType);
                    //job.JobDataMap.Add("JobParam", tasksQz.JobParams);

                    IJobDetail jobdetail = JobBuilder.Create(jobType).WithIdentity(tasksQz.Id.ToString(), tasksQz.AppId).Build();

                    //IJobDetail job = JobBuilder.Create<T>()
                    //    .WithIdentity(sysSchedule.Name, sysSchedule.JobGroup)
                    //    .Build();
                    #endregion
                    ITrigger trigger;
                    if (tasksQz.Cron != null && CronExpression.IsValidExpression(tasksQz.Cron) && tasksQz.IsCron)
                    {
                        trigger = CreateCronTrigger(tasksQz);
                    }
                    else
                    {
                        //tasksQz.IntervalSecond = 5;
                        trigger = CreateSimpleTrigger(tasksQz);
                    }

                    // 告诉Quartz使用我们的触发器来安排作业
                    await _scheduler.ScheduleJob(jobdetail, trigger);
                    result.success = true;
                    result.msg = $"启动任务:【{tasksQz.Name}】成功";
                    return result;
                }
                catch (Exception ex)
                {
                    result.success = false;
                    result.msg = $"任务计划异常:【{ex.Message}】";
                    return result;
                }
            }
            else
            {
                result.success = false;
                result.msg = $"任务计划不存在:【{tasksQz?.Name}】";
                return result;
            }
        }

        /// <summary>
        /// 暂停一个指定的计划任务
        /// </summary>
        /// <returns></returns>
        public async Task<MessageModel<string>> StopScheduleJobAsync(TasksQzEntity qzModel)
        {
            var result = new MessageModel<string>();
            try
            {
                JobKey jobKey = new JobKey(qzModel.Id.ToString(), qzModel.AppId);
                if (!await _scheduler.CheckExists(jobKey))
                {
                    result.success = false;
                    result.msg = $"未找到要停止的任务:【{qzModel.Name}】";
                    return result;
                }
                else
                {
                    //暂停还在_scheduler中，这边直接移除
                    await this._scheduler.PauseJob(jobKey);
                    //var aaaa = await this._scheduler.GetTriggersOfJob(jobKey);
                    //var sss = await this._scheduler.IsJobGroupPaused(qzModel.AppId);
                    //var sss2 = await this._scheduler.GetJobGroupNames();
                    //var bbbb = this._scheduler.TriggerJob(aaaa.First().JobKey);
                    var res = await this._scheduler.DeleteJob(jobKey);
                    if (!res)
                    {
                        await this._scheduler.ResumeJob(jobKey);
                        result.msg = $"停止任务:【{qzModel.Name}】失败";
                        return result;
                    }
                    result.success = true;
                    result.msg = $"停止任务:【{qzModel.Name}】成功";
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 恢复指定的计划任务
        /// </summary>
        /// <param name="tasksQz"></param>
        /// <returns></returns>
        public async Task<MessageModel<string>> ResumeJob(TasksQzEntity qzModel)
        {
            var result = new MessageModel<string>();
            try
            {
                JobKey jobKey = new JobKey(qzModel.Id.ToString(), qzModel.AppId);
                if (!await _scheduler.CheckExists(jobKey))
                {
                    result.success = false;
                    result.msg = $"未找到要重新的任务:【{qzModel.Name}】,请先选择添加计划！";
                    return result;
                }
                //await this._scheduler.ResumeJob(jobKey);
                //await this._scheduler.TriggerJob(jobKey);//立即执行
                //_scheduler.RescheduleJob(triggerKey, trigger);//更新时间表达式、
                //_scheduler.GetCurrentlyExecutingJobs
                ITrigger trigger;
                if (qzModel.Cron != null && CronExpression.IsValidExpression(qzModel.Cron) && qzModel.IsCron)
                {
                    trigger = CreateCronTrigger(qzModel);
                }
                else
                {
                    trigger = CreateSimpleTrigger(qzModel);
                }

                TriggerKey triggerKey = new TriggerKey(qzModel.Id.ToString(), qzModel.AppId);
                await _scheduler.RescheduleJob(triggerKey, trigger);

               

                result.success = true;
                result.msg = $"恢复计划任务:【{qzModel.Name}】成功";
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region 创建触发器帮助方法

        /// <summary>
        /// 创建SimpleTrigger触发器（简单触发器）
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <param name="starRunTime"></param>
        /// <param name="endRunTime"></param>
        /// <returns></returns>
        private ITrigger CreateSimpleTrigger(TasksQzEntity qzModel)
        {
            /*
            if (qzModel.RunTimes > 0)
            {
                ITrigger trigger = TriggerBuilder.Create()
                //.StartNow()
                .WithIdentity(qzModel.Id.ToString(), qzModel.AppId)
                .WithSimpleSchedule(x =>
                x.WithIntervalInSeconds(qzModel.IntervalSecond)
                .WithRepeatCount(qzModel.RunTimes))//指定了执行次数
                .ForJob(qzModel.Id.ToString(), qzModel.AppId).Build();
                return trigger;
            }
            else
            {
                ITrigger trigger = TriggerBuilder.Create()
                //.StartNow()
                .WithIdentity(qzModel.Id.ToString(), qzModel.AppId)
                .WithSimpleSchedule(x =>
                x.WithIntervalInSeconds(qzModel.IntervalSecond)
                .RepeatForever()).ForJob(qzModel.Id.ToString(), qzModel.AppId).Build();
                return trigger;
            }
            */

            ITrigger trigger = TriggerBuilder.Create()
               //.StartNow() // 触发作业立即运行，然后每10秒重复一次，无限循环
               .WithIdentity(qzModel.Id.ToString(), qzModel.AppId)
               .WithSimpleSchedule(x =>
               x.WithIntervalInSeconds(qzModel.IntervalSecond)
               .RepeatForever()).ForJob(qzModel.Id.ToString(), qzModel.AppId).Build();
            return trigger;

        }



        /// <summary>
        /// 创建类型Cron的触发器
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private ITrigger CreateCronTrigger(TasksQzEntity qzModel)
        {
            // 作业触发器
            return TriggerBuilder.Create()
                   .WithIdentity(qzModel.Id.ToString(), qzModel.AppId)
                    //.StartAt(qzModel.BeginTime.Value)//开始时间
                    //.EndAt(qzModel.EndTime.Value)//结束数据
                   .WithCronSchedule(qzModel.Cron)//指定cron表达式
                    //.StartNow()
                   .ForJob(qzModel.Id.ToString(), qzModel.AppId)//作业名称
                   .Build();
        }
        #endregion

    }
}
