using UnitSelection.Entities.Classes;
using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.Classes.Contract.Dto;

namespace UnitSelection.Services.Classes.Contract;

public interface ClassRepository : Repository
{
    void Add(Class newClass);
    void Update(Class newClass);
    IList<GetClassDto> GetAll();
    GetClassByIdDto GetById(int id);
    GetClassByTermIdDto? GetByTermId(int termId);
    void Delete(Class newClass);
    bool IsNameExist(string name, int termId);
    Class FindById(int id);
}