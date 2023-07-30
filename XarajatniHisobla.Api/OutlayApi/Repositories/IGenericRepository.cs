using System.Linq.Expressions;

namespace OutlayApi.Repositories;

public interface IGenericRepository<TEntity> where TEntity : class
{
    List<TEntity> GetAll();
    ValueTask<TEntity> AddAsync(TEntity entity);
    ValueTask AddRangeAsync(IEnumerable<TEntity> TEntity);
    ValueTask<TEntity?> GetByIdAsync(int id);
    ValueTask<TEntity> Remove(TEntity entity);
    ValueTask RemoveRange(IQueryable<TEntity> TEntity);
    IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression);
    ValueTask<TEntity> Update(TEntity entity);
}