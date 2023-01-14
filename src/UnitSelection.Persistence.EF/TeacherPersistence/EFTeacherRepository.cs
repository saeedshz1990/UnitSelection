using UnitSelection.Entities.Teachers;
using UnitSelection.Services.Teachers.Contract;

namespace UnitSelection.Persistence.EF.TeacherPersistence;

public class EFTeacherRepository :TeacherRepository
{
    private readonly EFDataContext _context;

    public EFTeacherRepository(EFDataContext context)
    {
        _context = context;
    }

    public void Add(Teacher teacher)
    {
        _context.Add(teacher);
    }

    public bool IsExistNationalCode(string nationalCode)
    {
        return _context.Teachers.Any(_ => _.NationalCode == nationalCode);
    }
}