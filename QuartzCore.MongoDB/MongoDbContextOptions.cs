using QuartzCore.MongoDB.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuartzCore.MongoDB
{
    public class MongoDbContextOptions : IMongoDbContextOptions
    {
        public string ConnectionString { get; set; }
    }
}
