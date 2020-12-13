using QuartzCore.Blazor.Shared;
using QuartzCore.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.Tasks
{
    public interface ISchedulerCenter
    {

        /// <summary>
        /// 开启任务调度
        /// </summary>
        /// <returns></returns>
        Task<MessageModel<string>> StartScheduleAsync();
        /// <summary>
        /// 停止任务调度
        /// </summary>
        /// <returns></returns>
        Task<MessageModel<string>> StopScheduleAsync();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        Task<MessageModel<string>> AddScheduleJobAsync(TasksQzEntity sysSchedule);
        /// <summary>
        /// 停止一个任务
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        Task<MessageModel<string>> StopScheduleJobAsync(TasksQzEntity sysSchedule);
        /// <summary>
        /// 恢复一个任务
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        Task<MessageModel<string>> ResumeJob(TasksQzEntity sysSchedule);

    }
}
