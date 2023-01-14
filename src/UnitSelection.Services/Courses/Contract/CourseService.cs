using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.Courses.Contract.Dto;

namespace UnitSelection.Services.Courses.Contract;

public interface CourseService : Service
{
    Task Add(AddCourseDto dto);
    Task Update(UpdateCourseDto dto, int id);
    IList<GetCourseDto> GetAll();
    GetCourseByIdDto GetById(int id);
    GetCourseByClassIdDto GetByClassId(int classId);
}