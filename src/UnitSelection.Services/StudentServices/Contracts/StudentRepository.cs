using UnitSelection.Entities.Students;
using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.StudentServices.Contracts.Dto;

namespace UnitSelection.Services.StudentServices.Contracts;

public interface StudentRepository : Repository
{
    void Add(Student student);
    void Update(Student dto);
    IList<GetStudentDto> GetAll();
    GetStudentByIdDto GetById(int id);
    Student? FindById(int id);
    bool IsExistNationalCode(string nationalCode);
}