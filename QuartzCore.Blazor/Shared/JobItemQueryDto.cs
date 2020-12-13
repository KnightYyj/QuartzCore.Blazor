using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.Blazor.Shared
{

    public class JobItemQueryDto : QueryDto
    {
        [DisplayName("应用Id")]
        public string AppId { get; set; }
        [DisplayName("任务名称")]
        public string Name { get; set; }

        public int Deleted { get; set; } = -1;

    }



    public class QzRunLogQueryDto : QueryDto
    {
        [DisplayName("应用Id")]
        public string AppId { get; set; }
        [DisplayName("任务Id")]
        public int TasksQzId { get; set; }

        public DateTime?[] RangePicker { get; set; } = new DateTime?[] { null, null };

    }
}
