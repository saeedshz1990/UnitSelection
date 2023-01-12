using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.Classes;
using UnitSelection.Services.Classes.Contract;
using UnitSelection.Services.Classes.Contract.Dto;

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

    public void Update(Class newClass)
    {
        _context.Update(newClass);
    }

    public IList<GetClassDto> GetAll()
    {
        return _context.Classes.Select(_ => new GetClassDto
        {
            Id = _.Id,
            Name = _.Name,
            TermId = _.TermId
        }).ToList();
    }

    public GetClassByIdDto GetById(int id)
    {
        return _context.Classes
            .Where(_ => _.Id == id)
            .Select(_ => new GetClassByIdDto
            {
                Name = _.Name,
                TermId = _.TermId
            }).FirstOrDefault()!;
    }

    public GetClassByTermIdDto? GetByTermId(int termId)
    {
        return _context.Classes
            .Where(_ => _.TermId == termId)
            .Include(_ => _.Term)
            .Select(_ => new GetClassByTermIdDto
            {
                Name = _.Name,
            }).FirstOrDefault();
    }

    public bool IsNameExist(string name, int termId)
    {
        return _context.Classes
            .Any(_ => _.Name == name && _.TermId == termId);
    }

    public Class FindById(int id)
    {
        return _context.Classes
            .FirstOrDefault(_ => _.Id == id);
    }
}