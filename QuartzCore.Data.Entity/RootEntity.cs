using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.Data.Entity
{
    public class RootEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        [Column(Name = "id", IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }

    }
}
