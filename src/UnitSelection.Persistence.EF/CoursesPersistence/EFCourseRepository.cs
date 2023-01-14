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

    public IList<GetCourseDto> GetAll()
    {
        return _context.Courses
            .Select(_ => new GetCourseDto
            {
                Id = _.Id,
                Name = _.Name,
                UnitCount = _.UnitCount,
                DayOfWeek = _.DayOfWeek,
                StartHour = _.StartHour,
                EndHour = _.EndHour,
                ClassId = _.ClassId
            }).ToList();
    }

    public GetCourseByIdDto GetById(int id)
    {
        return _context.Courses
            .Where(_ => _.Id == id)
            .Select(_ => new GetCourseByIdDto
            {
                Name = _.Name,
                DayOfWeek = _.DayOfWeek,
                UnitCount = _.UnitCount,
                StartHour = _.StartHour,
                EndHour = _.EndHour,
                ClassId = _.ClassId
            }).FirstOrDefault()!;
    }

    public GetCourseByClassIdDto GetByClassId(int classId)
    {
        return _context.Courses
            .Where(_ => _.ClassId == classId)
            .Select(_ => new GetCourseByClassIdDto
            {
                Name = _.Name,
                DayOfWeek = _.DayOfWeek,
                StartHour = _.StartHour,
                EndHour = _.EndHour,
                UnitCount = _.UnitCount
            }).FirstOrDefault()!;
    }

    public bool IsCourseNameExist(string name)
    {
        return _context.Courses.Any(_ => _.Name == name);
    }

    public Course FindById(int id)
    {
        return _context.Courses.FirstOrDefault(_ => _.Id == id)!;
    }

    public void Delete(Course course)
    {
        _context.Remove(course);
    }
}