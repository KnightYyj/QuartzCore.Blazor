using Microsoft.Extensions.Configuration;
using System;

namespace QuartzCore.Common
{
    public class Global
    {
        private static IConfiguration _configuration;
        public static IConfiguration Config
        {
            get { return _configuration; }
            set
            {
                _configuration = value;
            }
        }
    }
}
