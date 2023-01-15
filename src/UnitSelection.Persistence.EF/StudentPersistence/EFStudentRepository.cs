using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.Students;
using UnitSelection.Services.StudentServices.Contracts;

namespace UnitSelection.Persistence.EF.StudentPersistence;

public class EFStudentRepository : StudentRepository
{
    private readonly EFDataContext _context;

    public EFStudentRepository(EFDataContext context)
    {
        _context = context;
    }

    public void Add(Student student)
    {
        _context.Add(student);
    }

    public void Update(Student dto)
    {
        _context.Update(dto);
    }

    public Student? FindById(int id)
    {
        return _context.Students.FirstOrDefault(_ => _.Id == id);
    }

    public bool IsExistNationalCode(string nationalCode)
    {
        return _context.Students
            .Any(_ => _.NationalCode == nationalCode);
    }
}