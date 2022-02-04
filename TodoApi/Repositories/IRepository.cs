using System.Linq.Expressions;

namespace TodoApi.Repositories
{
  public interface IRepository<TEntity> where TEntity : class, new()
  {
    IQueryable<TEntity> GetQueryable();
    Task<List<TEntity>> GetListAsync();
    Task<int> GetCountAsync();
    Task<TEntity> GetAsync(Guid id);
    Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(Guid id);
    Task<TEntity> DeleteAsync(TEntity entity);
    Task<TEntity> DeleteAsync(Guid id);
  }
}
