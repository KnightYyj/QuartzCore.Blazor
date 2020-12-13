using FreeSql;
using QuartzCore.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.Data.Freesql
{
    public class FreeSqlContext : DbContext
    {
        public FreeSqlContext()
        {
        }

        public FreeSqlContext(IFreeSql freeSql) : base(freeSql, null)
        {
            fsql = freeSql;
        }

        public IFreeSql fsql { get; set; }

        public DbSet<AppEntity> Apps { get; set; }

        public DbSet<TasksQzEntity> TasksQzs { get; set; }

        public DbSet<QzRunLogEntity> QzRunLogs { get; set; }
        
    }
}
