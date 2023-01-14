using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.Teachers.Contract.Dto;

namespace UnitSelection.Services.Teachers.Contract;

public interface TeacherService : Service
{
    Task Add(AddTeacherDto dto);
    IList<GetTeacherDto> GetAll();
    GetTeacherByIdDto GetById(int id);
    GetTeacherByCourseIdDto GetByCourseId(int courseId);
}