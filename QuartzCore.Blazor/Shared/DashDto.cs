using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.Blazor.Shared
{
    public class DashDto
    {
        public int appCount { get; set; } = 0;
        public int jobItemCount { get; set; } = 0;

        public ChartData[] ChartDatas { get; set; }

        /*
        public object[] ChartData2 { get; set; } = new object[]
                                                {
                                                    new {date = "00:00:00~02:00:00", value = 3},
                                                    new {date = "02:00:00~04:00:00", value = 40},
                                                    new {date = "04:00:00~06:00:00", value = 3.5},
                                                    new {date = "06:00:00~08:00:00", value = 55},
                                                    new {date = "08:00:00~10:00:00", value = 4.9, festival = "劳动节"},
                                                    new {date =  "10:00:00~12:00:00", value = 6},
                                                    new {date =  "12:00:00~14:00:00", value = 7},
                                                    new {date =  "14:00:00~16:00:00", value = 9},
                                                    new {date =  "16:00:00~18:00:00", value = 3},
                                                    new {date =  "18:00:00~20:00:00", value = 13, festival = "国庆节"},
                                                    new {date =  "20:00:00~22:00:00", value = 6},
                                                    new {date =  "22:00:00~24:00:00", value = 23}
                                              };*/
    }
    public class ChartData
    {
        public string date { get; set; }

        public int value { get; set; }

        public string festival { get; set; }
    }
}
