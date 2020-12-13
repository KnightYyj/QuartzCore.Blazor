using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.Blazor.Shared
{
    public class TasksQzDto
    {
        /// <summary>
        /// ID
        /// </summary>

        public int Id { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [Required(ErrorMessage = "任务名称不能为空")]
        [MaxLength(50, ErrorMessage = "任务名称长度不能超过50位")]
        [DisplayName("任务名称")]
        public string Name { get; set; }
        /// <summary>
        /// 任务所属应用
        /// </summary>
        [Required(ErrorMessage = "所属应用不能为空")]
        [MaxLength(36, ErrorMessage = "任务名称长度不能超过36位")]
        //[DisplayName("所属应用")]
        public string AppId { get; set; }

        [DisplayName("所属应用")]
        public string AppName { get; set; }
        /// <summary>
        /// 触发器类型（false:simple true:cron）
        /// </summary>
        [Required]
        [DisplayName("触发器类型")]
        public bool IsCron { get; set; }
        /// <summary>
        /// 执行间隔时间, 秒为单位
        /// </summary>
        public int IntervalSecond { get; set; }
        /// <summary>
        /// 任务运行时间表达式
        /// </summary>
        //[Required(ErrorMessage = "表达式不能为空")]
        [DisplayName("触发器")]
        public string Cron { get; set; }
        /// <summary>
        /// 任务所在DLL对应的程序集名称
        /// </summary>
        public string AssemblyName { get; set; }
        /// <summary>
        /// 任务所在类
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        [DisplayName("备注")]
        public string Remark { get; set; }
        /// <summary>
        /// 执行次数
        /// </summary> 
        [DisplayName("执行次数")]
        public int RunTimes { get; set; }

        /// <summary>
        /// 是否启动
        /// </summary> 
        [DisplayName("启动状态")]
        public bool IsStart { get; set; } = false;
        /// <summary>
        /// 执行传参
        /// </summary>      
        public string JobParams { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// （0、否 1、是）
        /// </summary>
        public bool IsApiUrl { get; set; }
        /// <summary>
        /// url
        /// </summary>
        [DisplayName("Url/程序集")]
        public string ApiUrl { get; set; }

        /// <summary>
        /// 请求体(仅POST有效)
        /// </summary>

        public string RequestValue { get; set; }

        /// <summary>
        /// 1:GET  2:POST
        /// </summary>

        public int MethodType { get; set; }

        /// <summary>
        /// 负责人工号(不填默认是你,多个负责人间用半角分号隔开，比如：123456;11134)
        /// </summary>

        public string HandlerJobNum { get; set; }

        /// <summary>
        /// 报警邮箱
        /// </summary>

        public string ErrorEmail { get; set; }

        #region 额外渲染用

        [DisplayName("日志")]
        public List<QzRunLogDto> Logs { get; set; } = new List<QzRunLogDto>();

        #endregion
    }
}
