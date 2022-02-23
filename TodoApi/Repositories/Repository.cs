using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Repositories
{
  // https://www.programmingwithwolfgang.com/repository-pattern-net-core/
  // Also borrows inspiration from ABP Repository
  public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
  {
    protected readonly TodoContext dbContext;

    public Repository(TodoContext dbContext)
    {
      this.dbContext = dbContext;
    }

    public IQueryable<TEntity> GetQueryable()
    {
      try
      {
        return dbContext.Set<TEntity>();
      }
      catch (Exception ex)
      {
        throw new Exception($"Couldn't retrieve entities: {ex.Message}");
      }
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
      if (entity == null)
      {
        throw new ArgumentNullException($"{nameof(entity)} entity must not be null");
      }

      try
      {
        await dbContext.AddAsync(entity);
        await dbContext.SaveChangesAsync();

        return entity;
      }
      catch (Exception ex)
      {
        throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
      }
    }

    public async Task<List<TEntity>> GetListAsync()
    {
      return await GetQueryable().ToListAsync();
    }

    public async Task<int> GetCountAsync()
    {
      return await GetQueryable().CountAsync();
    }

    public async Task<TEntity> GetAsync(Guid id)
    {
      var entity = await dbContext.Set<TEntity>().FindAsync(id);

      if (entity == null)
      {
        throw new Exception($"Could not find {nameof(entity)}");
      }

      return entity;
    }

    public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
      #pragma warning disable CS8603
      return await GetQueryable().FirstOrDefaultAsync(predicate);
      #pragma warning restore CS8603
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
      if (entity == null)
      {
        throw new ArgumentNullException($"{nameof(entity)} entity must not be null");
      }

      try
      {
        dbContext.Update(entity);
        await dbContext.SaveChangesAsync();

        return entity;
      }
      catch (Exception ex)
      {
        throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
      }
    }

    public async Task<TEntity> UpdateAsync(Guid id)
    {
      var entity = await GetAsync(id);
      return await UpdateAsync(entity);
    }

    public Task<TEntity> DeleteAsync(TEntity entity)
    {
      throw new NotImplementedException();
    }

    public Task<TEntity> DeleteAsync(Guid id)
    {
      throw new NotImplementedException();
    }
  }
}
