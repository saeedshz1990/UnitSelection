using UnitSelection.Infrastructure.Application;

namespace UnitSelection.Persistence.EF;

public class EFUnitOfWork : UnitOfWork
{
    private readonly EFDataContext _context;

    public EFUnitOfWork(EFDataContext context)
    {
        _context = context;
    }

    public async Task Begin()
    {
        await _context.Database.BeginTransactionAsync();
    }

    public async Task Complete()
    {
        await _context.SaveChangesAsync();
    }
}