using QuartzCore.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.Blazor.Shared
{
    public class QzRunLogDto
    {
        public int Id { get; set; }
        [DisplayName("任务项Id")]
        public int TasksQzId { get; set; }

        [DisplayName("任务项名称")]
        public string TasksQzName { get; set; }

        [DisplayName("应用Id")]
        public string AppId { get; set; }

        [DisplayName("所属应用")]
        public string AppName { get; set; }


        [DisplayName("日志类型")]
        public SysLogType LogType { get; set; }

        /// <summary>
        /// Job执行开始时间
        /// </summary>
        [DisplayName("执行时间")]
        public DateTime? LogTime { get; set; }
        [DisplayName("消息")]
        public string LogText { get; set; }

        /// <summary>
        /// 耗时
        /// </summary>
        [DisplayName("执行耗时")]
        public string Milliseconds { get; set; }
    }
}
