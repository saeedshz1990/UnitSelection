using UnitSelection.Entities.Classes;
using UnitSelection.Infrastructure.Application;

namespace UnitSelection.Services.Classes.Contract;

public interface ClassRepository : Repository
{
    void Add(Class newClass);
    void Update(Class newClass);

    bool IsNameExist(string name);
    Class FindById(int id);
}