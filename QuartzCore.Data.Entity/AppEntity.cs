using FreeSql.DataAnnotations;
using System;

namespace QuartzCore.Data.Entity
{
    [Table(Name ="App")]
    public class AppEntity
    {
        /// <summary>
        /// 项目标识
        /// </summary>
        [Column(Name = "id", StringLength = 36)]
        public string Id { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        [Column(Name = "name", StringLength = 50)]
        public string Name { get; set; }

        [Column(Name = "create_time")]
        public DateTime CreateTime { get; set; }

        [Column(Name = "update_time")]
        public DateTime? UpdateTime { get; set; }

        [Column(Name = "enabled")]
        public bool Enabled { get; set; }
    }
}
