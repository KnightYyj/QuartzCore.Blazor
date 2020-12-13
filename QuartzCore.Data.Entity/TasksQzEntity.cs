using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.Data.Entity
{
    /// <summary>
    /// 任务计划表
    /// </summary>
    [Table(Name = "TasksQz")]
    public class TasksQzEntity : RootEntity
    {
        /// <summary>
        /// 任务项名称
        /// </summary>
        [Column(Name = "name", StringLength = 200, IsNullable = false)]
        [Description("任务项名称")] 
        public string Name { get; set; }

        /// <summary>
        /// 任务所属应用
        /// </summary>
        [Column(Name = "app_id", StringLength = 36)]
        [Description("任务所属应用")]
        public string AppId { get; set; }
        /// <summary>
        /// 触发器类型（false:simple true:cron）
        /// </summary>
        [Column(Name = "trigger_type")]
        [Description("触发器类型 cron or simple")]
        public bool IsCron { get; set; }
        /// <summary>
        /// 执行间隔时间, 秒为单位
        /// </summary>
        [Description("执行间隔时间, 秒为单位")]
        [Column(Name = "interval_second")]
        public int IntervalSecond { get; set; }
        /// <summary>
        /// 任务运行时间表达式
        /// </summary>
        [Description("任务运行时间表达式")]
        [Column(Name = "cron", StringLength = 200, IsNullable = true)]
        public string Cron { get; set; }
        /// <summary>
        /// 任务所在DLL对应的程序集名称
        /// </summary>
        [Description("任务所在DLL对应的程序集名称")]
        [Column(Name = "assembly_name", StringLength = 200, IsNullable = true)]
        public string AssemblyName { get; set; }
        /// <summary>
        /// 任务所在类
        /// </summary>
        [Description("任务所在类")]
        [Column(Name = "class_name", StringLength = 200, IsNullable = true)]
        public string ClassName { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        [Description("任务描述")]
        [Column(Name = "remark", StringLength = 2000, IsNullable = true)]
        public string Remark { get; set; }

        /// <summary>
        /// 执行次数
        /// </summary>
        [Column(Name = "run_times")]
        [Description("执行次数")]
        public int RunTimes { get; set; }

       
        /// <summary>
        /// 是否启动
        /// </summary>
        [Column(Name = "is_start")]
        [Description("是否启动")]
        public bool IsStart { get; set; } = false;
        /// <summary>
        /// 执行传参
        /// </summary>
        [Column(Name = "job_params")]
        [Description("执行传参")]
        public string JobParams { get; set; }



        [Column(Name = "is_deleted", IsNullable = false)]
        [Description("是否删除")]
        public bool IsDeleted { get; set; }

        [Column(Name = "create_time")]
        public DateTime CreateTime { get; set; }

        [Column(Name = "update_time")]
        public DateTime? UpdateTime { get; set; }

        #region HTTP

        /// <summary>
        /// （0、否 1、是）
        /// </summary>
        [Column(Name = "is_apiurl")]
        public bool IsApiUrl { get; set; }
        /// <summary>
        /// url
        /// </summary>
        [Column(Name = "api_url")]
        public string ApiUrl { get; set; }

        /// <summary>
        /// 请求体(仅POST有效)
        /// </summary>
        [Column(Name = "request_value")]
        public string RequestValue { get; set; }

        /// <summary>
        /// 1:GET  2:POST
        /// </summary>
        [Column(Name = "method_type")]
        public int MethodType { get; set; }

        /// <summary>
        /// 负责人工号(不填默认是你,多个负责人间用半角分号隔开，比如：123456;11134)
        /// </summary>
        [Column(Name = "handler_jobnum")]
        public string HandlerJobNum { get; set; }

        /// <summary>
        /// 报警邮箱
        /// </summary>
        [Column(Name = "error_email")]
        [Description("报警邮箱")]
        public string ErrorEmail { get; set; }

        #endregion
    }
}
