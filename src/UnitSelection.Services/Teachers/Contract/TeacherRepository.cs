using UnitSelection.Entities.Teachers;
using UnitSelection.Infrastructure.Application;

namespace UnitSelection.Services.Teachers.Contract;

public interface TeacherRepository : Repository
{
    void Add(Teacher teacher);
    bool IsExistNationalCode(string nationalCode);
}