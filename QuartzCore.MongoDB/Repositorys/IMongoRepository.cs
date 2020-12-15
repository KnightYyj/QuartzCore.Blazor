using MongoDB.Driver;
using QuartzCore.Blazor.Shared;
using QuartzCore.Blazor.Shared.MongoDB;
using QuartzCore.MongoDB.LogEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.MongoDB.Repositorys
{
    public interface IMongoRepository<TEntity> where TEntity : IBaseMEntity<string>
    {
        Task<TEntity> GetByIdAsync(string id);
        Task<string> InsertAsync(TEntity entity);
        Task<bool> DeleteAsync(string id);
        Task<List<TEntity>> GetListAsync(int skip = 0, int count = 0);
        Task<List<TEntity>> GetListByFieldAsync(string fieldName, string fieldValue);
        Task<List<TEntity>> GetAsync(FilterDefinition<TEntity> filter);
        Task<Pagelist<TEntity>> GetAsync(FilterDefinition<TEntity> filter, PageInputByDateDto pageInput);
        Task<Pagelist<TEntity>> GetAsync(FilterDefinition<TEntity> filter, PageInputDto pageInput);

    }
}
