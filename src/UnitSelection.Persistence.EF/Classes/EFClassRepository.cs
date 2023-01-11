using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.Classes;
using UnitSelection.Services.Classes.Contract;

namespace UnitSelection.Persistence.EF.Classes;

public class EFClassRepository : ClassRepository
{
    private readonly EFDataContext _context;

    public EFClassRepository(EFDataContext context)
    {
        _context = context;
    }

    public void Add(Class newClass)
    {
        _context.Add(newClass);
    }

    public bool IsNameExist(string name)
    {
        return _context.Classes
            .Any(_ => _.Name == name);
    }
}