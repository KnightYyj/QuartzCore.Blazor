using QuartzCore.Common;
using QuartzCore.Data.Entity;
using QuartzCore.Data.Freesql;
using QuartzCore.IService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartzCore.Service
{
    public class AppService : IAppService
    {
        private FreeSqlContext _dbContext;

        public AppService(FreeSqlContext context)
        {
            _dbContext = context;
        }

        public async Task<bool> AddAsync(AppEntity app)
        {
            await _dbContext.Apps.AddAsync(app);
            int x = await _dbContext.SaveChangesAsync();
            var result = x > 0;
            return result;
        }

        public async Task<AppEntity> GetAsync(string id)
        {
            return await _dbContext.Apps.Where(a => a.Id == id).ToOneAsync();
        }

        public async Task<List<AppEntity>> GetAsync()
        {
            //return await _dbContext.Apps.Where(a => a.Enabled == true).ToListAsync();
            return await _dbContext.Apps.Where(_ => 1 == 1).ToListAsync();
        }

        public async Task<bool> UpdateAsync(AppEntity app)
        {
            _dbContext.Update(app);
            var x = await _dbContext.SaveChangesAsync();
            var result = x > 0;
            return result;
        }


        public Task<bool> DeleteAsync(string appId)
        {
            var result = FreeSQL.Instance.Delete<AppEntity>(new AppEntity { Id = appId }).ExecuteAffrows();
            return Task.FromResult(result > 0);
        }
    }
}
