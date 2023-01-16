using UnitSelection.Entities.Courses;
using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.CourseServices.Contract.Dto;

namespace UnitSelection.Services.CourseServices.Contract;

public interface CourseService : Service
{
    Task Add(AddCourseDto dto);
    Task Update(UpdateCourseDto dto, int id);
    IList<GetCourseDto> GetAll();
    GetCourseByIdDto GetById(int id);
    GetCourseByClassIdDto GetByClassId(int classId);
    Task Delete(int id);

    Course GetCourseById(int id);
}