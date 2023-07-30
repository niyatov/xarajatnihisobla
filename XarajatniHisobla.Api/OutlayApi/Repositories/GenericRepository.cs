using OutlayApi.Data;
using System.Linq.Expressions;

namespace OutlayApi.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly AppDbContext _context;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
    }

    public async ValueTask AddRangeAsync(IEnumerable<TEntity> TEntity)
    {
        await _context.Set<TEntity>().AddRangeAsync(TEntity);
        await _context.SaveChangesAsync();
    }

    public async ValueTask<TEntity> AddAsync(TEntity entity)
    {
        var entry = await _context.Set<TEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        => _context.Set<TEntity>().Where(expression);

    public List<TEntity> GetAll()
        => _context.Set<TEntity>().ToList();

    public ValueTask<TEntity?> GetByIdAsync(int id)
        => _context.Set<TEntity>().FindAsync(id);

    public async ValueTask RemoveRange(IQueryable<TEntity> TEntity)
    {
        _context.Set<TEntity>().RemoveRange(TEntity);
        await _context.SaveChangesAsync();
    }

    public async ValueTask<TEntity> Remove(TEntity entity)
    {
        var entry = _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public async ValueTask<TEntity> Update(TEntity entity)
    {
        var entry = _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }
}