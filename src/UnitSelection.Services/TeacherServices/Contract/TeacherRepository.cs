using UnitSelection.Entities.Teachers;
using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.TeacherServices.Contract.Dto;

namespace UnitSelection.Services.TeacherServices.Contract;

public interface TeacherRepository : Repository
{
    void Add(Teacher teacher);
    IList<GetTeacherDto> GetAll();
    GetTeacherByIdDto GetById(int id);
    GetTeacherByCourseIdDto GetTeacherByCourseId(int courseId);
    void Delete(Teacher teacher);
    void Update(Teacher teacher);
    Teacher? FindById(int id);
    bool IsExistById(int id);
    bool IsExistChooseUnit(int teacherId);
    bool IsExistByNationalCode(string nationalCode);
}