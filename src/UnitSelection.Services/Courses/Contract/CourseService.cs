using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.Courses.Contract.Dto;

namespace UnitSelection.Services.Courses.Contract;

public interface CourseService :Service
{
    Task Add(AddCourseDto dto);
}