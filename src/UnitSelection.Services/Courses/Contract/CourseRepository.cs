using UnitSelection.Entities.Courses;
using UnitSelection.Infrastructure.Application;

namespace UnitSelection.Services.Courses.Contract;

public interface CourseRepository :Repository
{
    void Add(Course course);
    bool IsCourseNameExist(string name);
}