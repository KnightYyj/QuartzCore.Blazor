using QuartzCore.Common;
using QuartzCore.Data.Entity.SeedData;
using System;

namespace QuartzCore.Data.Freesql
{
    public static class FreeSQL
    {
        private static IFreeSql _freesql;

        static FreeSQL()
        {
            _freesql = new FreeSql.FreeSqlBuilder()
                          .UseConnectionString(ProviderToFreesqlDbType(DbProvider), DbConnection)
                          .Build();
            //FluentApi.Config(_freesql);
            DBSeed.Ensure(_freesql);
        }
 
        public static IFreeSql Instance => _freesql;

        private static string DbProvider => Global.Config["db:provider"];
        private static string DbConnection => Global.Config["db:conn"];

        private static FreeSql.DataType ProviderToFreesqlDbType(string provider)
        {
            switch (provider)
            {
                case "sqlite":
                    return FreeSql.DataType.Sqlite;
                case "mysql":
                    return FreeSql.DataType.MySql;
                case "sqlserver":
                    return FreeSql.DataType.SqlServer;
                case "npgsql":
                    return FreeSql.DataType.PostgreSQL;
                case "oracle":
                    return FreeSql.DataType.Oracle;
                default:
                    break;
            }

            return FreeSql.DataType.Sqlite;
        }
    }
}
