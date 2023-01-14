using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.Courses;
using UnitSelection.Services.Courses.Contract;
using UnitSelection.Services.Courses.Contract.Dto;

namespace UnitSelection.Persistence.EF.CoursesPersistence;

public class EFCourseRepository : CourseRepository
{
    private readonly EFDataContext _context;

    public EFCourseRepository(EFDataContext context)
    {
        _context = context;
    }

    public void Add(Course course)
    {
        _context.Add(course);
    }

    public void Update(Course dto)
    {
        _context.Update(dto);
    }

    public bool IsCourseNameExist(string name)
    {
        return _context.Courses.Any(_ => _.Name == name);
    }

    public Course FindById(int id)
    {
        return _context.Courses.FirstOrDefault(_ => _.Id == id)!;
    }
}