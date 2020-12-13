using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.Blazor.Shared
{
    public class QueryDto
    {
        /// <summary>
        /// 页开始序号
        /// </summary>
        public int PageIndex { get; set; } = 0;
        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; } = 5;

        public List<QuerySort> Sorts { get; set; } = new List<QuerySort>();
    }

    public class QuerySort
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }
        /// <summary>
        /// 排序方向
        /// </summary>
        public string SortOrder { get; set; }
    }
}
