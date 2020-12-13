using QuartzCore.Blazor.Shared;
using QuartzCore.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.IService
{
    public interface ITasksQzService
    {
        bool isValidExpression(string expstr);
        List<string> NextValidTime(string expstr);

        Task<MessageModel<DashDto>> GetDashAsync();
        Task<List<TasksQzEntity>> GetAsync();

        Task<MessageModel<List<TasksQzEntity>>> FindAsync(JobItemQueryDto queryDto);

        Task<TasksQzEntity> GetByAppIdAsync(string appId);
        Task<TasksQzEntity> GetAsync(string name);
        Task<TasksQzEntity> GetAsync(int id);
        Task<bool> AddAsync(TasksQzEntity app);
        Task<bool> UpdateAsync(TasksQzEntity app);

        //Task<bool> DeleteAsync(TasksQzEntity app);
    }
}
