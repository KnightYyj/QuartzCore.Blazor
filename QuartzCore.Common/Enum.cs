using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.Common
{
    /// <summary>
    /// 布尔值状态
    /// </summary>
    public enum BoolStatus : int
    {
        All = -1,
        /// <summary>
        /// 假(0)
        /// </summary>
        False = 0,
        /// <summary>
        /// 真(1)
        /// </summary>
        True = 1,
    }
}
