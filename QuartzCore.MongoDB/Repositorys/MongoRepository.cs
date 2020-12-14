using MongoDB.Bson;
using MongoDB.Driver;
using QuartzCore.Blazor.Shared;
using QuartzCore.Blazor.Shared.MongoDB;
using QuartzCore.MongoDB.DbContexts;
using QuartzCore.MongoDB.LogEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCore.MongoDB.Repositorys
{
    public class MongoRepository<TEntity> : IMongoRepository<TEntity> where TEntity : IBaseMEntity<string>
    {

        private readonly MongoDbContextBase _mongoDbContext = null;

        public virtual IMongoCollection<TEntity> Collection { get; private set; }

        //BsonDocument
        public MongoRepository(IServiceProvider serviceProvider, MongoDbContextBase mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
            Collection = _mongoDbContext.Collection<TEntity>();
        }

        public async Task<string> InsertAsync(TEntity t)
        {
            await Collection.InsertOneAsync(t);
            return t.Id;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteResult = await Collection.DeleteOneAsync(x => x.Id == id);
            return deleteResult.DeletedCount != 0;
        }

        public async Task<List<TEntity>> GetListAsync(int skip = 0, int count = 0)
        {
            try
            {
                var result = await Collection.Find(x => true)
                                   .Skip(skip)
                                   .Limit(count)
                                   .ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
  


        public async Task<List<TEntity>> GetListByFieldAsync(string fieldName, string fieldValue)
        {
            var filter = Builders<TEntity>.Filter.Eq(fieldName, fieldValue);
            var result = await Collection.Find(filter).ToListAsync();
            return result;
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            var filters = new List<FilterDefinition<TEntity>>
            {
                Builders<TEntity>.Filter.Eq(e => e.Id, id)
            };
            //AddGlobalFilters(filters);
            return await Collection.Find(Builders<TEntity>.Filter.And(filters)).FirstOrDefaultAsync();

        }

        public async Task<bool> Update(TEntity t)
        {
            var filter = Builders<TEntity>.Filter.Eq(x => x.Id, t.Id);
            var replaceOneResult = await Collection.ReplaceOneAsync(doc => doc.Id == t.Id, t);
            return replaceOneResult.ModifiedCount != 0;
        }

        public virtual async Task<List<TEntity>> GetAsync(FilterDefinition<TEntity> filter)
        {
            var result = new List<TEntity>();
            var builder = Builders<TEntity>.Filter;
            var dateFilter = builder.Where(d => 1 == 1);
            var filters = filter == null ? dateFilter : builder.And(dateFilter, filter);
            var entitys = Collection.Find(filter);
            result = await entitys.ToListAsync();
            return result;

        }
        public virtual async Task<Pagelist<TEntity>> GetAsync(FilterDefinition<TEntity> filter, PageInputByDateDto pageInput)
        {
            var builder = Builders<TEntity>.Filter;
            if (pageInput.StartTime != null && pageInput.EndTime != null)
            {
                var dateFilter = builder.Where(d => 1==1);
                var filters = filter == null ? dateFilter : builder.And(dateFilter, filter);
                return await GetAsync(filters, (PageInputDto)pageInput);
            }
            return await GetAsync(filter, (PageInputDto)pageInput);
        }

        public virtual async Task<Pagelist<TEntity>> GetAsync(FilterDefinition<TEntity> filter, PageInputDto pageInput)
        {
            if (filter == null) filter = new BsonDocument();//mongodb过滤不允许为空
            var pagedList = new Pagelist<TEntity>();
            var result = new List<TEntity>();
            var entitys = Collection.Find(filter);
            var count = await entitys.CountDocumentsAsync();
            if (pageInput.PageSize == 0)
            {
                //排序
                if (pageInput.Isdesc)
                {
                    result = await entitys.SortByDescending(d => d.Id).ToListAsync();
                }
                else
                {
                    result = await entitys.ToListAsync();
                }
            }
            else
            {
                //排序
                if (pageInput.Isdesc)
                {
                    result = await entitys.SortByDescending(d => d.Id).Skip((pageInput.Page - 1) * pageInput.PageSize).Limit(pageInput.PageSize).ToListAsync();
                }
                else
                {
                    result = await entitys.Skip((pageInput.Page - 1) * pageInput.PageSize).Limit(pageInput.PageSize).ToListAsync();
                }

            }
            pagedList.Data.AddRange(result);
            pagedList.TotalCount = count;
            pagedList.PageSize = pageInput.PageSize;



            return pagedList;
        }

    }
}
 