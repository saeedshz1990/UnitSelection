using UnitSelection.Entities.Classes;
using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.ClassServices.Contract.Dto;

namespace UnitSelection.Services.ClassServices.Contract;

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
    bool IsExistInChooseUnit(int courseId);
}