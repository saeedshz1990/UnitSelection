using UnitSelection.Entities.ChooseUnits;
using UnitSelection.Entities.Courses;
using UnitSelection.Entities.Students;
using UnitSelection.Services.ChooseUnitServices.Contracts;

namespace UnitSelection.Persistence.EF.ChooseUnitPersistence;

public class EFChooseUnitRepository : ChooseUnitRepository
{
    private readonly EFDataContext _context;

    public EFChooseUnitRepository(EFDataContext context)
    {
        _context = context;
    }

    public void Add(ChooseUnit chooseUnit)
    {
        _context.Add(chooseUnit);
    }

    public Student GetStudent(int studentId)
    {
        return _context.Students.FirstOrDefault(_ => _.Id == studentId)!;
    }

    public Student? FindByStudentId(int studentId)
    {
        return _context.Students.FirstOrDefault(_ => _.Id == studentId);
    }
}