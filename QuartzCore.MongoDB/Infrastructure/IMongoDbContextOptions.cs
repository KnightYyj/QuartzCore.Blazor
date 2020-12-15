using System;
using System.Collections.Generic;
using System.Text;

namespace QuartzCore.MongoDB.Infrastructure
{
    public interface IMongoDbContextOptions
    {
        string ConnectionString { get; set; }
    }
}
