using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.Blazor.Shared.MongoDB
{

    /// <summary>
    /// 分页输入
    /// </summary>
    public class PageInputDto
    {
        public string Search { get; set; }
        public bool Isdesc { get; set; } = true;
        private int page;

        /// <summary>
        /// 页码
        /// </summary>
        public int Page
        {
            get
            {
                return page;
            }
            set
            {
                page = value;
                if (page <= 0) page = 1;

            }
        }
        private int pagesize;
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize
        {
            set { pagesize = value; }
            get { return pagesize; }

        }
    }
}
