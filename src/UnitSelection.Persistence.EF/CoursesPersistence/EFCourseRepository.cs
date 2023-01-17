using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.ChooseUnits;
using UnitSelection.Entities.Courses;
using UnitSelection.Services.CourseServices.Contract;
using UnitSelection.Services.CourseServices.Contract.Dto;

namespace UnitSelection.Persistence.EF.CoursesPersistence;

public class EFCourseRepository : CourseRepository
{
    private readonly DbSet<Course> _courses;
    private readonly DbSet<ChooseUnit> _chooseUnits;

    public EFCourseRepository(EFDataContext context)
    {
        _courses = context.Set<Course>();
        _chooseUnits = context.Set<ChooseUnit>();
    }

    public void Add(Course course)
    {
        _courses.Add(course);
    }

    public void Update(Course dto)
    {
        _courses.Update(dto);
    }

    public IList<GetCourseDto> GetAll()
    {
        return _courses
            .Select(_ => new GetCourseDto
            {
                Id = _.Id,
                Name = _.Name,
                UnitCount = _.UnitCount,
                DayOfWeek = _.DayOfWeek,
                StartHour = _.StartHour,
                EndHour = _.EndHour,
                ClassId = _.ClassId,
                GroupOfCourse = _.GroupOfCourse
            }).ToList();
    }

    public GetCourseByIdDto GetById(int id)
    {
        return _courses
            .Where(_ => _.Id == id)
            .Select(_ => new GetCourseByIdDto
            {
                Name = _.Name,
                DayOfWeek = _.DayOfWeek,
                UnitCount = _.UnitCount,
                StartHour = _.StartHour,
                EndHour = _.EndHour,
                ClassId = _.ClassId,
                GroupOfCourse = _.GroupOfCourse
            }).FirstOrDefault()!;
    }

    public GetCourseByClassIdDto GetByClassId(int classId)
    {
        return _courses
            .Where(_ => _.ClassId == classId)
            .Select(_ => new GetCourseByClassIdDto
            {
                Name = _.Name,
                DayOfWeek = _.DayOfWeek,
                StartHour = _.StartHour,
                EndHour = _.EndHour,
                UnitCount = _.UnitCount,
                GroupOfCourse = _.GroupOfCourse
            }).FirstOrDefault()!;
    }

    public bool IsCourseNameExist(string name)
    {
        return _courses.Any(_ => _.Name == name);
    }

    public Course FindById(int id)
    {
        return _courses.FirstOrDefault(_ => _.Id == id)!;
    }

    public void Delete(Course course)
    {
        _courses.Remove(course);
    }

    public Course GetCourseById(int id)
    {
        return _courses
            .Include(_ => _.Teachers)
            .Include(_ => _.Class)
            .FirstOrDefault(_ => _.Id == id)!;
    }

    public bool IsExistInChooseUnit(int courseId)
    {
        return _chooseUnits.Any(_ => _.CourseId == courseId);
    }
}