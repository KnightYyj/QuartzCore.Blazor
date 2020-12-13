using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.Data.Freesql
{
    public static class ServiceCollectionExt
    {
        public static void AddFreeSqlDbContext(this IServiceCollection sc)
        {
            sc.AddFreeDbContext<FreeSqlContext>(options => options.UseFreeSql(FreeSQL.Instance));
        }
    }
}
