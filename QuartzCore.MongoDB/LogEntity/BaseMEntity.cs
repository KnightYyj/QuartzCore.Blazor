using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.MongoDB.LogEntity
{
 
    public interface BaseMEntity
    {
    }

    /// <summary>
    /// 实体接口
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IBaseMEntity<out TKey> : BaseMEntity
    {
        [Description("主键")]
        TKey Id { get; }
    }
}
