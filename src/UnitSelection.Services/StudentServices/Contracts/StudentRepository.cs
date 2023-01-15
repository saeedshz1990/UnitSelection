using UnitSelection.Entities.Students;
using UnitSelection.Infrastructure.Application;

namespace UnitSelection.Services.StudentServices.Contracts;

public interface StudentRepository : Repository
{
    void Add(Student student);
    void Update(Student dto);

    Student? FindById(int id);
    bool IsExistNationalCode(string nationalCode);
}