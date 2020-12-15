using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.Blazor.Shared
{
    /// <summary>
    /// 分页输出
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pagelist<T>
    {
        public Pagelist()
        {
            Data = new List<T>();
        }
        /// <summary>
        /// 数据
        /// </summary>
        public List<T> Data { get; set; }
        /// <summary>
        /// 数据总数
        /// </summary>
        public long TotalCount { get; set; }
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; }
    }
}
