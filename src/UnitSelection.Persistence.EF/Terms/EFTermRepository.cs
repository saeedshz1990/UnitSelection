using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.Terms;
using UnitSelection.Services.Terms;
using UnitSelection.Services.Terms.Contract;

namespace UnitSelection.Persistence.EF.Terms;

public class EFTermRepository : TermRepository
{
    private readonly EFDataContext _context;

    public EFTermRepository(EFDataContext context)
    {
        _context = context;
    }

    public void Add(Term term)
    {
        _context.Add(term);
    }

    public void Update(Term term)
    {
        _context.Update(term);
    }

    public bool IsNameExist(string name)
    {
        return _context.Terms.Any(_ => _.Name == name);
    }

    public Term FindById(int id)
    {
        return _context.Terms
            .FirstOrDefault(_ => _.Id == id)!;
    }
}