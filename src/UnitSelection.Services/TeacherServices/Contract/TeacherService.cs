using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.TeacherServices.Contract.Dto;

namespace UnitSelection.Services.TeacherServices.Contract;

public interface TeacherService : Service
{
    Task Add(AddTeacherDto dto);
    IList<GetTeacherDto> GetAll();
    GetTeacherByIdDto GetById(int id);
    GetTeacherByCourseIdDto GetByCourseId(int courseId);
    Task Delete(int id);
    Task Update(UpdateTeacherDto dto, int id);
}