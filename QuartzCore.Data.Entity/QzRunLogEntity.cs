using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.Data.Entity
{

    public enum SysLogType
    {
        Normal = 0,
        Warn = 1
    }

    [Table(Name = "QzRunLog")]
    public class QzRunLogEntity
    {
        [Column(Name = "id", IsIdentity = true)]
        public int Id { get; set; }

        [Column(Name = "taskqz_id")]
        public int TasksQzId { get; set; }

        [Column(Name = "app_id")]
        public string AppId { get; set; }

        [Column(Name = "log_type")]
        public SysLogType LogType { get; set; }
        /// <summary>
        /// Job执行开始时间
        /// </summary>
        [Column(Name = "log_time")]
        public DateTime? LogTime { get; set; }

        [Column(Name = "log_text", StringLength = 2000)]
        public string LogText { get; set; }

        /// <summary>
        /// 耗时
        /// </summary>
        public string Milliseconds { get; set; }
    }
}
