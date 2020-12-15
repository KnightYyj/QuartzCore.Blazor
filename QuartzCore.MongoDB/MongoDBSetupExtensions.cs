using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using QuartzCore.MongoDB.DbContexts;
using QuartzCore.Common;
using QuartzCore.Common.Helper;
using QuartzCore.MongoDB.Repositorys;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace QuartzCore.MongoDB
{
    public static class MongoDBSetupExtensions
    {

        public static IServiceCollection AddMongoDbContext(this IServiceCollection services)
        {
            bool isProvider = Global.Config["mongo:isprovider"].ObjToBool();
            if (isProvider)
            {
                string DbConn = Global.Config["mongo:conn"];
                services.AddMongoDbContext<DefaultMongoDbContext>(options =>
                {
                    options.ConnectionString = DbConn;
                });
                //services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
                services.TryAddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            }

            return services;
        }
        public static IServiceCollection AddMongoDbContext<TContext>(this IServiceCollection services, [CanBeNull] Action<MongoDbContextOptions> optionsAction) where TContext : MongoDbContextBase
        {
            MongoDbContextOptions options = new MongoDbContextOptions();
            optionsAction(options);
            services.AddSingleton<MongoDbContextOptions>(options);
            services.AddScoped<MongoDbContextBase, TContext>();
            return services;
        }
    }
}
