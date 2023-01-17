using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.ChooseUnits;
using UnitSelection.Entities.Classes;
using UnitSelection.Services.ClassServices.Contract;
using UnitSelection.Services.ClassServices.Contract.Dto;

namespace UnitSelection.Persistence.EF.ClassPersistence;

public class EFClassRepository : ClassRepository
{
    private readonly DbSet<Class> _classes;
    private readonly DbSet<ChooseUnit> _chooseUnits;

    public EFClassRepository(EFDataContext context)
    {
        _classes = context.Set<Class>();
        _chooseUnits = context.Set<ChooseUnit>();
    }

    public void Add(Class newClass)
    {
        _classes.Add(newClass);
    }

    public void Update(Class newClass)
    {
        _classes.Update(newClass);
    }

    public IList<GetClassDto> GetAll()
    {
        return _classes.Select(_ => new GetClassDto
        {
            Id = _.Id,
            Name = _.Name,
            TermId = _.TermId
        }).ToList();
    }

    public GetClassByIdDto GetById(int id)
    {
        return _classes
            .Where(_ => _.Id == id)
            .Select(_ => new GetClassByIdDto
            {
                Name = _.Name,
                TermId = _.TermId
            }).FirstOrDefault()!;
    }

    public GetClassByTermIdDto? GetByTermId(int termId)
    {
        return _classes
            .Where(_ => _.TermId == termId)
            .Include(_ => _.Term)
            .Select(_ => new GetClassByTermIdDto
            {
                Name = _.Name,
            }).FirstOrDefault();
    }

    public void Delete(Class newClass)
    {
        _classes.Remove(newClass);
    }

    public bool IsNameExist(string name, int termId)
    {
        return _classes
            .Any(_ => _.Name == name && _.TermId == termId);
    }

    public Class FindById(int id)
    {
        return _classes
            .FirstOrDefault(_ => _.Id == id);
    }

    public bool IsExistInChooseUnit(int courseId)
    {
        return _chooseUnits.Any(_ => _.ClassId == courseId);
    }
}