using QuartzCore.Blazor.Shared;
using QuartzCore.Common;
using QuartzCore.Common.Helper;
using QuartzCore.Data.Entity;
using QuartzCore.Data.Freesql;
using QuartzCore.IService;
using QuartzCore.MongoDB.LogEntity;
using QuartzCore.MongoDB.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.Service
{
    public class QzRunLogService : IQzRunLogService
    {
        private FreeSqlContext _dbContext;
        private static string _logCount => Global.Config["logCount:num"];

        //private static object _syncObj = new object();

        IMongoRepository<QzRunLogMoEntity> _mongoRepository;

        public QzRunLogService(FreeSqlContext context, IMongoRepository<QzRunLogMoEntity> mongoRepository)
        {
            _dbContext = context;
            _mongoRepository = mongoRepository;
        }

        //public async Task<List<TasksQzEntity>> GetAsync()
        //{
        //    return await _dbContext.QzRunLogs.Where( ).ToListAsync();
        //}
        public async Task<List<QzRunLogEntity>> Find(int[] tasksQzIds)
        {
            return FreeSQL.Instance.Select<QzRunLogEntity>()
                .WhereIf(tasksQzIds.Length > 0, b => tasksQzIds.Contains(b.TasksQzId))
                .ToList();
        }

        public async Task<MessageModel<List<QzRunLogEntity>>> Find(QzRunLogQueryDto queryDto)
        {
            MessageModel<List<QzRunLogEntity>> messageModel = new MessageModel<List<QzRunLogEntity>>();
            var items = FreeSQL.Instance.Select<QzRunLogEntity>()
                .WhereIf(!string.IsNullOrEmpty(queryDto.AppId), b => b.AppId == queryDto.AppId)
                .WhereIf(queryDto.TasksQzId > 0, b => b.TasksQzId == queryDto.TasksQzId)
                .WhereIf((queryDto.RangePicker != null && queryDto.RangePicker.Length > 0), b => b.LogTime > queryDto.RangePicker[0] && b.LogTime < queryDto.RangePicker[1])
                .OrderByDescending(b => b.LogTime)
                .Count(out var total) //总记录数量
                .Page(queryDto.PageIndex, queryDto.PageSize)
                //.Skip(queryDto.PageIndex)
                //.Limit(queryDto.PageSize) //第100行-110行的记录
                .ToList();
            //var items =  FreeSQL.Instance.Select<QzRunLogEntity, TasksQzEntity, AppEntity>()
            //                .LeftJoin((a, b, c) => a.TasksQzId == b.Id)
            //                .LeftJoin((a, b, c) => a.AppId == c.Id)
            //                .WhereIf(queryDto.TasksQzId > 0, (a, b, c) => a.TasksQzId == queryDto.TasksQzId)
            //                .WhereIf(!string.IsNullOrEmpty( queryDto.AppId), (a, b, c) => a.AppId == queryDto.AppId)
            //                .OrderByDescending((a, b, c) => a.LogTime)
            //                .Count(out var total) //总记录数量
            //                .Page(queryDto.PageIndex, queryDto.PageSize)
            //                .ToList((a, b, c) => new { a, b, c });
            messageModel.response = items;
            messageModel.Total =  total.ObjToInt();
            messageModel.success = true;

            return messageModel;
        }

        public async Task<bool> AddAsync(QzRunLogEntity log)
        {
            #region mongo
            QzRunLogMoEntity mlog = new QzRunLogMoEntity();
            mlog.AppId = log.AppId;
            mlog.TasksQzId = log.TasksQzId;
            mlog.LogText = log.LogText;
            mlog.LogTime = log.LogTime?.AddHours(8);
            mlog.Milliseconds = log.Milliseconds;
            mlog.LogType = (int)log.LogType;
            await _mongoRepository.InsertAsync(mlog);
            #endregion

            var result = false;
            var list = await _dbContext.QzRunLogs.Where(x => x.TasksQzId == log.TasksQzId && x.AppId == log.AppId).ToListAsync();
            if (list.Count >= _logCount.ObjToInt())
            {
                var oldLog = list.OrderBy(x => x.LogTime).First();
                log.Id = oldLog.Id;
                _dbContext.Update(log);
                var y = await _dbContext.SaveChangesAsync();
                result = y > 0;
                return result;
            }
            await _dbContext.QzRunLogs.AddAsync(log);
            int x = await _dbContext.SaveChangesAsync();

            result = x > 0;
            return result;
        }
    }
}
