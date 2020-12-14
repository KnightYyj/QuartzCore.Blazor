using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.Common.Attributes
{
    /// <summary>
    /// MongoDb表名称特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MongoDBTableAttribute : Attribute
    {
        public MongoDBTableAttribute(string tablename)
        {
            TableName = tablename;
        }

        /// <summary>
        /// MongoDB表名称
        /// </summary>
        public string TableName { get; }
    }
}
