using UnitSelection.Entities.Courses;
using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.Courses.Contract.Dto;

namespace UnitSelection.Services.Courses.Contract;

public interface CourseRepository :Repository
{
    void Add(Course course);
    void Update(Course dto);
    bool IsCourseNameExist(string name);
    Course FindById(int id);
}