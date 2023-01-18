using System.Linq.Expressions;
using UnitSelection.Infrastructure;

namespace UnitSelection.Persistence.EF;

public class RepositoryBase<TKey, T> : IRepository<TKey, T> where T : class
{
    private readonly EFDataContext _context;

    public RepositoryBase(EFDataContext context)
    {
        _context = context;
    }

    public T Get(TKey id)
    {
        return _context.Find<T>(id);
    }

    public List<T> Get()
    {
        return _context.Set<T>().ToList();
    }

    public void Create(T entity)
    {
        _context.Add(entity);
    }

    public bool Exists(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Any(expression);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}