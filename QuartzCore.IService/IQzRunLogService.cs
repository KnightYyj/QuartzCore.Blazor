using QuartzCore.Blazor.Shared;
using QuartzCore.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.IService
{
    public interface IQzRunLogService
    {
        //Task<List<QzRunLogEntity>> GetAsync();
        Task<MessageModel<List<QzRunLogEntity>>>  Find( QzRunLogQueryDto queryDto);

        Task<List<QzRunLogEntity>> Find(int[] tasksQzIds);

        Task<bool> AddAsync(QzRunLogEntity app);
    }
}
