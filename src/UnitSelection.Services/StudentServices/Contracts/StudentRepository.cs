using UnitSelection.Entities.Students;
using UnitSelection.Infrastructure.Application;

namespace UnitSelection.Services.StudentServices.Contracts;

public interface StudentRepository :Repository
{
    void Add(Student student);

    bool IsExistNationalCode(string nationalCode);
}