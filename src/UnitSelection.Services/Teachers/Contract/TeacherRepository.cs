using UnitSelection.Entities.Teachers;
using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.Teachers.Contract.Dto;

namespace UnitSelection.Services.Teachers.Contract;

public interface TeacherRepository : Repository
{
    void Add(Teacher teacher);
    IList<GetTeacherDto> GetAll();
    GetTeacherByIdDto GetById(int id);
    GetTeacherByCourseIdDto GetTeacherByCourseId(int courseId);
    void Delete(Teacher teacher);
    void Update(Teacher teacher);
    Teacher? FindById(int id);
    bool IsExistNationalCode(string nationalCode);
}