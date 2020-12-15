using System;
using System.Collections.Generic;
using Xunit;

namespace QuartzCore.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            List<DateTime> lst = new List<DateTime>();
            DateTime dtime = DateTime.Now;
            lst.Add(dtime);
            for (int i = 1; i <= 12; i++)
            {
                lst.Add(dtime.AddHours(i * (-2)));
            }
            Assert.Equal(12,lst.Count);
        }
    }
}
