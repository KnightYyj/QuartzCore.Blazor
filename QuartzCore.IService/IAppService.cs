using QuartzCore.Data.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuartzCore.IService
{
    public interface IAppService
    {
        Task<List<AppEntity>> GetAsync();
        Task<AppEntity> GetAsync(string id);
        Task<bool> AddAsync(AppEntity app);

        Task<bool> DeleteAsync(string appId);


        Task<bool> UpdateAsync(AppEntity app);

        //Task<List<AppEntity>> GetAllAppsAsync();


    }
}
