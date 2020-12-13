using Quartz;
using QuartzCore.Blazor.Shared;
using QuartzCore.Common;
using QuartzCore.Common.Helper;
using QuartzCore.Data.Entity;
using QuartzCore.Data.Freesql;
using QuartzCore.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.Service
{
    public class TasksQzService : ITasksQzService
    {
        private FreeSqlContext _dbContext;

        public TasksQzService(FreeSqlContext context)
        {
            _dbContext = context;

        }

        public bool isValidExpression(string expstr)
        {
            if (string.IsNullOrEmpty(expstr))
                return false;
            string[] expArr = expstr.Split(new char[1] { ' ' });
            if (expArr.Length == 6 || expArr.Length == 7)
            {
                try
                {
                    if (expArr.Length == 7)
                    {
                        int.Parse(expArr[6]);
                    }
                    bool isValid = CronExpression.IsValidExpression(expstr);
                    if (isValid)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }
        public List<string> NextValidTime(string expstr)
        {
            if (string.IsNullOrEmpty(expstr))
                return new List<string>();
            List<string> lst = new List<string>();
            CronExpression exp = new CronExpression(expstr);
            DateTimeOffset ddo = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
            for (int i = 0; i < 5; i++)
            {
                ddo = (DateTimeOffset)exp.GetNextValidTimeAfter(ddo);
                lst.Add(ddo.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            return lst;
        }



        public async Task<List<TasksQzEntity>> GetAsync()
        {
            return await _dbContext.TasksQzs.Where(a => a.IsDeleted == false).ToListAsync();
        }

        public async Task<TasksQzEntity> GetAsync(int id)
        {
            return await _dbContext.TasksQzs.Where(a => a.Id == id).ToOneAsync();
        }

        public async Task<TasksQzEntity> GetAsync(string name)
        {
            return await _dbContext.TasksQzs.Where(a => a.Name == name).ToOneAsync();
        }

        public async Task<TasksQzEntity> GetByAppIdAsync(string appId)
        {
            return await _dbContext.TasksQzs.Where(a => a.AppId == appId).ToOneAsync();
        }


        public async Task<bool> AddAsync(TasksQzEntity qz)
        {
            await _dbContext.TasksQzs.AddAsync(qz);
            int x = await _dbContext.SaveChangesAsync();
            var result = x > 0;
            return result;
        }

        public async Task<bool> UpdateAsync(TasksQzEntity qz)
        {
            _dbContext.Update(qz);
            var x = await _dbContext.SaveChangesAsync();
            var result = x > 0;
            return result;
        }





        public async Task<MessageModel<List<TasksQzEntity>>> FindAsync(JobItemQueryDto queryDto)
        {
            MessageModel<List<TasksQzEntity>> messageModel = new MessageModel<List<TasksQzEntity>>();
            var jobItems = FreeSQL.Instance.Select<TasksQzEntity>()
                 .WhereIf(!string.IsNullOrEmpty(queryDto.AppId), b => b.AppId == queryDto.AppId)
                 .WhereIf(!string.IsNullOrEmpty(queryDto.Name), b => b.Name == queryDto.Name)
                 .WhereIf(queryDto.Deleted != (int)BoolStatus.All, b =>b.IsDeleted ==  (queryDto.Deleted == (int)BoolStatus.True))
                 .OrderBy(b => b.CreateTime)
                 .Count(out var total) //总记录数量
                 .Page(queryDto.PageIndex, queryDto.PageSize)
                 //.Skip(queryDto.PageIndex)
                 //.Limit(queryDto.PageSize) //第100行-110行的记录
                 .ToList();

            messageModel.response = jobItems;
            messageModel.Total = total.ObjToInt();
            messageModel.success = true;
            return messageModel;
        }


        public async Task<MessageModel<DashDto>> GetDashAsync()
        {
            MessageModel<DashDto> messageModel = new MessageModel<DashDto>();
            messageModel.response = new DashDto();
            messageModel.response.appCount = (await _dbContext.Apps.Where(a => a.Enabled == true).ToListAsync()).Count;
            messageModel.response.jobItemCount = (await _dbContext.TasksQzs.Where(a => a.IsDeleted == false).ToListAsync()).Count;
            messageModel.success = true;
            messageModel.msg = "成功";

            List<DateTime> lst = new List<DateTime>();
            DateTime dtime = DateTime.Now;
            lst.Add(dtime);
            for (int i = 1; i <= 12; i++)
            {
                lst.Add(dtime.AddHours(i * (-2)));
            }
            //_dbContext.QzRunLogs.Where()

            ChartData[] data2 =
            {
        new ChartData{date = "00:00:00~02:00:00", value = 3},
        new ChartData{date = "02:00:00~04:00:00", value = 40},
        new ChartData{date = "04:00:00~06:00:00", value = 35},
        new ChartData{date = "06:00:00~08:00:00", value = 55},
        new ChartData{date = "08:00:00~10:00:00", value = 4, festival = "待提供"},
        new ChartData{date =  "10:00:00~12:00:00", value = 6},
        new ChartData{date =  "12:00:00~14:00:00", value = 7},
        new ChartData{date =  "14:00:00~16:00:00", value = 9},
        new ChartData{date =  "16:00:00~18:00:00", value = 3},
        new ChartData{date =  "18:00:00~20:00:00", value = 13, festival = "待提供"},
        new ChartData{date =  "20:00:00~22:00:00", value = 6},
        new ChartData{date =  "22:00:00~24:00:00", value = 23}
    };
            messageModel.response.ChartDatas = data2.ToArray();
            return messageModel;
        }

    }
}
