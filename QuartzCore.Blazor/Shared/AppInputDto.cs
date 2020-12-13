using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.Blazor.Shared
{
    public class AppInputDto
    {
        /// <summary>
        /// 项目标识
        /// </summary>
        [Required(ErrorMessage = "应用标识不能为空")]
        [MaxLength(36, ErrorMessage = "应用标识长度不能超过36位")]
        [DisplayName("应用标识")]
        public string Id { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        [Required(ErrorMessage = "应用名称不能为空")]
        [MaxLength(50, ErrorMessage = "应用名称长度不能超过50位")]
        [DisplayName("应用名称")]
        public string Name { get; set; }
        [Required]
        [DisplayName("是否启用")]
        public bool Enabled { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
